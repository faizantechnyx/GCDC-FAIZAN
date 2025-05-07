using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common;

namespace GCDC.Core.Repositories.Common
{
    public abstract class AbstractRepository
    {
        protected ILogger Logger;
        protected IVariationContextAccessor VariationContextAccessor;
        protected ICompositeViewEngine CompositeViewEngine;
        protected IUmbracoContextAccessor UmbracoContextAccessor;
        protected IUmbracoHelperAccessor UmbracoHelperAccessor;
        protected IContentService ContentService;
        protected ILocalizationService LocalizationService;
        protected UmbracoHelper UmbracoHelper;
        protected IMediaService MediaService;
        protected IHttpContextAccessor HttpContextAccessor;
        protected IUmbracoContext UmbracoContext;
        protected AbstractRepository(ILogger logger, IBaseControllerFactory factory)
        {
            Logger = logger;

            // Use factory to initialize protected fields
            CompositeViewEngine = factory.GetCompositeViewEngine();
            UmbracoContextAccessor = factory.GetUmbracoContextAccessor();
            VariationContextAccessor = factory.GetVariationContextAccessor();
            UmbracoHelperAccessor = factory.GetUmbracoHelperAccessor();
            ContentService = factory.GetContentService();
            MediaService = factory.GetMediaService();
            HttpContextAccessor = factory.GetHttpContextAccessor();
            LocalizationService = factory.GetLocalizationService();

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
            var globalSite = UmbracoContext.Content?.GetAtRoot().FirstOrDefault(x => x.ContentType.Alias == Constants.RootAlias.Site);

            GCDC.Common.Helpers.Common.Root = globalSite as Site;

            // Set Global Variables
            GCDC.Common.Helpers.Common.SetGlocalVariable(UmbracoContext, UmbracoHelper, ContentService, LocalizationService, HttpContextAccessor);
        }
    }
}
