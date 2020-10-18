using CSE.BL.Interfaces;
using CSE.BL.ShoppingList;
using System.Collections.Generic;
using System.IO;

namespace CSE.BL.Database
{
    /* Repository connects Gateways and Factories.
     * Repository will persist the objects using
     * the Gateway and cache them.*/
    public class ShoppingItemRepository : IShoppingItemRepository, IDatabaseConnection
    {
        private readonly IShoppingItemGateway gateway;
        private readonly IShoppingItemFactory factory;
        private List<ShoppingItem> cache;
        // Load shopping items list into cache
        public List<ShoppingItem> Cache
        {
            get
            {
                if (cache == null)
                {
                    var data = gateway.Load();

                    cache = new List<ShoppingItem>();

                    foreach (var record in data)
                    {
                        if (record.Id != null && record.Id > gateway.Id)
                            gateway.Id = record.Id.Value;
                        cache.Add(factory.CreateInstance(record));
                    }
                }

                return cache;
            }
        }
        // Initialize repository gateway and factory (future database implementation)
        public ShoppingItemRepository(IShoppingItemGateway gateway = null, IShoppingItemFactory factory = null)
        {
            // Default file path to local file if database gateway is not set
            this.gateway = gateway ?? new ShoppingItemGateway(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName).FullName + "\\CSE.BL\\LocalData\\ShoppingItemsList.bin");
            this.factory = factory ?? new ShoppingItemFactory();
        }
        // Insert Shopping Item into database
        public void Insert(ShoppingItem shoppingItem)
        {
            if (shoppingItem.Id == null)
            {
                shoppingItem.Id = ++gateway.Id;
            }
            if (cache == null)
            {
                gateway.Insert(shoppingItem, Cache);
            }
            else
            {
                gateway.Insert(shoppingItem, cache);
            }
        }
        // Retrieve searched Shopping Item
        public ShoppingItem Find(int id)
        {
            if (cache == null)
                return gateway.Find(id, Cache);
            return gateway.Find(id, cache);
        }
        // Retrieve all Shopping Items
        public List<ShoppingItem> GetAll()
        {
            if (cache == null)
                return Cache;
            return cache;
        }
        // Remove specific Shopping Item
        public void Remove(ShoppingItem shoppingItem)
        {
            if (cache == null && shoppingItem != null)
            {
                gateway.Remove(shoppingItem.Id, Cache);
            }
            else if (shoppingItem != null)
            {
                gateway.Remove(shoppingItem.Id, cache);
            }
        }
        // Update specific Shopping Item
        public void Update(ShoppingItem shoppingItem)
        {
            if (cache == null)
                gateway.Update(Cache, shoppingItem);
            gateway.Update(cache, shoppingItem);
        }
        // Save any changes to database by provided gateway
        public void SaveChanges()
        {
            gateway.SaveChanges(cache);
        }
        // Set database connection (file path object if to a file)
        public void SetConnection(object filePath)
        {
            gateway.SetConnection(filePath);
        }
        // Close connection and save changes
        public void Dispose()
        {
            SaveChanges();
        }
    }
}
