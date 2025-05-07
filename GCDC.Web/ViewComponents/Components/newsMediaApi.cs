using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GCDC.Web.ViewComponents.Components
{
    public class newsMediaApi : ViewComponent
    {
        private readonly IAntiforgery _antiforgery;

        public newsMediaApi(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }
        public async Task<IViewComponentResult> InvokeAsync(IPublishedContent component)
        {
            using (new FunctionTracer())
            {
                string ComponentViewName = "_NewsMediaApi.cshtml";
                try
                {
                    // Generate the anti-forgery token and pass it to the view
                    var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
                    ViewData["RequestVerificationToken"] = tokens.RequestToken;
                    return View(Constants.ComponentViewLocation + ComponentViewName, component as NewsMediaApi);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
