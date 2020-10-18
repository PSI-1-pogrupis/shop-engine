using CSE.BL.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace CSE.BL.Database
{
    public class ShoppingItemGateway : IShoppingItemGateway
    {
        private string filePath;
        protected int id = 0;

        public ShoppingItemGateway(string filePath)
        {
            this.filePath = filePath;
        }

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public IShoppingItem Find(int id, List<IShoppingItem> list)
        {
            return list.Find(b => b.Id.Equals(id));
        }

        public void Insert(IShoppingItem shoppingItem, List<IShoppingItem> list)
        {
            SetId(shoppingItem);
            list.Add(shoppingItem);
            BinaryFileManager.WriteToBinaryFile(filePath, list);
        }

        public List<IShoppingItem> Load()
        {
            List<IShoppingItem> readList = BinaryFileManager.ReadFromBinaryFile<IShoppingItem>(filePath);
            return readList;
        }

        public void Remove(int? id, List<IShoppingItem> cache)
        {
            foreach (var item in from IShoppingItem item in cache
                                 where item.Id == id
                                 select item)
            {
                cache.Remove(item);
                break;
            }
        }

        public void Update(List<IShoppingItem> cache, IShoppingItem shoppingItem)
        {
            foreach (var item in from IShoppingItem item in cache
                                 where item.Id == shoppingItem.Id
                                 select item)
            {
                item.Name = shoppingItem.Name;
                item.Shop = shoppingItem.Shop;
                item.Unit = shoppingItem.Unit;
                item.Amount = shoppingItem.Amount;
                item.Price = shoppingItem.Price;
                break;
            }
        }

        public void SaveChanges(List<IShoppingItem> cache)
        {
            BinaryFileManager.WriteToBinaryFile(filePath, cache);
        }

        public void SetConnection(object dataPath)
        {
            this.filePath = (string)dataPath;
        }

        private void SetId(IShoppingItem shoppingItem)
        {
            if (shoppingItem.Id < id)
            {
                shoppingItem.Id = ++id;
            }
        }
    }
}
