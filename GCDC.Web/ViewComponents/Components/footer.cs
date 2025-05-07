using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace GCDC.Web.ViewComponents.Components
{
    public class footer : ViewComponent
    {
        public footer()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(IPublishedContent component)
        {
            using (new FunctionTracer())
            {
                string ComponentViewName = "_Footer.cshtml";
                try
                {
                    IPublishedContent footerNode = component;

                    if (footerNode == null)
                    {
                        var componentsNode = Common.Helpers.Common.GetParentNode("components");
                        if (componentsNode != null)
                        {
                            var targetPageComponentsNode = componentsNode.Children.FirstOrDefault(x => x.Name == "Shared Component");
                            if (targetPageComponentsNode != null)
                            {
                                footerNode = targetPageComponentsNode.Children.FirstOrDefault(x => x.ContentType.Alias == "footer");
                            }
                        }
                    }
                    return View(Constants.ComponentViewLocation + ComponentViewName, footerNode as Footer);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
