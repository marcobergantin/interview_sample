using Ninject;
using Products.WebApi.IoC;
using Products.WebApi.Logging;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.Tracing;
using WebApiContrib.IoC.Ninject;

namespace Products.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

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

            //use custom logger for web api tracing
            GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new Logger());
        }
    }
}
