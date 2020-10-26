using CSE.BL.Database;
using System;
using System.Collections.Generic;

namespace CSE.BL.Interfaces
{
    public interface IShoppingItemGateway : IDisposable
    {
        List<ShoppingItemData> GetAll();
        ShoppingItemData Find(string shoppingItemName);
        void Insert(ShoppingItemData shoppingItem);
        void Remove(ShoppingItemData shoppingItem);
        int SaveChanges();
    }
}
