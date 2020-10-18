using CSE.BL.ShoppingList;
using System.Collections.Generic;

namespace CSE.BL.Interfaces
{
    public interface IShoppingItemGateway
    {
        int Id { get; set; }
        List<ShoppingItem> Load();
        ShoppingItem Find(int id, List<ShoppingItem> list);
        void Insert(ShoppingItem shoppingItem, List<ShoppingItem> list);
        void Update(List<ShoppingItem> cache, ShoppingItem item);
        void Remove(int? id, List<ShoppingItem> cache);
        void SaveChanges(List<ShoppingItem> cache);
        void SetConnection(object dataPath);
    }
}
