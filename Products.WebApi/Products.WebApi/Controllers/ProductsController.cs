using Products.WebApi.Interfaces;
using Products.WebApi.Models;
using System;
using System.Drawing;
using System.IO;
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
        public IHttpActionResult Get()
        {
            return Ok(_productsService.GetProducts());
        }

        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                return Ok(_productsService.GetProduct(id));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [Route("")]
        public IHttpActionResult Post([FromBody]ProductModel p)
        {
            _productsService.InsertProduct(p);
            return Ok();
        }

        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody]ProductModel p)
        {
            try
            {
                _productsService.ModifyProduct(id, p);
                return Ok();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                _productsService.DeleteProduct(id);
                return Ok();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [Route("Images/{id}")]
        public IHttpActionResult Put(int id, [FromBody]byte[] buffer)
        {
            try
            {
                Image image = Image.FromStream(new MemoryStream(buffer));
                if (image != null)
                {
                    //image parsed successfully
                    _productsService.InsertImageForProduct(id, buffer);
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        #endregion
    }
}