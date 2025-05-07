using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GCDC.Web.ViewComponents.Components
{
    public class galleryApi : ViewComponent
    {
        private readonly IAntiforgery _antiforgery;

        public galleryApi(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }
        public async Task<IViewComponentResult> InvokeAsync(IPublishedContent component)
        {
            using (new FunctionTracer())
            {
                string ComponentViewName = "_GalleryApi.cshtml";
                try
                {
                    // Generate the anti-forgery token and pass it to the view
                    var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
                    ViewData["RequestVerificationToken"] = tokens.RequestToken;
                    return View(Constants.ComponentViewLocation + ComponentViewName, component as GalleryApi);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
