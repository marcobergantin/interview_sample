using Products.WebApi.Contracts;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Products.WebApi.Services
{
    public class ProductsRepository : IProductsRepository
    {
        ProductModelContainer _context;

        public ProductsRepository()
        {
            _context = new ProductModelContainer();
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            // here we should implement paging otherwise as soon as the number of products is going to increase
            // the database is not going to be happy

            var products = await _context.ProductSet.ToListAsync();

            return products;
        }

        public async Task<Product> GetById(int id)
        {
            var product = await GetProductFromDbById(id);

            return product;
        }

        public async Task Add(Product p)
        {
            _context.ProductSet.Add(p);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Product p)
        {
            _context.Entry(p).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            _context.ProductSet.Remove(product);
            await _context.SaveChangesAsync();
        }

        private async Task<Product> GetProductFromDbById(int id)
        {
            var product = (await _context.ProductSet.Where(x => x.Id == id).ToListAsync()).FirstOrDefault();

            return product;
        }
    }
}