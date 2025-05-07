using GCDC.Common.Helpers;
using GCDC.Common.Models.Api.Request;
using GCDC.Common.Models.Api.Request.CRM_Request;
using GCDC.Common.Models.Api.Response;
using GCDC.Common.Models.CRM;
using GCDC.Core.Repositories.Api;
using GCDC.Core.Repositories.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace GCDC.Web.Controllers.Api
{
    [ApiController]

    [Route("api")]
    public class APIController : ControllerBase
    {
        private readonly ILogger<APIController> _logger;
        private readonly APIRepository _repository;
        private readonly FormsRepository _formsRepository;
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public APIController(
            ILogger<APIController> logger,
            IBaseControllerFactory factory,
            APIRepository repository,
            FormsRepository formrepository,
            IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _repository = repository;
            _formsRepository = formrepository;
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }


        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchBody request)

        {
            using (new FunctionTracer())
            {
                try
                {
                    var response = _repository.GetSearchData(request);
                    if (response != null)
                    {
                        string contentType = request?.ContentType;
                        var viewpath = string.Empty;
                        /*                        object viewModel = null;
                        */
                        if (contentType == GCDC.Common.Helpers.Constants.ContentTypes.News)
                        {

                            string componentViewName = "_newsItem.cshtml";
                            viewpath = Common.Helpers.Constants.ComponentViewLocation + componentViewName;
                        }
                        /* else if (contentType == GCDC.Common.Helpers.Constants.ContentTypes.FreqAskedQuestions)
                         {
                             viewpath = "~/Views/Partials/Components/_fAQApi.cshtml";
                         }*/
                        else if (contentType == GCDC.Common.Helpers.Constants.ContentTypes.ImagesVideos)
                        {
                            string componentViewName = "_GalleryItem.cshtml";
                            viewpath = Common.Helpers.Constants.ComponentViewLocation + componentViewName;
                        }

                        if (viewpath != null && viewpath.Trim().Length > 0)
                        {
                            var renderedView = await RenderViewToStringAsync(viewpath, response);
                            return Ok(new { statusCode = HttpStatusCode.OK, HtmlContent = renderedView, HasNextPage = response.HasNextPage, TotalResults = response.TotalResults, TotalPages = response.TotalPages });

                        }
                        else
                        {
                            return BadRequest(ApiResponse.BadRequest("No View to Bind."));
                        }
                    }

                    return BadRequest(ApiResponse.BadRequest("No search data found."));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return BadRequest(ApiResponse.ExpectationFailed(ex.Message));
                }
            }
        }

        [HttpPost("load")]
        public async Task<IActionResult> Load([FromBody] SearchBody request)

        {
            using (new FunctionTracer())
            {
                try
                {
                    var response = _repository.LoadData(request);
                    if (response != null)
                    {
                        string contentType = request?.ContentType;
                        var viewpath = string.Empty;
                        /*                        object viewModel = null;
                        */
                        if (contentType == GCDC.Common.Helpers.Constants.ContentTypes.News)
                        {

                            string componentViewName = "_StoriesItem.cshtml";
                            viewpath = Common.Helpers.Constants.ComponentViewLocation + componentViewName;
                        }

                        if (viewpath != null && viewpath.Trim().Length > 0)
                        {
                            var renderedView = await RenderViewToStringAsync(viewpath, response);
                            return Ok(new { statusCode = HttpStatusCode.OK, HtmlContent = renderedView, HasNextPage = response.HasNextPage, TotalResults = response.TotalResults, TotalPages = response.TotalPages });

                        }
                        else
                        {
                            return BadRequest(ApiResponse.BadRequest("No View to Bind."));
                        }
                    }

                    return BadRequest(ApiResponse.BadRequest("No search data found."));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    return BadRequest(ApiResponse.ExpectationFailed(ex.Message));
                }
            }
        }


        private async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            try
            {
                var actionContext = new ActionContext(HttpContext, RouteData, ControllerContext.ActionDescriptor);
                var viewResult = _razorViewEngine.GetView(null, viewName, false);

                if (!viewResult.Success)
                {
                    throw new FileNotFoundException($"View '{viewName}' not found.");
                }

                using (var sw = new StringWriter())
                {
                    var viewContext = new ViewContext(
                    actionContext,
                        viewResult.View,
                        new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = model },
                        new TempDataDictionary(HttpContext, _tempDataProvider),
                        sw,
                        new HtmlHelperOptions()
                    );

                    await viewResult.View.RenderAsync(viewContext);

                    string result = sw.ToString();
                    // Remove all newline characters
                    result = result.Replace("\r\n", "").Replace("\n", "");

                    // Optionally, also remove excess whitespace between elements
                    result = Regex.Replace(result, @"\s+", " "); // Collapse any remaining whitespace to a single space
                    result = result.Trim(); // Trim leading and trailing whitespace

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpPost("submit-form")]
        public async Task<IActionResult> SubmitForm([FromBody] FormRequest request)
        {
            try
            {

                if (request == null || string.IsNullOrEmpty(request.FormData))
                {
                    return BadRequest("Invalid form submission data.");
                }

                FormData formData = new FormData();

                // Check for null or invalid request
                if (request == null || request.FormId <= 0 || string.IsNullOrEmpty(request.FormData))
                {
                    return BadRequest("Invalid form submission data."); // 400 Bad Request
                }

                // Retrieve the parent node for form submissions
                var parentNode = GCDC.Common.Helpers.Common.GetParentNode(GCDC.Common.Helpers.Constants.RootAlias.FormSubmissionNode);

                if (parentNode == null)
                {
                    return StatusCode(500, "Failed to find the FormSubmissions parent node."); // 500 Internal Server Error
                }

                int parentNodeId = parentNode.Id;
                if (parentNodeId == 0)
                {
                    return StatusCode(500, "Failed to find the FormSubmissions parent node Id."); // 500 Internal Server Error
                }

                string submissionNodeName = string.Empty;

                bool isCaptcha = await CaptchaVerify(request.RecaptchaToken);

                if (!isCaptcha)
                {
                    return BadRequest(ApiResponse.Forbidden(Common.Helpers.Common.GetDictionaryValueOrDefault(Constants.DictionaryKeys.RecaptchaValidationFailed)));

                }
                if (request.FormId == 44)
                {
                    using JsonDocument doc = JsonDocument.Parse(request.FormData);
                    if (doc != null && doc.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        JsonElement root = doc.RootElement;

                        string firstName = root.TryGetProperty("firstName", out JsonElement firstNameElement) ? firstNameElement.GetString() : "";
                        string lastName = root.TryGetProperty("lastName", out JsonElement lastNameElement) ? lastNameElement.GetString() : "";
                        string emailAddress = root.TryGetProperty("email", out JsonElement emailAddressElement) ? emailAddressElement.GetString() : "";
                        string phoneNumber = root.TryGetProperty("phone", out JsonElement phoneElement) ? phoneElement.ToString() : "";
                        string message = root.TryGetProperty("message", out JsonElement messageElement) ? messageElement.GetString() : "";
                        string referralId = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.FormData)["referralId"];
                        string utmCampaign = root.TryGetProperty("utm_campaign", out JsonElement utmCampaignElement) ? utmCampaignElement.GetString() : "";
                        string utmContent = root.TryGetProperty("utm_content", out JsonElement utmContentElement) ? utmContentElement.GetString() : "";
                        string utmMedium = root.TryGetProperty("utm_medium", out JsonElement utmMediumElement) ? utmMediumElement.GetString() : "";
                        string utmSource = root.TryGetProperty("utm_source", out JsonElement utmSourceElement) ? utmSourceElement.GetString() : "";
                        string utmTerm = root.TryGetProperty("utm_term", out JsonElement utmTermElement) ? utmTermElement.GetString() : "";

                        GeneralInquiry gnrldata = new GeneralInquiry(firstName, lastName, emailAddress, phoneNumber, message, referralId, utmCampaign, utmContent, utmMedium, utmSource, utmTerm);
                        formData.fieldValues = new List<FieldData>();
                        formData.fieldValues = GCDC.Common.Helpers.Common.ConvertToList(gnrldata);

                        submissionNodeName = $"{firstName} {lastName} - {emailAddress}";
                    }
                }
                else if (request.FormId == 46)
                {
                    using JsonDocument doc = JsonDocument.Parse(request.FormData);
                    if (doc != null && doc.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        JsonElement root = doc.RootElement;
                        string firstName = root.TryGetProperty("firstName", out JsonElement firstNameElement) ? firstNameElement.GetString() : "";
                        string lastName = root.TryGetProperty("lastName", out JsonElement lastNameElement) ? lastNameElement.GetString() : "";
                        string emailAddress = root.TryGetProperty("email", out JsonElement emailAddressElement) ? emailAddressElement.GetString() : "";
                        string phoneNumber = root.TryGetProperty("phone", out JsonElement phoneElement) ? phoneElement.ToString() : "";
                        string message = root.TryGetProperty("message", out JsonElement messageElement) ? messageElement.GetString() : "";
                        string company = root.TryGetProperty("CompanyName", out JsonElement companyElement) ? companyElement.GetString() : "";
                        string jobTitle = root.TryGetProperty("JobTitle", out JsonElement jobTitleElement) ? jobTitleElement.GetString() : "";
                        string referralId = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.FormData)["referralId"];
                        string utmCampaign = root.TryGetProperty("utm_campaign", out JsonElement utmCampaignElement) ? utmCampaignElement.GetString() : "";
                        string utmContent = root.TryGetProperty("utm_content", out JsonElement utmContentElement) ? utmContentElement.GetString() : "";
                        string utmMedium = root.TryGetProperty("utm_medium", out JsonElement utmMediumElement) ? utmMediumElement.GetString() : "";
                        string utmSource = root.TryGetProperty("utm_source", out JsonElement utmSourceElement) ? utmSourceElement.GetString() : "";
                        string utmTerm = root.TryGetProperty("utm_term", out JsonElement utmTermElement) ? utmTermElement.GetString() : "";

                        MediaAndPress gnrldata = new MediaAndPress(firstName, lastName, emailAddress, phoneNumber, message, company, jobTitle, referralId,
                            utmCampaign, utmContent, utmMedium, utmSource, utmTerm);
                        formData.fieldValues = new List<FieldData>();
                        formData.fieldValues = GCDC.Common.Helpers.Common.ConvertToList(gnrldata);

                        submissionNodeName = $"{firstName} {lastName} - {emailAddress}";
                    }
                }
                else if (request.FormId == 45)
                {
                    using JsonDocument doc = JsonDocument.Parse(request.FormData);
                    if (doc != null && doc.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        JsonElement root = doc.RootElement;
                        string firstName = root.TryGetProperty("firstName", out JsonElement firstNameElement) ? firstNameElement.GetString() : "";
                        string lastName = root.TryGetProperty("lastName", out JsonElement lastNameElement) ? lastNameElement.GetString() : "";
                        string emailAddress = root.TryGetProperty("email", out JsonElement emailAddressElement) ? emailAddressElement.GetString() : "";
                        string phoneNumber = root.TryGetProperty("phone", out JsonElement phoneElement) ? phoneElement.ToString() : "";
                        string linkedinUrl = root.TryGetProperty("CompanyName", out JsonElement linkedinUrlElement) ? linkedinUrlElement.GetString() : "";
                        string coverLetter = root.TryGetProperty("message", out JsonElement messageElement) ? messageElement.GetString() : "";
                        string referralId = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.FormData)["referralId"];
                        string utmCampaign = root.TryGetProperty("utm_campaign", out JsonElement utmCampaignElement) ? utmCampaignElement.GetString() : "";
                        string utmContent = root.TryGetProperty("utm_content", out JsonElement utmContentElement) ? utmContentElement.GetString() : "";
                        string utmMedium = root.TryGetProperty("utm_medium", out JsonElement utmMediumElement) ? utmMediumElement.GetString() : "";
                        string utmSource = root.TryGetProperty("utm_source", out JsonElement utmSourceElement) ? utmSourceElement.GetString() : "";
                        string utmTerm = root.TryGetProperty("utm_term", out JsonElement utmTermElement) ? utmTermElement.GetString() : "";

                        CareerCRM gnrldata = new CareerCRM(firstName, lastName, emailAddress, phoneNumber, linkedinUrl, coverLetter, referralId,
                            utmCampaign, utmContent, utmMedium, utmSource, utmTerm);
                        formData.fieldValues = new List<FieldData>();
                        formData.fieldValues = GCDC.Common.Helpers.Common.ConvertToList(gnrldata);

                        submissionNodeName = $"{firstName} {lastName} - {emailAddress}";
                    }
                }
                else if (request.FormId == 47)
                {
                    using JsonDocument doc = JsonDocument.Parse(request.FormData);
                    if (doc != null && doc.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        JsonElement root = doc.RootElement;
                        string firstName = root.TryGetProperty("firstName", out JsonElement firstNameElement) ? firstNameElement.GetString() : "";
                        string lastName = root.TryGetProperty("lastName", out JsonElement lastNameElement) ? lastNameElement.GetString() : "";
                        string emailAddress = root.TryGetProperty("email", out JsonElement emailAddressElement) ? emailAddressElement.GetString() : "";
                        string phoneNumber = root.TryGetProperty("phone", out JsonElement phoneElement) ? phoneElement.ToString() : "";
                        string company = root.TryGetProperty("CompanyName", out JsonElement companyElement) ? companyElement.GetString() : "";
                        string message = root.TryGetProperty("message", out JsonElement messageElement) ? messageElement.GetString() : "";
                        string jobTitle = root.TryGetProperty("JobTitle", out JsonElement jobTitleElement) ? jobTitleElement.GetString() : "";
                        string referralId = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.FormData)["referralId"];
                        string utmCampaign = root.TryGetProperty("utm_campaign", out JsonElement utmCampaignElement) ? utmCampaignElement.GetString() : "";
                        string utmContent = root.TryGetProperty("utm_content", out JsonElement utmContentElement) ? utmContentElement.GetString() : "";
                        string utmMedium = root.TryGetProperty("utm_medium", out JsonElement utmMediumElement) ? utmMediumElement.GetString() : "";
                        string utmSource = root.TryGetProperty("utm_source", out JsonElement utmSourceElement) ? utmSourceElement.GetString() : "";
                        string utmTerm = root.TryGetProperty("utm_term", out JsonElement utmTermElement) ? utmTermElement.GetString() : "";

                        InvestorCRM gnrldata = new InvestorCRM(firstName, lastName, emailAddress, phoneNumber, company, message,jobTitle, referralId,
                            utmCampaign, utmContent, utmMedium, utmSource, utmTerm);
                        formData.fieldValues = new List<FieldData>();
                        formData.fieldValues = GCDC.Common.Helpers.Common.ConvertToList(gnrldata);

                        submissionNodeName = $"{firstName} {lastName} - {emailAddress}";
                    }
                }
                else if (request.FormId == 48)
                {
                    using JsonDocument doc = JsonDocument.Parse(request.FormData);
                    if (doc != null && doc.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        JsonElement root = doc.RootElement;
                        string firstName = root.TryGetProperty("firstName", out JsonElement firstNameElement) ? firstNameElement.GetString() : "";
                        string lastName = root.TryGetProperty("lastName", out JsonElement lastNameElement) ? lastNameElement.GetString() : "";
                        string emailAddress = root.TryGetProperty("email", out JsonElement emailAddressElement) ? emailAddressElement.GetString() : "";
                        string phoneNumber = root.TryGetProperty("phone", out JsonElement phoneElement) ? phoneElement.ToString() : "";
                        string message = root.TryGetProperty("message", out JsonElement messageElement) ? messageElement.GetString() : "";
                        string company = root.TryGetProperty("CompanyName", out JsonElement companyElement) ? companyElement.GetString() : "";
                        string jobTitle = root.TryGetProperty("JobTitle", out JsonElement jobTitleElement) ? jobTitleElement.GetString() : "";
                        string referralId = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.FormData)["referralId"];
                        string utmCampaign = root.TryGetProperty("utm_campaign", out JsonElement utmCampaignElement) ? utmCampaignElement.GetString() : "";
                        string utmContent = root.TryGetProperty("utm_content", out JsonElement utmContentElement) ? utmContentElement.GetString() : "";
                        string utmMedium = root.TryGetProperty("utm_medium", out JsonElement utmMediumElement) ? utmMediumElement.GetString() : "";
                        string utmSource = root.TryGetProperty("utm_source", out JsonElement utmSourceElement) ? utmSourceElement.GetString() : "";
                        string utmTerm = root.TryGetProperty("utm_term", out JsonElement utmTermElement) ? utmTermElement.GetString() : "";

                        VendorCRM gnrldata = new VendorCRM(firstName, lastName, emailAddress, phoneNumber, message, company, jobTitle, referralId,
                            utmCampaign, utmContent, utmMedium, utmSource, utmTerm);
                        formData.fieldValues = new List<FieldData>();
                        formData.fieldValues = GCDC.Common.Helpers.Common.ConvertToList(gnrldata);

                        submissionNodeName = $"{firstName} {lastName} - {emailAddress}";
                    }
                }
                if (formData == null)
                    return BadRequest("Invalid payload format.");

                bool saveCRMResponse = Common.Helpers.Common.Root.SaveCrmresponse;
                // Submit form and save in Umbraco
                bool success = await _formsRepository.SubmitFormAndSave(request.FormId.ToString(), request.FormUrl, formData, submissionNodeName, saveCRMResponse);

                if (success)
                    return Ok("Form submitted successfully.");
                else
                    return StatusCode(500, "Failed to submit form.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error submitting form: {ex.Message}");
                return StatusCode(500, "An error occurred while submitting the form."); // 500 Internal Server Error
            }
        }

        private async Task<bool> CaptchaVerify(string token)
        {
            try
            {
                var dictionary = new Dictionary<string, string>
                    {
                        { "secret", Common.Helpers.Common.Root.GoogleReCaptchaSecretKey },
                        { "response", token }
                    };

                var postContent = new FormUrlEncodedContent(dictionary);

                HttpResponseMessage recaptchaResponse = null;
                string stringContent = "";

                // Call recaptcha api and validate the token
                using (var http = new HttpClient())
                {
                    recaptchaResponse = await http.PostAsync("https://www.google.com/recaptcha/api/siteverify", postContent);

                }
                stringContent = await recaptchaResponse.Content.ReadAsStringAsync();
                dynamic JSONdata = JObject.Parse(stringContent);

                if (JSONdata.success != "true" || JSONdata.score <= 0.5m)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}





