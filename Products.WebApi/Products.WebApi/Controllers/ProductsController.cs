using Products.WebApi.Interfaces;
using Products.WebApi.Models;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;

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
        public async Task<IHttpActionResult> Post([FromBody]ProductModel p)
        {
            Product inserted = await _productsService.InsertProduct(p);

            UrlHelper uriHelper = new UrlHelper(this.Request);
            uriHelper.Route("Products", inserted.Id);

            //return also link to product
            return Created(uriHelper.ToString(), inserted);
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
        #endregion
    }
}