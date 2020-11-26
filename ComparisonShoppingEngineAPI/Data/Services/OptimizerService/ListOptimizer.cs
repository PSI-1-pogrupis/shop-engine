using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs;
using ComparisonShoppingEngineAPI.DTOs.ShoppingList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Services.OptimizerService
{
    public class ListOptimizer
    {
        public ListOptimizer() { }

        public async Task<List<ShoppingItem>> GetLowestPriceListAsync(List<GetShoppingItemDto> list, List<ProductDto> dataList, List<string> availableShops, bool onlyReplaceUnspecified)
        {
            return await Task.Run(() => GetLowestPriceList(list, dataList, availableShops, onlyReplaceUnspecified));
        }

        // Generates lowest price shopping lists from a list of shops that the buyer may visit.
        public List<ShoppingItem> GetLowestPriceList(List<GetShoppingItemDto> list, List<ProductDto> dataList, List<string> availableShops, bool onlyReplaceUnspecified)
        {
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
                else
                {
                    bestPrice = new KeyValuePair<string, decimal>(item.Shop, dataList[i].ShopPrices.Single(x => x.Key == item.Shop).Value);
                }

                if (bestPrice.Key == "UNKNOWN") bestPrice = new KeyValuePair<string, decimal>("UNKNOWN", 0);

                ShoppingItem newItem = new ShoppingItem() { Name = item.Name, 
                                                            Shop = bestPrice.Key, 
                                                            PricePerUnit = bestPrice.Value, 
                                                            Amount = item.Amount, 
                                                            Unit = dataList[i].Unit };

                newList.Add(newItem);
            }

            return newList;
        }
    }
}
