using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.Data.Services.OptimizerService;
using ComparisonShoppingEngineAPI.DTOs;
using ComparisonShoppingEngineAPI.DTOs.Optimization;
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
            /*
            if(dataList.Count <= dto.ShoppingList.Count)
            {
                response.Success = false;
                response.Message = "No information about a product found.";
                return response;
            }
            */
            for(int i = 0; i < dto.ShoppingList.Count; i++)
            {
                GetShoppingItemDto item = dto.ShoppingList[i];

                if (item.Shop == "UNKNOWN") continue;

                if (dataList[i] == null) continue;

                if (!dataList[i].ShopPrices.Any(x => x.Key == item.Shop))
                {
                    response.Success = false;
                    response.Message = "Shop: " + item.Shop + " does not sell item: " + item.Name;
                    return response;
                }
            }

            List<ShoppingItem> optimizedList = await GetLowestPriceListAsync(dto.ShoppingList, dataList, dto.AllowedShops, dto.OnlyReplaceUnspecifiedShops);
            response.Data = optimizedList;

            return response;
        }

        public async Task<ServiceResponse<List<OptimizedShopListDto>>> GetOrderedShopLists(GetOptimizingShoppingListDto dto, List<ProductDto> dataList)
        {
            ServiceResponse<List<OptimizedShopListDto>> response = new ServiceResponse<List<OptimizedShopListDto>>();
            /*
            if (dataList.Count <= dto.ShoppingList.Count)
            {
                response.Success = false;
                response.Message = "No information about a product found.";
                return response;
            }
            */
            for (int i = 0; i < dto.ShoppingList.Count; i++)
            {
                GetShoppingItemDto item = dto.ShoppingList[i];

                if (item.Shop == "UNKNOWN") continue;

                if (dataList[i] == null) continue;

                if (!dataList[i].ShopPrices.Any(x => x.Key == item.Shop))
                {
                    response.Success = false;
                    response.Message = "Shop: " + item.Shop + " does not sell item: " + item.Name;
                    return response;
                }
            }

            List<OptimizedShopListDto> optimizedList = await GetShopLists(dto.ShoppingList, dataList, dto.AllowedShops, dto.OnlyReplaceUnspecifiedShops);
            optimizedList = optimizedList.OrderBy(a => !a.HasAllProducts).ThenBy(a => a.ListPrice).ToList();
            response.Data = optimizedList;

            return response;
        }

        public async Task<List<ShoppingItem>> GetLowestPriceListAsync(List<GetShoppingItemDto> list, List<ProductDto> dataList, List<string> availableShops, bool onlyReplaceUnspecified)
        {
            return await Task.Run(() => GetLowestPriceList(list, dataList, availableShops, onlyReplaceUnspecified));
        }

        // Generates lowest price shopping lists from a list of shops that the buyer may visit.
        public List<ShoppingItem> GetLowestPriceList(List<GetShoppingItemDto> list, List<ProductDto> dataList, List<string> availableShops, bool onlyReplaceUnspecified)
        {
            //if (dataList.Count <= list.Count) return null;

            List<ShoppingItem> newList = new List<ShoppingItem>();

            for (int i = 0; i < list.Count; i++)
            {
                GetShoppingItemDto item = list[i];

                KeyValuePair<string, decimal> bestPrice = new KeyValuePair<string, decimal>("UNKNOWN", decimal.MaxValue);

                if (!onlyReplaceUnspecified || item.Shop == "UNKNOWN")
                {
                    ProductDto itemData = dataList[i];

                    foreach (KeyValuePair<string, decimal> shop in itemData.ShopPrices)
                    {
                        if (shop.Key == "UNKNOWN") continue;
                        if (!availableShops.Contains(shop.Key)) continue;
                        if (shop.Value < bestPrice.Value) bestPrice = shop;
                    }
                }
                else if (dataList[i] != null) bestPrice = new KeyValuePair<string, decimal>(item.Shop, dataList[i].ShopPrices.Single(x => x.Key == item.Shop).Value);

                if (bestPrice.Key == "UNKNOWN") bestPrice = new KeyValuePair<string, decimal>("UNKNOWN", 0);

                if (dataList[i] == null) continue;

                ShoppingItem newItem = new ShoppingItem()
                {
                    Name = item.Name,
                    Shop = bestPrice.Key,
                    PricePerUnit = bestPrice.Value,
                    Amount = item.Amount,
                    Unit = dataList[i].Unit
                };

                newList.Add(newItem);
            }

            return newList;
        }

        public async Task<List<OptimizedShopListDto>> GetShopLists(List<GetShoppingItemDto> list, List<ProductDto> dataList, List<string> availableShops, bool onlyReplaceUnspecified)
        {
            List<OptimizedShopListDto> result = new List<OptimizedShopListDto>();

            foreach (var shop in availableShops)
            {
                OptimizedShopListDto shopList = new OptimizedShopListDto();

                var generated = await GetLowestPriceListAsync(list, dataList, new List<string>() { shop }, onlyReplaceUnspecified);
                if (generated != null)
                {
                    decimal price = 0;

                    foreach (var item in generated)
                    {
                        price += item.PricePerUnit * (decimal)item.Amount;

                        if (item.Shop == "UNKNOWN") shopList.HasAllProducts = false;
                    }

                    shopList.ListPrice = price;
                }

                shopList.ItemList = generated;
                shopList.Shop = shop;

                result.Add(shopList);
            }

            return result;
        }
    }
}
