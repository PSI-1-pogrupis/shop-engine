using ComparisonShoppingEngineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data
{
    public interface IProductRepository : IDisposable
    {
        public IEnumerable<ProductData> GetAll();

        public ProductData GetProductByName(string name);

        public void Insert(ProductData product);

        public void Delete(ProductData product);

        public void Update(ProductData data);
    }
}
