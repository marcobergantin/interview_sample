using System.Collections.Generic;
using Products.WebApi.Interfaces;
using Products.WebApi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public Product GetProduct(int id)
        {
            Product dbEntry = this.GetProductFromDB(id);
            if (dbEntry != null)
            {
                return dbEntry;
            }
            else
            {
                throw new ArgumentException("No element found with ID = " + id);
            }
        }

        public async Task<Product> InsertProduct(ProductModel p)
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
            return product;
        }

        public async void ModifyProduct(int id, ProductModel product)
        {
            Product dbEntry = this.GetProductFromDB(id);
            if (dbEntry != null)
            {
                dbEntry.Name = product.Name;
                dbEntry.Price = product.Price;
                dbEntry.LastUpdated = DateTime.Now;

                await _productsContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("No element found with ID = " + id);
            }
        }

        public async void DeleteProduct(int id)
        {
            Product dbEntry = this.GetProductFromDB(id);
            if (dbEntry != null)
            {
                _productsContext.ProductSet.Remove(dbEntry);
                await _productsContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("No element found with ID = " + id);
            }
        }

        private Product GetProductFromDB(int id)
        {
            return _productsContext.ProductSet.Where(p => p.Id == id)
                                                         .FirstOrDefault();
        }

        #endregion
    }
}