using System.Collections.Generic;
using Products.WebApi.Interfaces;
using Products.WebApi.Models;
using System;
using System.Linq;

namespace Products.WebApi.Services
{
    public class EFProductsService : IProductsService
    {
        #region Fields

        ProductsContext _productsContext;

        #endregion

        #region Constructor

        public EFProductsService()
        {
            _productsContext = new ProductsContext();
        }

        #endregion

        #region Methods

        public IEnumerable<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            foreach (var p in _productsContext.ProductSet)
            {
                products.Add(p);
            }

            return products;
        }

        public async void InsertProduct(ProductModel p)
        {
            Product product = new Product()
            {
                Id = _productsContext.ProductSet.Count(),
                Name = p.Name,
                Price = p.Price,
                LastUpdated = DateTime.Now
            };

            _productsContext.ProductSet.Add(product);
            await _productsContext.SaveChangesAsync();
        }

        public async void ModifyProduct(int id, ProductModel product)
        {
            Product dbEntry = _productsContext.ProductSet.Where(p => p.Id == id)
                                                         .FirstOrDefault();
            if (dbEntry != null)
            {
                dbEntry.Name = product.Name;
                dbEntry.Price = product.Price;
                dbEntry.LastUpdated = DateTime.Now;

                await _productsContext.SaveChangesAsync();
            }

        }

        public async void DeleteProduct(int id)
        {
            Product dbEntry = _productsContext.ProductSet.Where(p => p.Id == id)
                                                         .FirstOrDefault();
            if (dbEntry != null)
            {
                _productsContext.ProductSet.Remove(dbEntry);
                await _productsContext.SaveChangesAsync();
            }
        }

        #endregion
    }
}