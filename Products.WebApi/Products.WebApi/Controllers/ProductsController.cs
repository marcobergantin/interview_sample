using Products.WebApi.Interfaces;
using Products.WebApi.Models;
using System;
using System.Drawing;
using System.IO;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace Products.WebApi.Controllers
{
    [RoutePrefix("Products")]
    public class ProductsController : ApiController
    {
        #region Fields

        IProductsService _productsService;
        ITraceWriter _tracer;

        readonly string _loggerCategory = nameof(ProductsController);

        #endregion

        #region Constructor

        public ProductsController()
        {
            _productsService = (IProductsService)GlobalConfiguration.Configuration
                                                                    .DependencyResolver
                                                                    .GetService(typeof(IProductsService));
            _tracer = GlobalConfiguration.Configuration.Services.GetTraceWriter();
        }

        #endregion

        #region Methods

        [Route("")]
        public IHttpActionResult Get()
        {
            this.TraceRequest();
            return Ok(_productsService.GetProducts());
        }

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

        [Route("")]
        public IHttpActionResult Post([FromBody]ProductModel p)
        {
            this.TraceRequest();
            _productsService.InsertProduct(p);
            return Ok();
        }

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