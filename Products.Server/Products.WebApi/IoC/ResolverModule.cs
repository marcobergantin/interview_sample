using Ninject.Modules;
using Products.WebApi.Interfaces;
using Products.WebApi.Logging;
using Products.WebApi.Services;
using System.Web.Http.Tracing;

namespace Products.WebApi.IoC
{
    public class ResolverModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITraceWriter>().To<NLogger>();
            Bind<IProductsRepository>().To<ProductsRepository>();
            Bind<IProductsService>().To<ProductsService>();
        }
    }
}