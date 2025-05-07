using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.UmbracoContext;

namespace GCDC.Web.ViewComponents.Components
{
    public class header : ViewComponent
    {
        public header()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(IPublishedContent component)
        {
            using (new FunctionTracer())
            {
                string ComponentViewName = "_Header.cshtml";
                try
                {
                    IPublishedContent headerNode = component;
                    if (headerNode == null)
                    {
                        var componentsNode = Common.Helpers.Common.GetParentNode("components");
                        if (componentsNode != null)
                        {
                            var targetPageComponentsNode = componentsNode.Children.FirstOrDefault(x => x.Name == "Shared Component");
                            if (targetPageComponentsNode != null)
                            {
                                headerNode = targetPageComponentsNode.Children.FirstOrDefault(x => x.ContentType.Alias == "header");
                            }
                        }
                    }
                    return View(Constants.ComponentViewLocation + ComponentViewName, headerNode as Header);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
