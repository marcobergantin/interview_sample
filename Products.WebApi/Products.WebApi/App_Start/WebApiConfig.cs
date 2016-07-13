using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using WebApiContrib.IoC.Ninject;
using System.Web.Http.Dependencies;
using Products.WebApi.IoC;
using Products.WebApi.Logging;

namespace Products.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Dependency Resolver setup
            IKernel kernel = new StandardKernel(new ResolverModule());
            IDependencyResolver ninjectResolver = new NinjectResolver(kernel);

            GlobalConfiguration.Configuration.DependencyResolver = ninjectResolver;
        }
    }
}
