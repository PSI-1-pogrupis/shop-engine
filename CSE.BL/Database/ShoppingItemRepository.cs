using CSE.BL.Interfaces;
using CSE.BL.ShoppingList;
using System.Collections.Generic;

namespace CSE.BL.Database
{
    /* Repository connects Gateways and Factories.
     * Repository will persist the objects using
     * the Gateway and cache them.*/
    public class ShoppingItemRepository : IShoppingItemRepository, IDatabaseConnection
    {
        private readonly IShoppingItemGateway gateway;
        private readonly IShoppingItemFactory factory;
        private List<ShoppingItemData> cache = null;
        // Load shopping items list into cache
        public List<ShoppingItemData> Cache
        {
            get
            {
                if (cache == null)
                {
                    var data = gateway.Load();

                    cache = new List<ShoppingItemData>();

                    foreach (var record in data)
                    {
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
            this.gateway = gateway ?? new BinaryShoppingItemGateway(System.AppDomain.CurrentDomain.BaseDirectory + "\\ShoppingItemsList.bin");
            this.factory = factory ?? new ShoppingItemFactory();
        }
        // Insert Shopping Item into database
        public void Insert(ShoppingItemData shoppingItem)
        {
            if (cache == null)
            {
                gateway.Insert(shoppingItem, Cache);
            }
            else
            {
                gateway.Insert(shoppingItem, cache);
            }
        }
        public ShoppingItemData Find(string name)
        {
            return gateway.Find(name, Cache);
        }
        // Retrieve all Shopping Items
        public List<ShoppingItemData> GetAll()
        {
            return Cache;
        }
        // Remove specific Shopping Item
        public void Remove(ShoppingItemData shoppingItem)
        {
            if (cache == null && shoppingItem != null)
            {
                gateway.Remove(shoppingItem.Name, Cache);
            }
            else if (shoppingItem != null)
            {
                gateway.Remove(shoppingItem.Name, cache);
            }
        }
        // Update specific Shopping Item
        public void Update(ShoppingItemData shoppingItem)
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
        }
    }
}
