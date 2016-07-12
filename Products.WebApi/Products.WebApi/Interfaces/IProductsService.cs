using Products.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Products.WebApi.Interfaces
{
    public interface IProductsService
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(int id);
        Task<Product> InsertProduct(ProductModel p);
        void ModifyProduct(int id, ProductModel p);
        void DeleteProduct(int id);
    }

    public enum StatusCode
    {
        Ok,
        NotFound,
        Created,

    }
}