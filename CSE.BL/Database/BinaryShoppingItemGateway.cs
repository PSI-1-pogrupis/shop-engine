using CSE.BL.Interfaces;
using CSE.BL.ShoppingList;
using System.Collections.Generic;
using System.Linq;

namespace CSE.BL.Database
{
    public class BinaryShoppingItemGateway : IShoppingItemGateway
    {
        private string filePath;
        protected int id = 0;

        public BinaryShoppingItemGateway(string filePath)
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

        public ShoppingItem Find(int id, List<ShoppingItem> list)
        {
            return list.Find(b => b.Id.Equals(id));
        }

        public void Insert(ShoppingItem shoppingItem, List<ShoppingItem> list)
        {
            SetId(shoppingItem);
            list.Add(shoppingItem);
            BinaryFileManager.WriteToBinaryFile(filePath, list);
        }

        public List<ShoppingItem> Load()
        {
            List<ShoppingItem> readList = BinaryFileManager.ReadFromBinaryFile<ShoppingItem>(filePath);
            return readList;
        }

        public void Remove(int? id, List<ShoppingItem> cache)
        {
            foreach (var item in from ShoppingItem item in cache
                                 where item.Id == id
                                 select item)
            {
                cache.Remove(item);
                break;
            }
        }

        public void Update(List<ShoppingItem> cache, ShoppingItem shoppingItem)
        {
            foreach (var item in from ShoppingItem item in cache
                                 where item.Id == shoppingItem.Id
                                 select item)
            {
                item.Name = shoppingItem.Name;
                item.Unit = shoppingItem.Unit;
                item.Amount = shoppingItem.Amount;
                item.ShopPrices = shoppingItem.ShopPrices;
                break;
            }
        }

        public void SaveChanges(List<ShoppingItem> cache)
        {
            BinaryFileManager.WriteToBinaryFile(filePath, cache);
        }

        public void SetConnection(object dataPath)
        {
            this.filePath = (string)dataPath;
        }

        private void SetId(ShoppingItem shoppingItem)
        {
            if (shoppingItem.Id == null)
            {
                shoppingItem.Id = ++id;
            }
        }
    }
}
