using CSE.BL.ShoppingList;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL
{
    public class ListOptimizer
    {
        public ListOptimizer() { }

        public ShoppingListManager GetLowestPriceList(ShoppingListManager list, List<string> availableShops, bool onlyReplaceUnspecified)
        {
            ShoppingListManager newList = new ShoppingListManager();

            foreach(ShoppingItem item in list.ShoppingList)
            {
                ShoppingItem newItem = new ShoppingItem(item);

                (string, double) bestPrice = ("NULL", double.PositiveInfinity);

                if (!onlyReplaceUnspecified || item.SelectedShopName.Equals("ANY"))
                {
                    foreach ((string, double) shop in item.ShopPrices)
                    {
                        if (shop.Item1.Equals("ANY")) continue;

                        if (!availableShops.Contains(shop.Item1)) continue;

                        if (shop.Item2 < bestPrice.Item2) bestPrice = shop;
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
