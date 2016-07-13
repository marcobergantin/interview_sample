using System.Collections.Generic;
using Products.WebApi.Interfaces;
using Products.WebApi.Models;
using System;

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

        public IEnumerable<Product> GetProducts()
        {
            return _repository.GetAllProducts();
        }

        public Product GetProduct(int id)
        {
            Product dbEntry = _repository.GetProduct(id);
            if (dbEntry != null)
            {
                return dbEntry;
            }
            else
            {
                throw new ArgumentException(this.ProductNotFoundMessage(id));
            }
        }

        public void InsertProduct(ProductModel p)
        {
            //id will be taken care of by repository
            Product product = new Product()
            {
                Name = p.Name,
                Price = p.Price,
                LastUpdated = DateTime.Now
            };

             _repository.InsertProduct(product);
        }

        public void ModifyProduct(int id, ProductModel product)
        {
            _repository.ModifyProduct(id, product);
        }

        public void DeleteProduct(int id)
        {
            _repository.DeleteProduct(id);
        }

        public void InsertImageForProduct(int productId, byte[] image)
        {
            _repository.InsertImageForProduct(productId, image);
        }

        #endregion

        #region Utils

        private string ProductNotFoundMessage(int id)
        {
            return "No element found with ID = " + id;
        }

        #endregion
    }
}