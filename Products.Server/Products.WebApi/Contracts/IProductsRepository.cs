using System.Collections.Generic;
using System.Threading.Tasks;

namespace Products.WebApi.Interfaces
{
    public interface IProductsRepository
    {
        Task<Product> GetById(int id);
        Task<IEnumerable<Product>> GetAll();
        Task Add(Product p);
        Task Update(Product p);
        Task Delete(Product p);
    }
}
