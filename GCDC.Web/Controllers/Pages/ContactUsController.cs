using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using GCDC.Core.Repositories.Common;
using Microsoft.AspNetCore.Mvc;

namespace GCDC.Web.Controllers.Pages
{
    public class ContactUsController : BaseController
    {
        public ContactUsController(ILogger<ContactUsController> logger, IBaseControllerFactory factory) : base(logger, factory)
        {
        }

        public override IActionResult Index()
        {
            using (new FunctionTracer(false, CurrentPage?.Name ?? string.Empty))
            {
                string ViewPath = "ContactUs.cshtml";
                try
                {
                    ViewData["ContactUs"] = true;

                    return View(Constants.PageViewLocation + ViewPath, CurrentPage as ContactUs);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
