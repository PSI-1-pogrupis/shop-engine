using CSE.BL.Database;
using CSE.BL.ShoppingList;
using System.Collections.Generic;

namespace CSE.BL.Interfaces
{
    public interface IShoppingItemFactory
    {
        ShoppingItemData CreateInstance(ShoppingItemData data);
        ShoppingItemData CreateInstance(int id, string name, UnitTypes unit, Dictionary<ShopTypes, double> prices);
        void UpdateInstance(ShoppingItemData data, ShoppingItemData instance);
    }
}
