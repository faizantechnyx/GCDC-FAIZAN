using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using GCDC.Core.Repositories.Common;
using Microsoft.AspNetCore.Mvc;

namespace GCDC.Web.Controllers.Pages
{
    public class InitiativesAndProgramsController : BaseController
    {
        public InitiativesAndProgramsController(ILogger<InitiativesAndProgramsController> logger, IBaseControllerFactory factory) : base(logger, factory)
        {
        }

        public override IActionResult Index()
        {
            using (new FunctionTracer(false, CurrentPage?.Name ?? string.Empty))
            {
                string ViewPath = "InitiativesAndPrograms.cshtml";
                try
                {
                    
                    return View(Constants.PageViewLocation + ViewPath, CurrentPage as InitiativesAndPrograms);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
