using Products.WebApi.Models;
using System.Collections.Generic;

namespace Products.WebApi.Interfaces
{
    public interface IProductsService
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(int id);
        void InsertProduct(ProductModel p);
        void InsertImageForProduct(int productId, byte[] image);
        void ModifyProduct(int id, ProductModel p);
        void DeleteProduct(int id);
    }
}