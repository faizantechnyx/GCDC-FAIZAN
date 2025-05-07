using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GCDC.Web.ViewComponents.Components
{
    public class infoCardSection : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IPublishedContent component)
        {
            using (new FunctionTracer())
            {
                string ComponentViewName = "_InfoCardSection.cshtml";
                try
                {
                    return View(Constants.ComponentViewLocation + ComponentViewName, component as InfoCardSection);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
