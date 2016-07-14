using System.Collections.Generic;
using Products.WebApi.Interfaces;
using Products.WebApi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Products.WebApi.Exceptions;

namespace Products.WebApi.Services
{
    public class ProductsService : IProductsService
    {
        #region Fields

        IProductsRepository _repository;

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

            foreach (var p in await _repository.GetAll())
            {
                products.Add(MapProductToViewProductDto(p));
            }

            return products;
        }

        public async Task<ViewProductDto> GetProduct(int id)
        {
            var product = await _repository.GetById(id);

            return MapProductToViewProductDto(product);
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

            await _repository.Add(product);
        }

        public async Task ModifyProduct(int id, AddEditProductDto product)
        {
            this.ValidateProductModel(product);
            Product dbProduct = await _repository.GetById(id);

            if (dbProduct == null)
            {
                throw new ProductNotFoundException(String.Format("Product with id {0} not found.", id));
            }

            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            dbProduct.LastUpdated = DateTime.Now;

            await _repository.Update(dbProduct);
        }

        public async Task DeleteProduct(int id)
        {
            var dbProduct = await _repository.GetById(id);

            if (dbProduct == null)
            {
                throw new ProductNotFoundException(String.Format("Product with id {0} not found.", id));
            }

            await _repository.Delete(dbProduct);
        }

        public async Task InsertImageForProduct(int productId, byte[] image)
        {
            Product dbProduct = await _repository.GetById(productId);

            if (dbProduct == null)
            {
                throw new ProductNotFoundException(String.Format("Product with id {0} not found.", productId));
            }

            dbProduct.Image = image;
            dbProduct.LastUpdated = DateTime.Now;

            await _repository.Update(dbProduct);
        }

        #endregion

        #region Utils

        // This method is marked as internal for testing purpose, a validation framework should be used and injected to the service.
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

        // A library like Automapper should be used for mapping, or also a "home-made" framework is fine, but an interface should be
        // passed to the service so we can unit test it better. This method is marked as internal so we can test it.
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