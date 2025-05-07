using GCDC.Core.Repositories.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Web.BackOffice.Security;
using Umbraco.Cms.Core.Security;
using GCDC.Core.Authenticator2FA; // Make sure the namespace is correct for your UmbracoUserAppAuthenticator
using Umbraco.Cms.Core.Configuration.Models;
using GCDC.Core.Authenticator2FA;

namespace GCDC.Core.Composers
{
    public class GCDCComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            try
            {
                // Get the current assembly
                
                Assembly assembly = Assembly.GetExecutingAssembly();

                // Get all types in the assembly
                var classesInNamespace = assembly.GetTypes();

                foreach (var RepositoryClass in classesInNamespace)
                {
                    string _namespace = RepositoryClass.DeclaringType == null ? RepositoryClass.FullName : RepositoryClass.DeclaringType.FullName;
                    //exclude base/common classes and constructor
                    if (!RepositoryClass.IsAbstract && RepositoryClass.IsClass && _namespace.Contains(".Repositories.") && !_namespace.Contains("Common") && !RepositoryClass.Name.Contains("<>"))
                    {
                        if (_namespace.Contains("Api", StringComparison.OrdinalIgnoreCase))
                        {
                            builder.Services.AddScoped(RepositoryClass, serviceProvider =>
                            {
                                var loggerType = typeof(ILogger<>).MakeGenericType(RepositoryClass);
                                var logger = serviceProvider.GetRequiredService(loggerType);
                                var iBaseclassFactory = serviceProvider.GetRequiredService<IBaseControllerFactory>();  // Resolving IBaseControllerFactory
                                return Activator.CreateInstance(RepositoryClass, logger, iBaseclassFactory);
                            });
                        }
                        else
                        {
                            builder.Services.AddScoped(RepositoryClass);
                        }
                    }
                }
                // --- 2FA Authenticator Support ---

                var identityBuilder = new BackOfficeIdentityBuilder(builder.Services);

                identityBuilder.AddTwoFactorProvider<UmbracoUserAppAuthenticator>(UmbracoUserAppAuthenticator.Name);



                builder.Services.Configure<TwoFactorLoginViewOptions>(UmbracoUserAppAuthenticator.Name, options =>

                {

                    options.SetupViewPath = "..\\App_Plugins\\TwoFactorProviders\\twoFactorProviderGoogleAuthenticator.html";

                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

