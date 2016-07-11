using System.Collections.Generic;
using Products.WebApi.Interfaces;

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

        }

        #endregion

        #region Methods

        public IEnumerable<Product> GetProducts()
        {
            this.LazyInit();

            List<Product> products = new List<Product>();
            foreach (var p in _productsContext.ProductSet)
            {
                products.Add(p);
            }

            return products;
        }

        public async void InsertProduct(Product p)
        {
            this.LazyInit();

            _productsContext.ProductSet.Add(p);
            await _productsContext.SaveChangesAsync();
        }

        private void LazyInit()
        {
            if (_productsContext == null)
            {
                _productsContext = new ProductsContext();
            }
        }

        #endregion
    }
}