using CSE.BL.ShoppingList;

namespace CSE.BL.Interfaces
{
    public interface IShoppingItemFactory
    {
        ShoppingItem CreateInstance(ShoppingItem data);
        ShoppingItem CreateInstance(int id, ShopTypes shopType, string name, double amount, UnitTypes unit, double price);
        void UpdateInstance(ShoppingItem data, ShoppingItem instance);
    }
}
