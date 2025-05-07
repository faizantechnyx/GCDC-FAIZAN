using GCDC.Common.Helpers;
using GCDC.Common.Models.CMS;
using GCDC.Core.Repositories.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Serilog;
using System.Web;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common;

namespace GCDC.Web.Controllers.Pages
{
    public class DetailsController : Controller
    {
        private readonly ILogger<DetailsController> _logger;
        private readonly IVariationContextAccessor VariationContextAccessor;
        private readonly ICompositeViewEngine CompositeViewEngine;
        private readonly IUmbracoContextAccessor UmbracoContextAccessor;
        private readonly IUmbracoHelperAccessor UmbracoHelperAccessor;
        private readonly IContentService ContentService;
        private readonly ILocalizationService LocalizationService;
        private readonly UmbracoHelper UmbracoHelper;
        private readonly IUmbracoContext UmbracoContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DetailsController(ILogger<DetailsController> logger, IBaseControllerFactory factory)
        {
            _logger = logger;
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

        [Route("/" + Constants.ContentTypes.SearchText + "/" + "{newskey}")]
        [Route("/" + "{culture}" + "/" + Constants.ContentTypes.SearchText + "/" + "{newskey}")]
        public IActionResult GetNewsDetail(string newskey, string culture = Constants.Languages.Default)
        {
            using (new FunctionTracer())
            {
                string viewpath = "~/Views/Partials/Details/NewsDetail.cshtml";
                try
                {
                    Constants.Languages.Current = culture;
                    VariationContextAccessor.VariationContext = new VariationContext(culture);
                    var umbracoContext = UmbracoContextAccessor.GetRequiredUmbracoContext();
                    var dataSources = umbracoContext.Content?.GetAtRoot().FirstOrDefault(x => x.ContentType.Alias == Constants.RootAlias.DataSources);

                    IQueryable<IPublishedContent>? query = null;
                    var newsAndMediaNode = dataSources?.Children(VariationContextAccessor)?.FirstOrDefault(c => c.ContentType.Alias == Constants.ContentTypes.News);
                    var newsNode = newsAndMediaNode?.Children(VariationContextAccessor)?.Where(c => c.ContentType.Alias == Constants.Filters.NewsCategory).ToList();                  
                    var newsItem = (newsNode != null) ? newsNode
                                                        .SelectMany(n => n.Children(VariationContextAccessor))
                                                        .Select(a => a as NewsItem) // Cast each item to NewsItem
                                                        .FirstOrDefault(a => a != null && HttpUtility.HtmlDecode(newskey) == a.Slug)
                                                    : null;

                    ViewBag.NewsItemMeta = newsItem as IMetaTags;
                    return View(viewpath, newsItem as NewsItem);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, ex.Message);
                }
                return View(viewpath);
            }
        }





    }
}
