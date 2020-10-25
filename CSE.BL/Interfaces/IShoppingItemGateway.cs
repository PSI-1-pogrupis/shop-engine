using CSE.BL.Database;
using CSE.BL.ShoppingList;
using System.Collections.Generic;

namespace CSE.BL.Interfaces
{
    public interface IShoppingItemGateway
    {
        List<ShoppingItemData> Load();
        ShoppingItemData Find(string name, List<ShoppingItemData> list);
        void Insert(ShoppingItemData shoppingItem, List<ShoppingItemData> list);
        void Update(List<ShoppingItemData> cache, ShoppingItemData item);
        void Remove(string name, List<ShoppingItemData> cache);
        void SaveChanges(List<ShoppingItemData> cache);
        void SetConnection(object dataPath);
    }
}
