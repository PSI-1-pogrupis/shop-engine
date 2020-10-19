using CSE.BL.ShoppingList;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL
{
    public class ListOptimizer
    {
        public ListOptimizer() { }

        public ShoppingListManager GetLowestPriceList(ShoppingListManager list, List<ShopTypes> availableShops, bool onlyReplaceUnspecified)
        {
            ShoppingListManager newList = new ShoppingListManager();

            foreach(ShoppingItem item in list.ShoppingList)
            {
                ShoppingItem newItem = new ShoppingItem(item);

                KeyValuePair<ShopTypes, double> bestPrice = new KeyValuePair<ShopTypes, double>(ShopTypes.UNKNOWN, double.PositiveInfinity);

                if (!onlyReplaceUnspecified || item.SelectedShopName == ShopTypes.UNKNOWN)
                {
                    foreach (KeyValuePair<ShopTypes, double> shop in item.ShopPrices)
                    {
                        if (shop.Key == ShopTypes.UNKNOWN) continue;

                        if (!availableShops.Contains(shop.Key)) continue;

                        if (shop.Value < bestPrice.Value) bestPrice = shop;
                    }
                }
                else
                {
                    bestPrice = item.SelectedShop;
                }

                newItem.SelectedShop = bestPrice;
                newList.AddItem(newItem);
            }

            return newList;
        }
    }
}
