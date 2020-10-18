using CSE.BL.Interfaces;
using CSE.BL.ShoppingList;

namespace CSE.BL.Database
{
    /* Defines a way of creating and updating object.
     * Factory knows how to take all the nformation
     * needed to build an object and return ready-to-use object.*/
    public class ShoppingItemFactory : IShoppingItemFactory
    {
        // Create new ready-to-use object
        public IShoppingItem CreateInstance(IShoppingItem data)
        {
            if (data == null)
                return null;

            IShoppingItem instance = new ShoppingItem(data);

            return instance;
        }
        // Create new ready-to-use object using values
        public IShoppingItem CreateInstance(int id, ShopTypes shopType, string name, double amount, UnitTypes unit, double price)
        {
            IShoppingItem instance = new ShoppingItem(id, shopType, name, amount, unit, price);

            return instance;
        }

        // Update passed object
        public void UpdateInstance(IShoppingItem data, IShoppingItem instance)
        {
            instance.Id = data.Id;
            instance.Name = data.Name;
            instance.Unit = data.Unit;
            instance.Price = data.Price;
            instance.Amount = data.Amount;
        }
    }
}
