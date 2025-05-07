using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using GCDC.Core.Repositories.Common;
using Microsoft.AspNetCore.Mvc;

namespace GCDC.Web.Controllers.Pages
{
    public class WhatWeDoController : BaseController
    {
        public WhatWeDoController(ILogger<WhatWeDoController> logger, IBaseControllerFactory factory) : base(logger, factory)
        {
        }

        public override IActionResult Index()
        {
            using (new FunctionTracer(false, CurrentPage?.Name ?? string.Empty))
            {
                string ViewPath = "WhatWeDo.cshtml";
                try
                {
                    
                    return View(Constants.PageViewLocation + ViewPath, CurrentPage as WhatWeDo);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
