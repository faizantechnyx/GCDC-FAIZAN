using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using GCDC.Core.Repositories.Common;
using Microsoft.AspNetCore.Mvc;

namespace GCDC.Web.Controllers.Pages
{
    public class PrivacyPolicyController : BaseController
    {
        public PrivacyPolicyController(ILogger<PrivacyPolicyController> logger, IBaseControllerFactory factory) : base(logger, factory)
        {
        }

        public override IActionResult Index()
        {
            using (new FunctionTracer(false, CurrentPage?.Name ?? string.Empty))
            {
                string ViewPath = "PrivacyPolicy.cshtml";
                try
                {
                    
                    return View(Constants.PageViewLocation + ViewPath, CurrentPage as PrivacyPolicy);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}