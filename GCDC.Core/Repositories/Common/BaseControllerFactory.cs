using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Cms.Web.Common;

namespace GCDC.Core.Repositories.Common
{
    public interface IBaseControllerFactory
    {
        ICompositeViewEngine GetCompositeViewEngine();
        IUmbracoContextAccessor GetUmbracoContextAccessor();
        IVariationContextAccessor GetVariationContextAccessor();
        IUmbracoHelperAccessor GetUmbracoHelperAccessor();
        IContentService GetContentService();
        IMediaService GetMediaService();
        IHttpContextAccessor GetHttpContextAccessor();
        ILocalizationService GetLocalizationService();
        IPublishedContentQuery GetPublishedContentQuery();
        IScopeProvider GetScopeProvider();
    }

    public class BaseControllerFactory : IBaseControllerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public BaseControllerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public ICompositeViewEngine GetCompositeViewEngine() => _serviceProvider.GetRequiredService<ICompositeViewEngine>();
        public IUmbracoContextAccessor GetUmbracoContextAccessor() => _serviceProvider.GetRequiredService<IUmbracoContextAccessor>();
        public IVariationContextAccessor GetVariationContextAccessor() => _serviceProvider.GetRequiredService<IVariationContextAccessor>();
        public IUmbracoHelperAccessor GetUmbracoHelperAccessor() => _serviceProvider.GetRequiredService<IUmbracoHelperAccessor>();
        public IContentService GetContentService() => _serviceProvider.GetRequiredService<IContentService>();
        public IMediaService GetMediaService() => _serviceProvider.GetRequiredService<IMediaService>();
        public IHttpContextAccessor GetHttpContextAccessor() => _serviceProvider.GetRequiredService<IHttpContextAccessor>();
        public ILocalizationService GetLocalizationService() => _serviceProvider.GetRequiredService<ILocalizationService>();
        public IPublishedContentQuery GetPublishedContentQuery() => _serviceProvider.GetRequiredService<IPublishedContentQuery>();
        public IScopeProvider GetScopeProvider() => _serviceProvider.GetRequiredService<IScopeProvider>();

    }
}
