using CSE.BL.ShoppingList;
using System.Collections.Generic;

namespace CSE.BL.Interfaces
{
    public interface IShoppingItemFactory
    {
        ShoppingItem CreateInstance(ShoppingItem data);
        ShoppingItem CreateInstance(int id, string name, double amount, UnitTypes unit, Dictionary<ShopTypes, double> prices);
        void UpdateInstance(ShoppingItem data, ShoppingItem instance);
    }
}
