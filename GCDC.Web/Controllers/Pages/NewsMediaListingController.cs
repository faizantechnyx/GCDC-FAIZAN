using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using GCDC.Core.Repositories.Common;
using Microsoft.AspNetCore.Mvc;

namespace GCDC.Web.Controllers.Pages
{
    public class NewsMediaListingController : BaseController
    {
        public NewsMediaListingController(ILogger<NewsMediaListingController> logger, IBaseControllerFactory factory) : base(logger, factory)
        {
        }

        public override IActionResult Index()
        {
            using (new FunctionTracer(false, CurrentPage?.Name ?? string.Empty))
            {
                string ViewPath = "NewsMediaListing.cshtml";
                try
                {
                    ViewData["IsNewsMediaListing"] = true;
                    return View(Constants.PageViewLocation + ViewPath, CurrentPage as NewsMediaListing);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
