using Products.WebApi.Interfaces;
using Products.WebApi.Models;
using Products.WebApi.Services;
using System;
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
            _productsService = new EFProductsService();
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
            Product product = new Product()
            {
                Id = p.id,
                Name = p.name,
                Price = p.price,
                LastUpdated = DateTime.Now
            };

            _productsService.InsertProduct(product);
        }

        #endregion
    }
}