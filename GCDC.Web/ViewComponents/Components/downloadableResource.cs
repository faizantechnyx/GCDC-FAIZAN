using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GCDC.Web.ViewComponents.Components
{
    public class downloadableResource : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IPublishedContent component)
        {
            using (new FunctionTracer())
            {
                string ComponentViewName = "_DownloadableResource.cshtml";
                try
                {
                    return View(Constants.ComponentViewLocation + ComponentViewName, component as DownloadableResource);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
