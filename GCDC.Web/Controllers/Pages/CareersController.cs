using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using GCDC.Core.Repositories.Common;
using Microsoft.AspNetCore.Mvc;

namespace GCDC.Web.Controllers.Pages
{
    public class CareersController : BaseController
    {
        public CareersController(ILogger<CareersController> logger, IBaseControllerFactory factory) : base(logger, factory)
        {
        }

        public override IActionResult Index()
        {
            using (new FunctionTracer(false, CurrentPage?.Name ?? string.Empty))
            {
                string ViewPath = "Careers.cshtml";
                try
                {
                    ViewData["ContactUs"] = true;

                    return View(Constants.PageViewLocation + ViewPath, CurrentPage as Careers);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}