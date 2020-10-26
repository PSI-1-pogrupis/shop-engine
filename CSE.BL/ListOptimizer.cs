using CSE.BL.Database;
using CSE.BL.ShoppingList;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL
{
    /*
     * Optimizer generates optimal shopping lists based on certain criteria.
     */

    public class ListOptimizer
    {
        public ListOptimizer() { }

        // Generates lowest price shopping lists from a list of shops that the buyer may visit.
        public ShoppingListManager GetLowestPriceList(ShoppingListManager list, List<ShoppingItemData> dataList, List<ShopTypes> availableShops, bool onlyReplaceUnspecified)
        {
            
            ShoppingListManager newList = new ShoppingListManager();
            
            for(int i = 0; i < list.ShoppingList.Count; i++)
            {
                ShoppingItem item = list.ShoppingList[i];

                KeyValuePair<ShopTypes, decimal> bestPrice = new KeyValuePair<ShopTypes, decimal>(ShopTypes.UNKNOWN, decimal.MaxValue);

                if (!onlyReplaceUnspecified || item.Shop == ShopTypes.UNKNOWN)
                {
                    ShoppingItemData itemData = dataList[i];

                    foreach (KeyValuePair<ShopTypes, decimal> shop in itemData.ShopPrices)
                    {
                        if (shop.Key == ShopTypes.UNKNOWN) continue;

                        if (!availableShops.Contains(shop.Key)) continue;

                        if (shop.Value < bestPrice.Value) bestPrice = shop;
                    }
                }
                else
                {
                    bestPrice = new KeyValuePair<ShopTypes, decimal>(item.Shop, item.Price);
                }

                ShoppingItem newItem = new ShoppingItem(item.Name, bestPrice.Key, bestPrice.Value, item.Amount, item.Unit);

                newList.AddItem(newItem);
            }
            
            return newList;
        }
    }
}
