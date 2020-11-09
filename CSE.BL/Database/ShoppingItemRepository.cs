using CSE.BL.Interfaces;
using CSE.BL.ShoppingList;
using System;
using System.Collections.Generic;

namespace CSE.BL.Database
{
    /* Repository connects Gateways and Factories.
     * Repository will persist the objects using
     * the Gateway and cache them.*/
    public class ShoppingItemRepository : IShoppingItemRepository
    {
        private readonly IShoppingItemGateway gateway;

        // Initialize repository gateway and factory (future database implementation)
        public ShoppingItemRepository(IShoppingItemGateway gateway = null, IShoppingItemFactory factory = null)
        {
            // Default file path to local file if database gateway is not set
            this.gateway = gateway ?? new BinaryShoppingItemGateway(System.AppDomain.CurrentDomain.BaseDirectory + "\\ShoppingItemsList.bin", factory ?? new ShoppingItemFactory());
        }
        // Insert Shopping Item into database
        public void Insert(ShoppingItemData shoppingItem)
        {
            gateway.Insert(shoppingItem);
        }
        public ShoppingItemData Find(string name)
        {
            return gateway.Find(name);
        }
        // Retrieve all Shopping Items
        public List<ShoppingItemData> GetAll()
        {
            return gateway.GetAll();
        }
        // Remove specific Shopping Item
        public void Remove(ShoppingItemData shoppingItem)
        {
            gateway.Remove(shoppingItem);
        }
        // Save any changes to database by provided gateway
        public int SaveChanges()
        {
            return gateway.SaveChanges();
        }
        // Close connection and save changes
        public void Dispose()
        {
            gateway.Dispose();
        }
    }
}
