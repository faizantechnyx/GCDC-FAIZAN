using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using GCDC.Core.Repositories.Common;
using GCDC.Common.Helpers;
using GCDC.Core.Repositories.Common;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GCDC.Web.Controllers.Pages
{
    public class VisionAndMissionController : BaseController
    {
        public VisionAndMissionController(ILogger<VisionAndMissionController> logger, IBaseControllerFactory factory) : base(logger, factory)
        {
        }
        public override IActionResult Index()
        {
            using (new FunctionTracer(true, CurrentPage?.Name ?? string.Empty))
            {
                string ViewPath = "VisionAndMission.cshtml";
                try
                {
                    
                    return View(Constants.PageViewLocation + ViewPath, CurrentPage as VisionAndMission);
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
