using System.Collections.Generic;

namespace CSE.BL.Interfaces
{
    public interface IShoppingItemGateway
    {
        int Id { get; set; }
        List<IShoppingItem> Load();
        IShoppingItem Find(int id, List<IShoppingItem> list);
        void Insert(IShoppingItem shoppingItem, List<IShoppingItem> list);
        void Update(List<IShoppingItem> cache, IShoppingItem item);
        void Remove(int? id, List<IShoppingItem> cache);
        void SaveChanges(List<IShoppingItem> cache);
        void SetConnection(object dataPath);
    }
}
