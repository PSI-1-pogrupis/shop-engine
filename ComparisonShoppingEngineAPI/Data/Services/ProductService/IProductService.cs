using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data
{
    public interface IProductService
    {
        public Task<ServiceResponse<List<ProductDto>>> GetAll();

        public Task<ServiceResponse<ProductDto>> GetProductByName(string name);

        public Task<ServiceResponse<List<ProductDto>>> Insert(ProductDto product);

        public Task<ServiceResponse<List<ProductDto>>> Delete(string name);

        public Task<ServiceResponse<List<ProductDto>>> Update(ProductDto product);
    }
}
