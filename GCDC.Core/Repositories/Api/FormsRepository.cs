using GCDC.Common.Helpers;
using GCDC.Common.Models.Api.Request.CRM_Request;
using GCDC.Core.Repositories.Common;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Web.Common;

namespace GCDC.Core.Repositories.Api
{
    public class FormsRepository : AbstractRepository
    {
        private readonly ILogger<FormsRepository> _logger;
        private readonly IContentService _contentService;
        private readonly IScopeProvider _scopeProvider;
        private readonly IUmbracoHelperAccessor _umbracoHelperAccessor;

        public FormsRepository(ILogger<FormsRepository> logger, IBaseControllerFactory factory) : base(logger, factory)
        {
            _logger = logger;
            _scopeProvider = factory.GetScopeProvider();
            _contentService = factory.GetContentService();
            _umbracoHelperAccessor = factory.GetUmbracoHelperAccessor();
        }

        public async Task<bool> SubmitFormAndSave(string formId, string url, FormData formData, string submissionNodeName = "", bool saveCRMResponse = false)
        {
            try
            {
                string completeurl = $"{url}{formId}";

                var (response, isSuccess) = await GCDC.Common.Helpers.Common.CallCrmApiAsync(completeurl, formData);

                //if (isSuccess)
                {
                    int parentNodeId = GCDC.Common.Helpers.Common.GetFormSubmissionNodeId(formId);
                    bool saveResult = SaveFormSubmission(parentNodeId, formId, response, submissionNodeName, saveCRMResponse);
                    return saveResult;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool SaveFormSubmission(int parentNodeId, string formId, string response, string submissionNodeName = "", bool saveCRMResponse = false)
        {
            try
            {
                using (var scope = _scopeProvider.CreateScope(autoComplete: true))
                {
                    //submissionNodeName = string.IsNullOrEmpty(submissionNodeName) ? "Form Submission " + DateTime.UtcNow.ToString("yyyyMMddHHmmss") : submissionNodeName;
					submissionNodeName = "Form Submission - " + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    var formSubmission = _contentService.Create(submissionNodeName, parentNodeId, "formInfo");

                    if (formSubmission == null)
                    {
                        _logger.LogError("Failed to create form submission content.");
                        return false;
                    }
                    formSubmission.SetValue("formId", formId);
                    if (saveCRMResponse)
                        formSubmission.SetValue("response", response);
                    else
                        formSubmission.SetValue("response", string.IsNullOrEmpty(response) ? "Form submission failed" : "Form has been Successfully Send!");
                    formSubmission.SetValue("submissionDate", DateTime.UtcNow);

                    // Save and publish the content
                    var result = _contentService.SaveAndPublish(formSubmission);
                    return result.Success;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while saving form submission.");
                return false;
            }
        }
    }
}