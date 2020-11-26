using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.Data.Services.OptimizerService;
using ComparisonShoppingEngineAPI.DTOs;
using ComparisonShoppingEngineAPI.DTOs.ShoppingList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Services
{
    public class ShoppingListOptimizerService : IShoppingListOptimizerService
    {
        public async Task<ServiceResponse<List<ShoppingItem>>> OptimizeList(GetOptimizingShoppingListDto dto, List<ProductDto> dataList)
        {
            ServiceResponse<List<ShoppingItem>> response = new ServiceResponse<List<ShoppingItem>>();
            ListOptimizer optimizer = new ListOptimizer();

            for(int i = 0; i < dto.ShoppingList.Count; i++)
            {
                GetShoppingItemDto item = dto.ShoppingList[i];

                if (item.Shop == "UNKNOWN") continue;

                if (!dataList[i].ShopPrices.Any(x => x.Key == item.Shop))
                {
                    response.Success = false;
                    response.Message = "Shop: " + item.Shop + " does not sell item: " + item.Name;
                    return response;
                }
            }

            List<ShoppingItem> optimizedList = await optimizer.GetLowestPriceListAsync(dto.ShoppingList, dataList, dto.AllowedShops, dto.OnlyReplaceUnspecifiedShops);
            response.Data = optimizedList;

            return response;
        }
    }
}
