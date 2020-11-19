using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data
{
    public interface IProductService
    {
        public Task<ServiceResponse<List<GetProductDto>>> GetAll();

        public Task<ServiceResponse<GetProductDto>> GetProductByName(string name);

        public Task<ServiceResponse<List<GetProductDto>>> Insert(AddProductDto product);

        public Task<ServiceResponse<List<GetProductDto>>> Delete(string name);

        public Task<ServiceResponse<List<GetProductDto>>> Update(AddProductDto product);
    }
}
