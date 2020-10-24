using CSE.BL.ShoppingList;
using System;
using System.Collections.Generic;

namespace CSE.BL.Interfaces
{
    public interface IShoppingItemRepository : IDisposable
    {
        List<ShoppingItem> GetAll();
        ShoppingItem Find(string id);
        void Insert(ShoppingItem shoppingItem);
        void Update(ShoppingItem shoppingItem);
        void Remove(ShoppingItem shoppingItem);
        void SaveChanges();
    }
}
