using Products.WebApi.Models;
using System.Collections.Generic;

namespace Products.WebApi.Interfaces
{
    public interface IProductsService
    {
        IEnumerable<Product> GetProducts();
        void InsertProduct(ProductModel p);
        void ModifyProduct(int id, ProductModel p);
    }
}