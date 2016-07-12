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

        #region IProductsService Implements

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
                throw new ArgumentException(this.ProductNotFoundMessage(id));
            }
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
                throw new ArgumentException(this.ProductNotFoundMessage(id));
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
                throw new ArgumentException(this.ProductNotFoundMessage(id));
            }
        }

        public async void InsertImageForProduct(int productId, byte[] image)
        {
            Product dbEntry = this.GetProductFromDB(productId);
            if (dbEntry != null)
            {
                await this.InsertImageForProduct(dbEntry, image);
            }
            else
            {
                throw new ArgumentException(this.ProductNotFoundMessage(productId));
            }
        }

        private async Task<int> InsertImageForProduct(Product product, byte[] image)
        {
            product.Image = image;
            return await _productsContext.SaveChangesAsync();
        }

        #endregion

        #region Utils

        private Product GetProductFromDB(int id)
        {
            return _productsContext.ProductSet.Where(p => p.Id == id)
                                                         .FirstOrDefault();
        }

        private string ProductNotFoundMessage(int id)
        {
            return "No element found with ID = " + id;
        }

        #endregion
    }
}