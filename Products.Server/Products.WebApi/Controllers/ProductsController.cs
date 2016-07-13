using Products.WebApi.Interfaces;
using Products.WebApi.Models;
using System;
using System.Drawing;
using System.IO;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace Products.WebApi.Controllers
{
    /// <summary>
    /// Entry Point for Products API
    /// </summary>
    [RoutePrefix("Products")]
    public class ProductsController : ApiController
    {
        #region Fields

        IProductsService _productsService;
        ITraceWriter _tracer;

        readonly string _loggerCategory = nameof(ProductsController);

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a ProductsController
        /// </summary>
        public ProductsController(IProductsService productsService, ITraceWriter tracer)
        {
            _productsService = productsService;
            _tracer = tracer;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns all products
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult Get()
        {
            this.TraceRequest();
            return Ok(_productsService.GetProducts());
        }

        /// <summary>
        /// Returns Product with specified Id if present, Not Found otherwise
        /// </summary>
        /// <param name="id">Id of the product</param>
        /// <returns>Ok or NotFound if success or fail</returns>
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                this.TraceRequest();
                return Ok(_productsService.GetProduct(id));
            }
            catch (ArgumentException ex)
            {
                _tracer.Error(Request, _loggerCategory, ex);
                return NotFound();
            }
        }

        /// <summary>
        /// Allows to inser a product: the ID and last modified properties will be taken care of automatically.
        /// NB: to insert image POST to Products/Images/{id}
        /// </summary>
        /// <param name="p">The Product's data</param>
        /// <returns>Ok if succeeded</returns>
        [Route("")]
        public IHttpActionResult Post([FromBody]ProductModel p)
        {
            this.TraceRequest();
            _productsService.InsertProduct(p);
            return Ok();
        }

        /// <summary>
        /// Modify existing product with specified ID
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <param name="p">Product data</param>
        /// <returns>Ok or NotFound in case of success or fail</returns>
        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody]ProductModel p)
        {
            try
            {
                this.TraceRequest();
                _productsService.ModifyProduct(id, p);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                _tracer.Error(Request, _loggerCategory, ex);
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes product with specified ID
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns> Ok if deleted or NotFound in case of invalid ID (fail)</returns>
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                this.TraceRequest();
                _productsService.DeleteProduct(id);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                _tracer.Error(Request, _loggerCategory, ex);
                return NotFound();
            }
        }

        /// <summary>
        /// Allows to insert and/or modify the image for a product
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <param name="buffer">The raa bytes of the image</param>
        /// <returns>Ok if succeeded, BadRequest if buffer fails to be converted to an image, not found if ID is 
        /// non valid</returns>
        [Route("Images/{id}")]
        public IHttpActionResult Put(int id, [FromBody]byte[] buffer)
        {
            try
            {
                this.TraceRequest();
                Image image = Image.FromStream(new MemoryStream(buffer));
                if (image != null)
                {
                    //image parsed successfully
                    _productsService.InsertImageForProduct(id, buffer);
                    return Ok();
                }
                else
                {
                    _tracer.Error(Request, _loggerCategory, "Couldn't parse img", new string[] { });
                    return BadRequest();
                }
            }
            catch (ArgumentException ex)
            {
                _tracer.Error(Request, _loggerCategory, ex);
                return NotFound();
            }
        }

        private void TraceRequest()
        {
            _tracer.Info(Request, _loggerCategory, string.Empty, new string[] { });
        }

        #endregion
    }
}