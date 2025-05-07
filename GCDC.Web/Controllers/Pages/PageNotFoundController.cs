using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using GCDC.Core.Repositories.Common;
using Microsoft.AspNetCore.Mvc;

namespace GCDC.Web.Controllers.Pages
{
    public class PageNotFoundController : BaseController
    {
        public PageNotFoundController(ILogger<CareersController> logger, IBaseControllerFactory factory) : base(logger, factory)
        {
        }

        public override IActionResult Index()
        {
            using (new FunctionTracer(false, CurrentPage?.Name ?? string.Empty))
            {
                string ViewPath = "PageNotFound.cshtml";
                try
                {
                    ViewBag.IsNotFound = true;
                    return View(Constants.PageViewLocation + ViewPath, CurrentPage as PageNotFound);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}