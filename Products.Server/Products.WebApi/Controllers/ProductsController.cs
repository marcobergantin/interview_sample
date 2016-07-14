using Products.WebApi.Contracts;
using Products.WebApi.Models;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Tracing;
using Products.WebApi.Exceptions;

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
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                this.TraceRequest();
                return Ok(await _productsService.GetProducts());
            }
            catch (Exception ex)
            {
                _tracer.Error(Request, _loggerCategory, ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Returns Product with specified Id if present, Not Found otherwise
        /// </summary>
        /// <param name="id">Id of the product</param>
        /// <returns>Ok or NotFound if success or fail</returns>
        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                this.TraceRequest();
                var product = await _productsService.GetProduct(id);
                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                _tracer.Error(Request, _loggerCategory, ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Allows to inser a product: the ID and last modified properties will be taken care of automatically.
        /// NB: to insert image POST to Products/Images/{id}
        /// </summary>
        /// <param name="product">The Product's data</param>
        /// <returns>Ok if succeeded</returns>
        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]AddEditProductDto product)
        {
            try
            {
                this.TraceRequest();
                await _productsService.InsertProduct(product);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                _tracer.Error(Request, _loggerCategory, ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _tracer.Error(Request, _loggerCategory, ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Modify existing product with specified ID
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <param name="product">Product data</param>
        /// <returns>Ok or NotFound in case of success or fail</returns>
        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, [FromBody]AddEditProductDto product)
        {
            try
            {
                this.TraceRequest();
                await _productsService.ModifyProduct(id, product);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                _tracer.Error(Request, _loggerCategory, ex);
                return NotFound();
            }
            catch (Exception ex)
            {
                _tracer.Error(Request, _loggerCategory, ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Deletes product with specified ID
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns> Ok if deleted or NotFound in case of invalid ID (fail)</returns>
        [Route("{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                this.TraceRequest();
                await _productsService.DeleteProduct(id);
                return Ok();
            }
            catch (ProductNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _tracer.Error(Request, _loggerCategory, ex);
                return InternalServerError(ex);
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
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, [FromBody]byte[] buffer)
        {
            try
            {
                this.TraceRequest();
                using (var memoryStream = new MemoryStream(buffer))
                {
                    Image image = Image.FromStream(memoryStream);
                    if (image != null)
                    {
                        //image parsed successfully
                        await _productsService.InsertImageForProduct(id, buffer);
                        return Ok();
                    }
                    else
                    {
                        _tracer.Error(Request, _loggerCategory, "Couldn't parse img", new string[] { });
                        return BadRequest();
                    }

                }

            }
            catch (ArgumentException ex)
            {
                _tracer.Error(Request, _loggerCategory, ex);
                return BadRequest(ex.Message);
            }
            catch (ProductNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _tracer.Error(Request, _loggerCategory, ex);
                return InternalServerError(ex);
            }
        }

        private void TraceRequest()
        {
            _tracer.Info(Request, _loggerCategory, string.Empty, new string[] { });
        }

        #endregion
    }
}