using Products.WebApi.Interfaces;
using Products.WebApi.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace Products.WebApi.Controllers
{
    [RoutePrefix("Products")]
    public class ProductsController : ApiController
    {
        #region Fields

        IProductsService _productsService;

        #endregion

        #region Constructor

        public ProductsController()
        {
            _productsService = (IProductsService)GlobalConfiguration.Configuration
                                                                    .DependencyResolver
                                                                    .GetService(typeof(IProductsService));
        }

        #endregion

        #region Methods

        [Route("")]
        public IEnumerable<Product> Get()
        {
            return _productsService.GetProducts();
        }

        [Route("")]
        public void Post([FromBody]ProductModel p)
        {
            _productsService.InsertProduct(p);
        }

        [Route("{id}")]
        public void Put(int id, [FromBody]ProductModel p)
        {
            _productsService.ModifyProduct(id, p);
        }

        [Route("{id}")]
        public void Delete(int id)
        {
            _productsService.DeleteProduct(id);
        }
        #endregion
    }
}