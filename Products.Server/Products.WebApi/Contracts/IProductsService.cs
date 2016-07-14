using Products.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Products.WebApi.Contracts
{
    public interface IProductsService
    {
        Task<IEnumerable<ViewProductDto>> GetProducts();
        Task<ViewProductDto> GetProduct(int id);
        Task InsertProduct(AddEditProductDto productToInsert);
        Task InsertImageForProduct(int productId, byte[] image);
        Task ModifyProduct(int id, AddEditProductDto p);
        Task DeleteProduct(int id);
    }
}