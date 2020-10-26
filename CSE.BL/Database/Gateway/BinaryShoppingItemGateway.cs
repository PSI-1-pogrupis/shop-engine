using CSE.BL.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace CSE.BL.Database
{
    public class BinaryShoppingItemGateway : IShoppingItemGateway
    {
        private readonly string filePath;
        private readonly IShoppingItemFactory factory;
        private List<ShoppingItemData> cache = null;

        public BinaryShoppingItemGateway(string filePath, IShoppingItemFactory factory)
        {
            this.filePath = filePath;
            this.factory = factory;
            cache = Cache;
        }

        // Load shopping items list into cache
        public List<ShoppingItemData> Cache
        {
            get
            {
                if (cache == null)
                {
                    var data = Load();

                    cache = new List<ShoppingItemData>();

                    foreach (var record in data)
                    {
                        cache.Add(factory.CreateInstance(record));
                    }
                }

                return cache;
            }
        }

        public List<ShoppingItemData> GetAll()
        {
            return cache;
        }

        public ShoppingItemData Find(string name)
        {
            if (cache == null)
            {
                cache = Cache;
            }
            return cache.Find(b => b.Name.Equals(name));
        }

        public void Insert(ShoppingItemData shoppingItem)
        {
            if (cache == null)
            {
                cache = Cache;
            }

            var checkShoppingItem = from p in cache
                                    where p.Name == shoppingItem.Name
                                    select p;

            if (checkShoppingItem.FirstOrDefault() == null)
            {
                cache.Add(shoppingItem);
            }
            else
            {
                foreach (var item in cache.Where(a => a.Name == shoppingItem.Name))
                {
                    item.ShopPrices = shoppingItem.ShopPrices;
                    item.Unit = shoppingItem.Unit;
                    break;
                }
            }
        }

        private List<ShoppingItemData> Load()
        {
            List<ShoppingItemData> readList = BinaryFileManager.ReadFromBinaryFile<ShoppingItemData>(filePath);
            return readList;
        }

        public void Remove(ShoppingItemData shoppingItem)
        {
            if (cache == null)
            {
                cache = Cache;
            }

            foreach (var item in from item in cache
                                 where item.Name == shoppingItem.Name
                                 select item)
            {
                cache.Remove(item);
                break;
            }
        }

        public int SaveChanges()
        {
            return BinaryFileManager.WriteToBinaryFile(filePath, cache);
        }

        public void Dispose()
        {
            cache = null;
        }
    }
}
