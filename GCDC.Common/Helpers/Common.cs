using GCDC.Common.Models.Api.Request;
using GCDC.Common.Models.Api.Request.CRM_Request;
using GCDC.Common.Models.CMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using NUglify.JavaScript.Visitors;
using Serilog;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common;
using static Umbraco.Cms.Core.Constants.Conventions;

namespace GCDC.Common.Helpers
{
    public static class Common
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static IConfigurationManager configuration;
        public static ILogger _logger = Log.Logger;
        public static Site Root { get; set; }
        private static IUmbracoContext umbracoContext { get; set; }
        public static UmbracoHelper umbracoHelper { get; private set; }
        public static IContentService contentService { get; private set; }
        public static ILocalizationService localizationservice { get; private set; }
        private static IHttpContextAccessor httpContextAccessor;
        public static HttpContext HttpContext { get; private set; }
        public static HttpRequest HttpRequest { get; private set; }
        public static HttpResponse HttpResponse { get; private set; }
        public static List<LanguageItem> SupportedLanguages { get; set; }

        public static void Initialize(IConfigurationManager _configuration)
        {
            _logger.Information("Initialize");
            configuration = _configuration;
        }

        public static string GetCRMUrl()
        {
            string RetVal = "";
            RetVal = configuration["CRMForms:CRMUrl"];
            return RetVal;
        }

        public static string GetGeneralInquiryFormId()
        {
            string RetVal = "";
            RetVal = configuration["CRMForms:FormIds:GeneralInquiry"];
            return RetVal;
        }

        public static string GetCareersFormId()
        {
            string RetVal = "";
            RetVal = configuration["CRMForms:FormIds:Careers"];
            return RetVal;
        }

        public static string GetMediaPressFormId()
        {
            string RetVal = "";
            RetVal = configuration["CRMForms:FormIds:MediaPress"];
            return RetVal;
        }

        public static string GetInvestorFormId()
        {
            string RetVal = "";
            RetVal = configuration["CRMForms:FormIds:Investor"];
            return RetVal;
        }

        public static string GetVendorRegistrationFormId()
        {
            string RetVal = "";
            RetVal = configuration["CRMForms:FormIds:VendorRegistration"];
            return RetVal;
        }

        public static void SetGlocalVariable(IUmbracoContext _umbracoContext, UmbracoHelper _umbracoHelper, IContentService _contentService, ILocalizationService _localizationservice, IHttpContextAccessor _httpContextAccessor)
        {
            umbracoContext = _umbracoContext;
            umbracoHelper = _umbracoHelper;
            contentService = _contentService;
            localizationservice = _localizationservice;
            httpContextAccessor = _httpContextAccessor;
            HttpContext = httpContextAccessor?.HttpContext;
            HttpRequest = httpContextAccessor?.HttpContext?.Request;
            HttpResponse = httpContextAccessor?.HttpContext?.Response;
            SupportedLanguages = GetSupportedLanguages();

        }

        private static List<LanguageItem> GetSupportedLanguages()
        {
            List<LanguageItem> _SupportedLanguages = new List<LanguageItem>();
            _SupportedLanguages.Clear();
            IEnumerable<ILanguage> languages = localizationservice.GetAllLanguages();
            foreach (ILanguage language in languages)
            {
                // Get the .NET culture info
                CultureInfo cultureInfo = language.CultureInfo;
                _SupportedLanguages.Add(new LanguageItem
                {
                    LanguageCode = cultureInfo.TwoLetterISOLanguageName,
                    LanguageName = cultureInfo.DisplayName
                });
            }
            return _SupportedLanguages;
        }

        public static string? GetMediaUrl(this MediaWithCrops mediaWithCrops, string? cropAlias = null)
        {
            if (mediaWithCrops == null)
                return "";

            return string.IsNullOrEmpty(cropAlias)
                ? mediaWithCrops.MediaUrl() // Original image URL
                : mediaWithCrops.GetCropUrl(cropAlias); // Cropped image URL
        }

        public static string LanguageSwitcher(HttpRequest request)
        {
            string fullUrl = request != null ? request.GetDisplayUrl() : "";
            Uri uri = new Uri(fullUrl);

            // Extract the relative path
            string ctxUrl = uri.PathAndQuery;

            string culture = string.Empty;

            if (Constants.Languages.Current == "en")
                culture = "ar";
            else if (Constants.Languages.Current == "ar")
                culture = "en";

            Constants.currentCulture = new System.Globalization.CultureInfo(Constants.Languages.Current);
            if (ctxUrl.Contains("/ar/") || ctxUrl.Contains("/ar"))
            {
                if (ctxUrl.Contains("/ar/"))
                    ctxUrl = ctxUrl.Replace("/ar/", "/" + culture + "/");
                else
                    ctxUrl = ctxUrl.Replace("/ar", "/" + culture);
            }
            else if (ctxUrl.Contains("/en/") || ctxUrl.Contains("/en"))
            {
                if (ctxUrl.Contains("/en/"))
                    ctxUrl = ctxUrl.Replace("/en/", "/" + culture + "/");
                else
                    ctxUrl = ctxUrl.Replace("/en", "/" + culture);
            }
            else
            {
                // Ensure ctxUrl starts with "/" before concatenating
                if (!ctxUrl.StartsWith("/"))
                    ctxUrl = "/" + ctxUrl;

                // Correct the concatenation here to add "/ar" before the path
                ctxUrl = "/" + culture + "/" + ctxUrl.TrimStart('/');
            }

            // Reconstruct the full URL with the updated path
            return $"{uri.Scheme}://{uri.Host}:{uri.Port}{ctxUrl}";
        }

        /// <summary>
        /// If translation is not found in dictionary/translation section, function will return KEY as value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string? GetDictionaryValueOrDefault(string? key)
        {
            string? result = key;
            if (umbracoHelper != null)
            {
                result = umbracoHelper.GetDictionaryValueOrDefault(key, key);
            }
            return result;
        }

        public static IPublishedContent GetParentNode(string ParentNodeAlias)
        {
            IPublishedContent response = null;
            var parentNode = umbracoHelper.ContentAtRoot().FirstOrDefault(x => x.ContentType.Alias == ParentNodeAlias);
            if (parentNode != null)
            {
                response = parentNode;
            }
            return response;
        }

        public static IPublishedContent GetChildNodesFromParent(string ParentAlias, string ChildAlias)
        {
            IPublishedContent response = null;
            var parent = GetParentNode(ParentAlias);
            if (parent != null && parent.Children != null && parent.Children.Any())
            {
                response = parent.Children.FirstOrDefault(x => x.ContentType.Alias == ChildAlias);
            }
            return response;
        }

        public static IQueryable<IPublishedContent> ApplyFilters(SearchBody request, IQueryable<IPublishedContent> query)
        {
            // Apply category and search filters
            query = ApplyContentTypeFilters(request, query);

            // Apply additional filters
            query = ApplyAdditionalFilters(request, query);

            return query;
        }

        private static IQueryable<IPublishedContent> ApplyContentTypeFilters(SearchBody request, IQueryable<IPublishedContent> query)
        {
            if (request.ContentType == Constants.ContentTypes.News || request.ContentType == Constants.ContentTypes.ImagesVideos)
            {
                if (!string.IsNullOrEmpty(request.Category))
                {
                    query = request.Category.Equals("All", StringComparison.OrdinalIgnoreCase)
                        ? GetChildren(query)
                        : query.Where(category => category.Name.Equals(request.Category, StringComparison.OrdinalIgnoreCase))
                               .SelectMany(category => category.Children);
                }

                if (request.SearchQuery != null && request.SearchQuery.Count > 0)
                {
                    foreach (var indfilter in request.SearchQuery)
                    {
                        if (indfilter.Key == Constants.QueryFilters.Search && !string.IsNullOrEmpty(indfilter.Value))
                        {
                            query = FilterByValues(query, indfilter.Value, content =>
                                request.ContentType == Constants.ContentTypes.News
                                    ? new[] { content.Value<string>("headline"), content.Value<string>("paragraph") }
                                    : new[] { content.Value<string>("title") });
                        }
                    }
                }
            }

            return query;
        }

        private static IQueryable<IPublishedContent> ApplyAdditionalFilters(SearchBody request, IQueryable<IPublishedContent> query)
        {
            if (request.Filters == null || !request.Filters.Any())
                return query;

            foreach (var filter in request.Filters)
            {
                var filterKey = filter.Key?.ToLower();
                var filterValue = filter.Value?.ToLower();

                if (string.IsNullOrEmpty(filterKey) || string.IsNullOrEmpty(filterValue))
                    continue;

                switch (filterKey)
                {
                    case var key when key == Constants.Filters.Year.ToLower():
                        query = ApplyYearFilter(query, filterValue);
                        break;

                    case var key when key == Constants.Filters.TagsItem.ToLower():
                        query = ApplyTagsFilter(query, filterValue);
                        break;

                    default:
                        return query.Where(c => false); // Return empty if filter is invalid
                }
            }

            return query;
        }

        private static IQueryable<IPublishedContent> ApplyYearFilter(IQueryable<IPublishedContent> query, string filterValue)
        {
            var yearValues = GetFilterValues(filterValue);

            return query.ToList().Where(item =>
            {
                var date = item.Value<DateTime?>("date");
                return date.HasValue && yearValues.Contains(date.Value.Year.ToString());
            }).AsQueryable();
        }

        private static IQueryable<IPublishedContent> ApplyTagsFilter(IQueryable<IPublishedContent> query, string filterValue)
        {
            if (string.IsNullOrWhiteSpace(filterValue))
                return query;

            var tagValues = GetFilterValues(filterValue, true); // e.g., ["sports", "fitness"]

            var items = query.ToList(); // Materialize for safety with Umbraco content

            var filteredItems = items.Where(item =>
            {
                var tags = item.Value<IEnumerable<IPublishedContent>>("tags");

                if (tags == null)
                    return false;

                return tags.Any(tag =>
                {
                    var tagName = tag?.Value<string>("TagsName");
                    return tagName != null && tagValues.Contains(tagName.ToLowerInvariant());
                });
            });

            return filteredItems.AsQueryable();
        }


        private static List<string> GetFilterValues(string filterValue, bool toLower = false)
        {
            if (string.IsNullOrEmpty(filterValue))
                return new List<string>();

            var values = filterValue.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(v => toLower ? v.Trim().ToLower() : v.Trim())
                                    .ToList();

            return values;
        }
        public static IQueryable<IPublishedContent> FilterByNodeFieldValue(IQueryable<IPublishedContent> query, string filterValue, string filterseparator, string nodeAlias, string fieldAlias, string culture)
        {
            string[] valueList = filterValue
                                       .Split(filterseparator, StringSplitOptions.RemoveEmptyEntries)
                                       .Select(s => s.Trim())
                                       .ToArray();
            if (valueList != null && valueList.Any())
            {
                query = query.AsEnumerable()
                    .Where(content =>
                    {
                        var node = (IPublishedContent)content.Value<IPublishedElement>(nodeAlias, culture);
                        var nodeValue = node?.Value<string>(fieldAlias);
                        return !string.IsNullOrEmpty(nodeValue) && valueList.Contains(nodeValue);
                    })
                    .AsQueryable();
            }

            return query;
        }

        /// <summary>
        /// Extracts distinct values from a list of objects based on a property selector.
        /// </summary>
        /// <typeparam name="T">Type of objects in the list.</typeparam>
        /// <typeparam name="TProperty">Type of the property to extract.</typeparam>
        /// <param name="items">List of objects.</param>
        /// <param name="selector">Function to select the desired property.</param>
        /// <returns>List of distinct values.</returns>
        public static List<TProperty> GetDistinctValues<T, TProperty>(List<T> items, Func<T, TProperty> selector)
        {
            return items?
                .Select(selector)
                .Where(value => value != null) // Remove null values
                .Distinct()
                .ToList() ?? new List<TProperty>();
        }

        public static Dictionary<TKey, List<T>> GroupItemsByProperty<T, TKey>(List<T> items, Func<T, TKey> keySelector)
        {
            return items
                .Where(item => item != null)
                .GroupBy(keySelector)
                .ToDictionary(group => group.Key, group => group.ToList());
        }

        public static string GetCrmAuthorizationHeader()
        {
            string RetVal = string.Empty;
            if (Root != null)
            {
                string credentials = $"{Root.HeaderUsername}:{Root.HeaderPassword}";
                string base64Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
                RetVal = $"Basic {base64Credentials}";
            }
            return RetVal;
        }

        public static async Task<(string response, bool isSuccess)> CallCrmApiAsync(string url, FormData formData)
        {
            try
            {
                string jsonBody = JsonSerializer.Serialize(formData);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = content;
                request.Headers.Add("Authorization", GetCrmAuthorizationHeader());
                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                return (responseContent, true);
            }
            catch (HttpRequestException httpEx)
            {
                return ($"Error: {httpEx.Message}", false);
            }
            catch (Exception ex)
            {
                return ($"Unexpected Error: {ex.Message}", false);
            }
        }

        public static int GetNodeId(string NodeAlias)
        {
            var parentNode = umbracoHelper.ContentAtRoot()
                                          .SelectMany(x => x.DescendantsOrSelf())
                                          .FirstOrDefault(x => x.ContentType.Alias == NodeAlias);

            return parentNode != null ? parentNode.Id : 0;
        }

        public static int GetFormSubmissionNodeId(string formId)
        {
            var parentNode = umbracoHelper.ContentAtRoot()
                                          .SelectMany(x => x.DescendantsOrSelf())
                                          .FirstOrDefault(x => x.ContentType.Alias == Constants.RootAlias.FormSubmissionNode);

            if (parentNode != null)
            {
                var formNode = parentNode.Children.Where(c => c.ContentType.Alias == Constants.RootAlias.FormCollectionsNode && c.Value<string>("formId") == formId).FirstOrDefault();
                return formNode != null ? formNode.Id : 0;
            }

            return 0;
        }

        public static List<FieldData> ConvertToList(object data)
        {
            List<FieldData> fieldDataList = data.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance) // Get properties instead of fields
                .Where(p => p.PropertyType == typeof(FieldData))
                .Select(p => (FieldData)p.GetValue(data))
                .ToList();

            return fieldDataList;
        }

        private static IQueryable<IPublishedContent> GetChildren(IQueryable<IPublishedContent> query)
        {
            return query.SelectMany(category => category.Children);
        }

        public static string? GetDetailPageUrl(this IPublishedContent content)
        {
            NewsItem item = content as NewsItem;
            if (item == null)
                return "";

            return HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + "/" + Constants.currentCulture + "/" + Constants.ContentTypes.SearchText + "/" + HttpUtility.HtmlEncode(item.Slug);
        }

        public static string GetFileSizeUnit(long fileSizeInBytes)
        {
            const long OneKB = 1024;
            const long OneMB = OneKB * 1024;
            const long OneGB = OneMB * 1024;

            string UnitBytes = GetDictionaryValueOrDefault(Constants.Labels.Bytes);
            string UnitKB = GetDictionaryValueOrDefault(Constants.Labels.KB);
            string UnitMB = GetDictionaryValueOrDefault(Constants.Labels.MB);
            string UnitGB = GetDictionaryValueOrDefault(Constants.Labels.GB);

            if (fileSizeInBytes >= OneGB)
                return $"{(fileSizeInBytes / (double)OneGB):F2} {UnitGB}";
            else if (fileSizeInBytes >= OneMB)
                return $"{(fileSizeInBytes / (double)OneMB):F2} {UnitMB}";
            else if (fileSizeInBytes >= OneKB)
                return $"{(fileSizeInBytes / (double)OneKB):F2} {UnitKB}";
            else
                return $"{fileSizeInBytes} {UnitBytes}";
        }

        public static (string FileExtension, long FileSize) GetMediaPickerFileDetails(MediaWithCrops fileField)
        {
            if (fileField == null)
                return (string.Empty, 0);

            // Get the file URL
            var fileUrl = fileField.Url();
            if (string.IsNullOrEmpty(fileUrl))
                return (string.Empty, 0);

            // Get the file extension
            string fileExtension = Path.GetExtension(fileUrl).TrimStart('.');

            // Get the file size (if the media contains this information)
            long fileSize = fileField.Value<long>("umbracoBytes");

            return (fileExtension, fileSize);
        }

        public static string GetHeaderLogoPath(string logofile)
        {
            string logo_path = "/assets/img/brand/logo.svg";
            if (!string.IsNullOrEmpty(logofile))
                logo_path = logofile;
            return HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + logo_path;
        }

        public static string GetFooterLogoPath(string logofile)
        {
            string logo_path = "/assets/img/brand/logo.svg";
            if (!string.IsNullOrEmpty(logofile))
                logo_path = logofile;
            return HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + logo_path;
        }

        public static string ConcateBaseUrl(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return string.Empty;
            return HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + filePath;
        }

        public static bool HasPropertyWithValue(object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrWhiteSpace(propertyName))
                return false;

            // Get the type of the object
            Type type = obj.GetType();

            // Find the property by name
            PropertyInfo property = type.GetProperty(propertyName);
            if (property == null)
                return false; // Property doesn't exist

            // Get the value of the property
            try
            {
                var value = property.GetValue(obj);
                return value != null; // True if the property exists and has a value
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public static IQueryable<T> FilterByValues<T>(IQueryable<T> query, string filterValue, Func<T, string[]> getFields)
        {
            if (string.IsNullOrEmpty(filterValue) || getFields == null)
                return query;

            var valueList = filterValue
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToArray();

            return query.AsEnumerable()
                .Where(content =>
                {
                    var fields = getFields(content);
                    return fields.Any(field =>
                        !string.IsNullOrEmpty(field) && valueList.Any(val => field.Contains(val, StringComparison.OrdinalIgnoreCase))
                    );
                })
                .AsQueryable();
        }

        public static string GetHtmlEncodedString(this string value)
        {
            string RetVal = "";
            string pattern = @"[^a-zA-Z0-9]"; // Allows only letters (uppercase/lowercase) and digits            
            RetVal = Regex.Replace(value, pattern, "");
            RetVal = RetVal.Replace(" ", "");
            RetVal = HttpUtility.UrlPathEncode(RetVal);
            return RetVal;
        }

        public static string GetAppVersion()
        {
            var version = Assembly.GetEntryAssembly()?.GetName().Version;
            return version.ToString();
        }

        public static IPublishedContent GetPopupModel(string url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url) || !url.Contains("#"))
                    return null;

                var beforeHash = url.Split('#')[0].Trim('/');
                if (string.IsNullOrEmpty(beforeHash))
                    return null;

                var pathSegments = beforeHash.Split('/', StringSplitOptions.RemoveEmptyEntries);

                var root = umbracoHelper.ContentAtRoot()?.FirstOrDefault(x => x.Name == "Components");
                if (root == null)
                    return null;

                IPublishedContent current = (IPublishedContent)root;
                foreach (var segment in pathSegments)
                {
                    current = current?.Children()?.FirstOrDefault(x => x.UrlSegment == segment);
                    if (current == null)
                        return null;
                }

                return current;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetPopupModel");
                return null;
            }
        }
    }
}
