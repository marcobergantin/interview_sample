using Products.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Products.WebApi.Controllers
{
    [RoutePrefix("Products")]
    public class ProductsController : ApiController
    {
        [Route("")]
        public List<Product> Get()
        {
            List<Product> products = new List<Product>();
            using (var ctx = new ProductsContext())
            {
                foreach (var p in ctx.ProductSet)
                {
                    products.Add(p);
                }
            }

            return products;
        }

        [Route("")]
        public async void Post([FromBody]ProductModel p)
        {
            using (var ctx = new ProductsContext())
            {
                Product product = new Product()
                {
                    Id = p.id,
                    Name = p.name,
                    Price = p.price,
                    LastUpdated = DateTime.Now
                };

                ctx.ProductSet.Add(product);
                int result = await ctx.SaveChangesAsync();
            }
        }
    }
}