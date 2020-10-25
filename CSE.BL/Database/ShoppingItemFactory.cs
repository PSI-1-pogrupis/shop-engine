using CSE.BL.Interfaces;
using CSE.BL.ShoppingList;
using System.Collections.Generic;

namespace CSE.BL.Database
{
    /* Defines a way of creating and updating object.
     * Factory knows how to take all the nformation
     * needed to build an object and return ready-to-use object.*/
    public class ShoppingItemFactory : IShoppingItemFactory
    {
        // Create new ready-to-use object
        public ShoppingItemData CreateInstance(ShoppingItemData data)
        {
            if (data == null)
                return null;

            ShoppingItemData instance = new ShoppingItemData(data.Name, data.Unit, data.ShopPrices);

            return instance;
        }
        // Create new ready-to-use object using values
        public ShoppingItemData CreateInstance(int id, string name, UnitTypes unit, Dictionary<ShopTypes, double> prices)
        {
            ShoppingItemData instance = new ShoppingItemData(name, unit, prices);

            return instance;
        }

        // Update passed object
        public void UpdateInstance(ShoppingItemData data, ShoppingItemData instance)
        {
            instance.Name = data.Name;
            instance.Unit = data.Unit;
            instance.ShopPrices = data.ShopPrices;
        }
    }
}
