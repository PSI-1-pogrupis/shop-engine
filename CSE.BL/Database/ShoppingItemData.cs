using CSE.BL.ShoppingList;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace CSE.BL.Database
{
    [Serializable]
    public class ShoppingItemData
    {
        public string Name { get; set; }

        public UnitTypes Unit { get; set; }

        public Dictionary<ShopTypes, decimal> ShopPrices { get; set; } //dictionary of shopd that contain this item and the item's prices in those shops

        public ShoppingItemData(string name, UnitTypes unit, Dictionary<ShopTypes, decimal> prices)
        {
            Name = name;
            Unit = unit;

            ShopPrices = prices;
        }
    }
}
