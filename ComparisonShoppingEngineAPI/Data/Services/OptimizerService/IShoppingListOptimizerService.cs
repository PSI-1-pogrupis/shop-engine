using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs;
using ComparisonShoppingEngineAPI.DTOs.Optimization;
using ComparisonShoppingEngineAPI.DTOs.ShoppingList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Services.OptimizerService
{
    public interface IShoppingListOptimizerService
    {
        public Task<ServiceResponse<List<ShoppingItem>>> OptimizeList(GetOptimizingShoppingListDto dto, List<ProductDto> dataList);

        public Task<ServiceResponse<List<OptimizedShopListDto>>> GetOrderedShopLists(GetOptimizingShoppingListDto dto, List<ProductDto> dataList);
    }
}
