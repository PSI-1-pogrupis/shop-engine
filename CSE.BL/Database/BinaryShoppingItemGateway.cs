using CSE.BL.Interfaces;
using CSE.BL.ShoppingList;
using System.Collections.Generic;
using System.Linq;

namespace CSE.BL.Database
{
    public class BinaryShoppingItemGateway : IShoppingItemGateway
    {
        private string filePath;

        public BinaryShoppingItemGateway(string filePath)
        {
            this.filePath = filePath;
        }

        public ShoppingItem Find(string name, List<ShoppingItem> list)
        {
            return list.Find(b => b.Name.Equals(name));
        }

        public void Insert(ShoppingItem shoppingItem, List<ShoppingItem> list)
        {
            list.Add(shoppingItem);
            BinaryFileManager.WriteToBinaryFile(filePath, list);
        }

        public List<ShoppingItem> Load()
        {
            List<ShoppingItem> readList = BinaryFileManager.ReadFromBinaryFile<ShoppingItem>(filePath);
            return readList;
        }

        public void Remove(string name, List<ShoppingItem> cache)
        {
            foreach (var item in from ShoppingItem item in cache
                                 where item.Name == name
                                 select item)
            {
                cache.Remove(item);
                break;
            }
        }

        public void Update(List<ShoppingItem> cache, ShoppingItem shoppingItem)
        {
            foreach (var item in from ShoppingItem item in cache
                                 where item.Name == shoppingItem.Name
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
    }
}
