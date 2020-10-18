using CSE.BL.ShoppingList;

namespace CSE.BL.Interfaces
{
    public interface IShoppingItemFactory
    {
        IShoppingItem CreateInstance(IShoppingItem data);
        IShoppingItem CreateInstance(int id, ShopTypes shopType, string name, double amount, UnitTypes unit, double price);
        void UpdateInstance(IShoppingItem data, IShoppingItem instance);
    }
}
