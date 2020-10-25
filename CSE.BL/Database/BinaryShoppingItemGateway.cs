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

        public ShoppingItemData Find(string name, List<ShoppingItemData> list)
        {
            return list.Find(b => b.Name.Equals(name));
        }

        public void Insert(ShoppingItemData shoppingItem, List<ShoppingItemData> list)
        {
            list.Add(shoppingItem);
            BinaryFileManager.WriteToBinaryFile(filePath, list);
        }

        public List<ShoppingItemData> Load()
        {
            List<ShoppingItemData> readList = BinaryFileManager.ReadFromBinaryFile<ShoppingItemData>(filePath);
            return readList;
        }

        public void Remove(string name, List<ShoppingItemData> cache)
        {
            foreach (var item in from ShoppingItemData item in cache
                                 where item.Name == name
                                 select item)
            {
                cache.Remove(item);
                break;
            }
        }

        public void Update(List<ShoppingItemData> cache, ShoppingItemData shoppingItem)
        {
            foreach (var item in from ShoppingItemData item in cache
                                 where item.Name == shoppingItem.Name
                                 select item)
            {
                item.Name = shoppingItem.Name;
                item.Unit = shoppingItem.Unit;
                item.ShopPrices = shoppingItem.ShopPrices;
                break;
            }
        }

        public void SaveChanges(List<ShoppingItemData> cache)
        {
            BinaryFileManager.WriteToBinaryFile(filePath, cache);
        }

        public void SetConnection(object dataPath)
        {
            filePath = (string)dataPath;
        }
    }
}
