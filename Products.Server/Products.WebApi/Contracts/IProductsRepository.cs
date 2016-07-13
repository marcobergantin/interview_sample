using Products.WebApi.Models;
using System.Collections.Generic;

namespace Products.WebApi.Interfaces
{
    public interface IProductsRepository
    {
        Product GetProduct(int id);
        IEnumerable<Product> GetAllProducts();
        void InsertProduct(Product p);
        void ModifyProduct(int id, ProductModel p);
        void DeleteProduct(int id);
        void InsertImageForProduct(int id, byte[] buffer);
    }
}
