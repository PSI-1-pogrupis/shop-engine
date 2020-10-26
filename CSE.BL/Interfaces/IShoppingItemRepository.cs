using CSE.BL.Database;
using System;
using System.Collections.Generic;

namespace CSE.BL.Interfaces
{
    public interface IShoppingItemRepository : IDisposable
    {
        // Retrieve all existing shopping items
        List<ShoppingItemData> GetAll();
        // Find shopping item by name
        ShoppingItemData Find(string name);
        // Insert and if exists update shopping item
        void Insert(ShoppingItemData shoppingItem);
        // Remove shopping item
        void Remove(ShoppingItemData shoppingItem);
        // Save made changes to specified gateway
        int SaveChanges();
    }
}
