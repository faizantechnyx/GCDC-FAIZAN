using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using GCDC.Core.Repositories.Common;
using Microsoft.AspNetCore.Mvc;

namespace GCDC.Web.Controllers.Pages
{
    public class SustainabilityController : BaseController
    {
        public SustainabilityController(ILogger<SustainabilityController> logger, IBaseControllerFactory factory) : base(logger, factory)
        {
        }

        public override IActionResult Index()
        {
            using (new FunctionTracer(false, CurrentPage?.Name ?? string.Empty))
            {   
                string ViewPath = "Sustainability.cshtml";
                try
                {
                    ViewData["IsSustainabilityPage"] = true;
                    return View(Constants.PageViewLocation + ViewPath, CurrentPage as Sustainability);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
