using GCDC.Common.Helpers;
using GCDC.Common.Models.Api.Request;
using GCDC.Common.Models.Api.Response;
using GCDC.Common.Models.CMS;
using GCDC.Core.Repositories.Common;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using CommonHelpers = GCDC.Common.Helpers.Common;


namespace GCDC.Core.Repositories.Api
{
    public class APIRepository : AbstractRepository
    {
        public APIRepository(ILogger<APIRepository> logger, IBaseControllerFactory factory) : base(logger, factory)
        {

        }

        public SearchListing GetSearchData(SearchBody request)
        {
            using (new FunctionTracer())
            {
                int totalRecords = 0;
                SearchListing RetVal = null;
                try
                {
                    if (string.IsNullOrEmpty(request.Culture))
                        request.Culture = Constants.Languages.Default;

                    VariationContextAccessor.VariationContext = new VariationContext(request.Culture);


                    var contentType = request?.ContentType;
                    if (!string.IsNullOrEmpty(contentType))
                    {
                        var dataSources = UmbracoContext.Content?.GetAtRoot().FirstOrDefault(x => x.ContentType.Alias == Constants.RootAlias.DataSources);

                        IQueryable<IPublishedContent>? query = null;

                        if (contentType != null)
                        {
                            var contentNode = dataSources?.Children(VariationContextAccessor).Where(c => c.ContentType.Alias == contentType && c.IsPublished() && c.HasCulture(request.Culture)).FirstOrDefault();
                            query = contentNode?.Children(VariationContextAccessor)?.Where(c => c.IsPublished() && c.HasCulture(request.Culture) && c.Cultures.ContainsKey(request.Culture)).AsQueryable();
                        }

                        query = CommonHelpers.ApplyFilters(request, query);

                        totalRecords = query.Count();
                        // Apply pagination if not skipped
                        if (!request.SkipPagination && request.PageSize.HasValue)
                        {
                            query = query.Skip(request.Skip).Take(request.PageSize.Value);
                        }
                        var filteredContent = query.ToList();

                        //List<Object> records = new List<Object>();

                        if (filteredContent != null && filteredContent.Any())
                        {
                            if (contentType == Constants.ContentTypes.News)
                            {
                                List<NewsItem> news = filteredContent.OfType<NewsItem>().ToList();
                                RetVal = new SearchListing(news, totalRecords, request);
                            }
                            /* else if (contentType == Constants.ContentTypes.FreqAskedQuestions)
                             {
                                 List<FAqsItem> faqs = filteredContent.OfType<FAqsItem>().ToList();
                                 RetVal = new SearchListing(faqs, totalRecords, request);
                             }*/
                            else if (contentType == Constants.ContentTypes.ImagesVideos)
                            {
                                List<MediaGalleryItem> media = filteredContent.OfType<MediaGalleryItem>().ToList();
                                RetVal = new SearchListing(media, totalRecords, request);
                            }
                        }

                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return RetVal;
            }
        }

        public SearchListing LoadData(SearchBody request)
        {
            using (new FunctionTracer())
            {
                int totalRecords = 0;
                SearchListing RetVal = null;
                try
                {
                    if (string.IsNullOrEmpty(request.Culture))
                        request.Culture = Constants.Languages.Default;

                    VariationContextAccessor.VariationContext = new VariationContext(request.Culture);

                    var contentType = request?.ContentType;
                    if (!string.IsNullOrEmpty(contentType))
                    {
                        var dataSources = UmbracoContext.Content?
                            .GetAtRoot()
                            .FirstOrDefault(x => x.ContentType.Alias == Constants.RootAlias.DataSources);

                        IQueryable<IPublishedContent>? query = null;

                        if (contentType != null)
                        {
                            var contentNode = dataSources?.Children
                                .Where(c => c.ContentType.Alias == contentType
                                            && c.IsPublished()
                                            && c.HasCulture(request.Culture))
                                .FirstOrDefault();

                            query = contentNode?.Children?
                                .Where(c => c.IsPublished()
                                            && c.HasCulture(request.Culture)
                                            && c.Cultures.ContainsKey(request.Culture))
                                .AsQueryable();
                        }

                        query = CommonHelpers.ApplyFilters(request, query);
                        totalRecords = query.Count();

                        if (!request.SkipPagination)
                        {
                            request.PageSize ??= 3;
                            query = query.Skip(request.Skip).Take(request.PageSize.Value);
                        }

                        var filteredContent = query.ToList();

                        if (filteredContent != null && filteredContent.Any())
                        {
                            if (contentType == Constants.ContentTypes.News)
                            {
                                List<NewsItem> news = filteredContent.OfType<NewsItem>().ToList();
                                RetVal = new SearchListing(news, totalRecords, request);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return RetVal;
            }
        }

    }
}
