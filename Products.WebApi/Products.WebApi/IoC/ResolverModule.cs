using Ninject.Modules;
using Products.WebApi.Interfaces;
using Products.WebApi.Services;

namespace Products.WebApi.IoC
{
    public class ResolverModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IProductsService>().To<EFProductsService>();
        }
    }
}