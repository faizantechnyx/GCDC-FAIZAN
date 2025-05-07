using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using GCDC.Core.Repositories.Common;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GCDC.Web.Controllers.Pages
{
    public class HomeController : BaseController
    {
        public HomeController(ILogger<HomeController> logger, IBaseControllerFactory factory) : base(logger, factory)
        {
        }
        public override IActionResult Index()
        {
            using (new FunctionTracer(true, CurrentPage?.Name ?? string.Empty))
            {
                string ViewPath = "Home.cshtml";
                try
                {                    
                    return View(Constants.PageViewLocation + ViewPath, CurrentPage as Home);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, ex.Message);
                }
                return View(Constants.PageViewLocation + ViewPath);
            }
        }
    }
}
