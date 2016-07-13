using Ninject;
using Products.WebApi.IoC;
using System.Web.Http;
using System.Web.Http.Dependencies;
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

        }
    }
}
