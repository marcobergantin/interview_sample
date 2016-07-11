using System.Collections.Generic;

namespace Products.WebApi.Interfaces
{
    public interface IProductsService
    {
        IEnumerable<Product> GetProducts();
        void InsertProduct(Product p);
    }
}