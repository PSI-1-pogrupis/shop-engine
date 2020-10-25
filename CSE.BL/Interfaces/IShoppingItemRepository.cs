using CSE.BL.Database;
using CSE.BL.ShoppingList;
using System;
using System.Collections.Generic;

namespace CSE.BL.Interfaces
{
    public interface IShoppingItemRepository : IDisposable
    {
        List<ShoppingItemData> GetAll();
        ShoppingItemData Find(string id);
        void Insert(ShoppingItemData shoppingItem);
        void Update(ShoppingItemData shoppingItem);
        void Remove(ShoppingItemData shoppingItem);
        void SaveChanges();
    }
}
