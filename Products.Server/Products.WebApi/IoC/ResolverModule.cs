using Ninject.Modules;
using Products.WebApi.Contracts;
using Products.WebApi.Logging;
using Products.WebApi.Services;
using Products.WebApi.Tracing;
using System.Web.Http.Tracing;

namespace Products.WebApi.IoC
{
    public class ResolverModule : NinjectModule
    {
        public override void Load()
        {         
            Bind<IProductsLogger>().To<LogManager>();
            Bind<ITraceWriter>().To<Logger>();
            Bind<IProductsRepository>().To<ProductsRepository>();
            Bind<IProductsService>().To<ProductsService>();
        }
    }
}