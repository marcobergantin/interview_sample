using Products.WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Products.WebApi.Models;

namespace Products.WebApi.Services
{
    public class ProductsRepository : IProductsRepository
    {
        ProductsContext _context;

        public ProductsRepository()
        {
            _context = new ProductsContext();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            foreach (var p in _context.ProductSet)
            {
                products.Add(p);
            }

            return products;
        }

        public Product GetProduct(int id)
        {
            return this.GetProductFromDB(id);
        }

        public async void InsertProduct(Product p)
        {
            _context.ProductSet.Add(p);
            await _context.SaveChangesAsync();
        }

        public async void ModifyProduct(int id, ProductModel p)
        {
            Product dbEntry = this.GetProductFromDB(id);
            if (dbEntry != null)
            {
                dbEntry.Name = p.Name;
                dbEntry.Price = p.Price;
                dbEntry.LastUpdated = DateTime.Now;

                await _context.SaveChangesAsync();
            }
        }

        public async void DeleteProduct(int id)
        {
            Product dbEntry = this.GetProductFromDB(id);
            if (dbEntry != null)
            {
                _context.ProductSet.Remove(dbEntry);
                await _context.SaveChangesAsync();
            }
        }

        public async void InsertImageForProduct(int id, byte[] buffer)
        {
            Product product = this.GetProductFromDB(id);
            if (product != null)
            {
                product.Image = buffer;
                product.LastUpdated = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        private Product GetProductFromDB(int id)
        {
            return _context.ProductSet.Where(p => p.Id == id)
                                                         .FirstOrDefault();
        }
    }
}