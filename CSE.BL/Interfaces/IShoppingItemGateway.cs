using CSE.BL.ShoppingList;
using System.Collections.Generic;

namespace CSE.BL.Interfaces
{
    public interface IShoppingItemGateway
    {
        List<ShoppingItem> Load();
        ShoppingItem Find(string name, List<ShoppingItem> list);
        void Insert(ShoppingItem shoppingItem, List<ShoppingItem> list);
        void Update(List<ShoppingItem> cache, ShoppingItem item);
        void Remove(string name, List<ShoppingItem> cache);
        void SaveChanges(List<ShoppingItem> cache);
        void SetConnection(object dataPath);
    }
}
