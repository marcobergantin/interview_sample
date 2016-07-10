using Products.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Products.WebApi.Controllers
{
    [RoutePrefix("Products")]
    public class ProductsController : ApiController
    {
        static List<Product> products = new List<Product>();

        [Route("")]
        public List<Product> Get()
        {       
            return products;
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

            products.Add(product);
        }
    }
}