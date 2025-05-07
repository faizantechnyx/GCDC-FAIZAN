using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using GCDC.Core.Repositories.Common;
using GCDC.Common.Helpers;
using GCDC.Core.Repositories.Common;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GCDC.Web.Controllers.Pages
{
    public class OurValuesController : BaseController
    {
        public OurValuesController(ILogger<OurValuesController> logger, IBaseControllerFactory factory) : base(logger, factory)
        {
        }
        public override IActionResult Index()
        {
            using (new FunctionTracer(true, CurrentPage?.Name ?? string.Empty))
            {
                string ViewPath = "OurValues.cshtml";
                try
                {
                    
                    return View(Constants.PageViewLocation + ViewPath, CurrentPage as OurValues);
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
