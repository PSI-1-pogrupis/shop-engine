using ComparisonShoppingEngineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Services
{
    public interface IProductService
    {
        public IEnumerable<ProductDto> GetAll();

        public ProductDto GetProductByName(string name);

        public ProductDto Insert(ProductDto product);

        public void Delete(ProductDto product);

        public ProductDto Update(ProductDto product);
    }
}
