using System.Collections.Generic;
using Products.WebApi.Contracts;
using Products.WebApi.Models;
using System;
using System.Threading.Tasks;
using Products.WebApi.Exceptions;
using NLog;

namespace Products.WebApi.Services
{
    public class ProductsService : IProductsService
    {
        #region Fields

        IProductsRepository _repository;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructor

        public ProductsService(IProductsRepository repository)
        {
            _repository = repository;
        }

        #endregion

        #region IProductsService Implements

        public async Task<IEnumerable<ViewProductDto>> GetProducts()
        {
            var products = new List<ViewProductDto>();

            _logger.Info("Getting all products");
            foreach (var p in await _repository.GetAll())
            {
                products.Add(MapProductToViewProductDto(p));
            }

            _logger.Info(string.Format("Retrieved {0} items", products.Count));

            return products;
        }

        public async Task<ViewProductDto> GetProduct(int id)
        {
            _logger.Info(string.Format("Getting product with id {0}", id.ToString()));

            var product = await _repository.GetById(id);
            if (product == null)
            {
                _logger.Warn("Not found");
                throw new ProductNotFoundException(String.Format("Product with id {0} not found.", id));
            }
            else
            {
                _logger.Info("Found");
                return MapProductToViewProductDto(product);
            }          
        }

        public async Task InsertProduct(AddEditProductDto productToInsert)
        {
            ValidateProductModel(productToInsert);

            //id will be taken care of by repository
            Product product = new Product()
            {
                Name = productToInsert.Name,
                Price = productToInsert.Price,
                LastUpdated = DateTime.Now
            };

            _logger.Info(string.Format("Inserting new product {0}", product.Name));
            await _repository.Add(product);
        }

        public async Task ModifyProduct(int id, AddEditProductDto product)
        {
            this.ValidateProductModel(product);
            _logger.Info(string.Format("Getting product with id {0} in order to modify it", id.ToString()));

            Product dbProduct = await _repository.GetById(id);
            if (dbProduct == null)
            {
                _logger.Warn("Not Found");
                throw new ProductNotFoundException(String.Format("Product with id {0} not found.", id));
            }

            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            dbProduct.LastUpdated = DateTime.Now;

            _logger.Info(string.Format("Update product {0}", id.ToString()));
            await _repository.Update(dbProduct);
        }

        public async Task DeleteProduct(int id)
        {
            _logger.Info(string.Format("Getting product with id {0} for deletion", id.ToString()));
            var dbProduct = await _repository.GetById(id);
            if (dbProduct == null)
            {
                _logger.Warn("Not Found");
                throw new ProductNotFoundException(String.Format("Product with id {0} not found.", id));
            }

            _logger.Info(string.Format("Delete product {0}", id.ToString()));
            await _repository.Delete(dbProduct);
        }

        public async Task InsertImageForProduct(int productId, byte[] image)
        {
            _logger.Info(string.Format("Getting product with id {0} for image update", productId.ToString()));
            Product dbProduct = await _repository.GetById(productId);
            if (dbProduct == null)
            {
                _logger.Warn("Not Found");
                throw new ProductNotFoundException(String.Format("Product with id {0} not found.", productId));
            }

            dbProduct.Image = image;
            dbProduct.LastUpdated = DateTime.Now;

            _logger.Info(string.Format("Update image for product {0}, image size is {1}KB", productId.ToString(), 
                ((float)(image.Length) / 1024)));
            await _repository.Update(dbProduct);
        }

        #endregion

        #region Utils

        // Marked as internal for test purposes, could use a validation framework (injected to the service)
        internal void ValidateProductModel(AddEditProductDto product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }

            if (String.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentNullException("Name can't be empty.");
            }

            if (product.Price < 0)
            {
                throw new ArgumentOutOfRangeException("Price can't be less than zero.");
            }
        }

        // Could use an object mapper (e.g. Automapper), marked as internal for test purposes
        internal ViewProductDto MapProductToViewProductDto(Product product)
        {
            var dto = new ViewProductDto()
            {
                Id = product.Id,
                Price = product.Price,
                Image = product.Image,
                Name = product.Name
            };

            return dto;
        }

        #endregion
    }
}