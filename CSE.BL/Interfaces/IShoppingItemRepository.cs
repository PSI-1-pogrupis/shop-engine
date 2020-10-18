using System;
using System.Collections.Generic;

namespace CSE.BL.Interfaces
{
    public interface IShoppingItemRepository : IDisposable
    {
        List<IShoppingItem> GetAll();
        IShoppingItem Find(int id);
        void Insert(IShoppingItem shoppingItem);
        void Update(IShoppingItem shoppingItem);
        void Remove(IShoppingItem shoppingItem);
        void SaveChanges();
    }
}
