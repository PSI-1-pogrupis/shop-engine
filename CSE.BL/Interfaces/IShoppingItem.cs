using CSE.BL.ShoppingList;

namespace CSE.BL.Interfaces
{
    public interface IShoppingItem
    {
        int? Id { get; set; }
        ShopTypes Shop { get; set; }
        string Name { get; set; }
        double Amount { get; set; }
        UnitTypes Unit { get; set; }
        double Price { get; set; }
    }
}
