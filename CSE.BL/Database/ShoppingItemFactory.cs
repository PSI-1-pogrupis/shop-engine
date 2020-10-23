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
        public ShoppingItem CreateInstance(ShoppingItem data)
        {
            if (data == null)
                return null;

            ShoppingItem instance = new ShoppingItem(data);

            return instance;
        }
        // Create new ready-to-use object using values
        public ShoppingItem CreateInstance(int id, string name, double amount, UnitTypes unit, Dictionary<ShopTypes, double> prices)
        {
            ShoppingItem instance = new ShoppingItem(name, amount, unit, prices);

            return instance;
        }

        // Update passed object
        public void UpdateInstance(ShoppingItem data, ShoppingItem instance)
        {
            instance.Name = data.Name;
            instance.Unit = data.Unit;
            instance.ShopPrices = data.ShopPrices;
            instance.Amount = data.Amount;
        }
    }
}
