using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Controllers;

namespace GCDC.Core.Repositories.Common
{
    public abstract class BaseController : RenderController
    {
        // Protected fields accessible in child classes
        protected ILogger Logger;
        protected IVariationContextAccessor VariationContextAccessor;
        protected ICompositeViewEngine CompositeViewEngine;
        protected IUmbracoContextAccessor UmbracoContextAccessor;
        protected IUmbracoHelperAccessor UmbracoHelperAccessor;
        protected IContentService ContentService;
        protected ILocalizationService LocalizationService;
        protected UmbracoHelper UmbracoHelper;
        protected IUmbracoContext UmbracoContext;
        private static IHttpContextAccessor _httpContextAccessor;

        public BaseController(ILogger logger, IBaseControllerFactory factory) : base((ILogger<RenderController>)logger, factory.GetCompositeViewEngine(), factory.GetUmbracoContextAccessor())
        {
            Logger = logger;

            // Use factory to initialize protected fields
            CompositeViewEngine = factory.GetCompositeViewEngine();
            UmbracoContextAccessor = factory.GetUmbracoContextAccessor();
            VariationContextAccessor = factory.GetVariationContextAccessor();
            UmbracoHelperAccessor = factory.GetUmbracoHelperAccessor();
            ContentService = factory.GetContentService();
            LocalizationService = factory.GetLocalizationService();
            _httpContextAccessor = factory.GetHttpContextAccessor();

            // UmbracoContextAccessor is null
            if (UmbracoContextAccessor == null)
            {
                throw new InvalidOperationException("UmbracoContextAccessor from factory");
            }
            // Initialize UmbracoHelper
            if (!UmbracoHelperAccessor.TryGetUmbracoHelper(out var umbracoHelper))
            {
                throw new InvalidOperationException("Unable to retrieve UmbracoHelper.");
            }
            UmbracoHelper = umbracoHelper;

            // Initialize VariationContext
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            Constants.Languages.Current = culture;
            VariationContextAccessor.VariationContext = new VariationContext(culture);

            // Initialize Global Site Root
            UmbracoContext = UmbracoContextAccessor.GetRequiredUmbracoContext();
            if (UmbracoContext == null)
            {
                throw new InvalidOperationException("Unable to retrieve UmbracoContext.");
            }
            var globalSite = UmbracoContext.Content?.GetAtRoot().FirstOrDefault(x => x.ContentType.Alias == Constants.RootAlias.Site);
            GCDC.Common.Helpers.Common.Root = globalSite as Site;

            // Set Global Variables
            GCDC.Common.Helpers.Common.SetGlocalVariable(UmbracoContext, UmbracoHelper, ContentService, LocalizationService, _httpContextAccessor);
        }

        protected IActionResult ReloadCurrentPage()
        {
            // Redirect to the current page
            return Redirect(Request.Path);
        }

        protected IActionResult RedirectToHomePage()
        {
            // Fetch the home page (assuming it's the first root node)
            var homePage = UmbracoHelper.ContentAtRoot().FirstOrDefault();

            if (homePage != null)
            {
                return Redirect(homePage.Url());
            }

            // Fallback: Reload current page if home page is not found
            return Redirect(Request.Path);
        }

        protected IActionResult ReloadCurrentPageWithParams(string queryString)
        {
            var currentPath = Request.Path;
            return Redirect($"{currentPath}{queryString}");
        }

        protected IActionResult Redirect(string url)
        {
            return Redirect(url);
        }

    }
}
