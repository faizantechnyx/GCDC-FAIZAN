using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using GCDC.Core.Repositories.Common;
using Microsoft.AspNetCore.Mvc;

namespace GCDC.Web.Controllers.Pages
{
    public class NewsMediaController : BaseController
    {
        public NewsMediaController(ILogger<NewsMediaController> logger, IBaseControllerFactory factory) : base(logger, factory)
        {
        }

        public override IActionResult Index()
        {
            using (new FunctionTracer(false, CurrentPage?.Name ?? string.Empty))
            {
                string ViewPath = "NewsMedia.cshtml";
                try
                {
                    ViewBag.NewsDetail = true;
                    return View(Constants.PageViewLocation + ViewPath, CurrentPage as NewsMedia);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
