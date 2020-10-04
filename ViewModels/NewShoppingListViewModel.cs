using CSE.BL.ShoppingList;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels
{
    public class NewShoppingListViewModel : BaseViewModel
    {
        private readonly List<ShoppingItem> productList;
        private readonly List<ShoppingItem> shoppingList;

        public List<ShoppingItem> ShoppingList
        {
            get { return shoppingList; }
        }

        public List<ShoppingItem> ProductList
        {
            get { return productList; }
        }

        public NewShoppingListViewModel()
        {
            productList = new List<ShoppingItem>();

            for (int i = 0; i < 20; i++)
            {
                productList.Add(new ShoppingItem("Item " + i.ToString(), 1, UnitTypes.kg));
            }
        }
    }
}
