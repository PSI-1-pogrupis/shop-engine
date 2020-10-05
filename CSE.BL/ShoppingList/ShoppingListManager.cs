using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.ShoppingList
{
    /*This class creates and holds a list of ShoppingItem objects and 
     * each object holds its own data and applicable methods.*/
    [Serializable]
    public class ShoppingListManager
    {
        private List<ShoppingItem> shoppingList;

        //constructor for ShoppingListManager class
        public ShoppingListManager()
        {
            shoppingList = new List<ShoppingItem>();
        }

        //method that gets the item from the shopping list at the given index
        public ShoppingItem GetItem(int index)
        {
            if (!CheckIndex(index))
                return null;

            return shoppingList[index];
        }

        //method that returns an item list
        public List<ShoppingItem> GetItemList() { return shoppingList; }

        //method that adds given item to the list and returns value 'true' if the input was correct
        public bool AddItem(ShoppingItem item)
        {
            bool ok = false;

            if (item != null)
            {
                shoppingList.Add(item);
                ok = true;
            }

            return ok;
        }

        //method to change the list item at given index. Returns 'true' if the index was valic
        public bool ChangeItem(ShoppingItem item, int index)
        {
            bool ok = false;
            if (CheckIndex(index))
            {
                shoppingList[index].Name = item.Name;
                shoppingList[index].Amount = item.Amount;
                shoppingList[index].Unit = item.Unit;
                ok = true;
            }

            return ok;
        }

        //method to delete item from the list at given index. Returns 'true' if index was valid
        public bool DeleteItem(int index)
        {
            bool ok = false;

            if (CheckIndex(index))
            {
                shoppingList.RemoveAt(index);
                ok = true;
            }

            return ok;
        }

        //method to check if the given index is valid
        private bool CheckIndex(int index)
        {
            bool ok = false;

            if ((index >= 0) && index < shoppingList.Count)
            {
                ok = true;
            }

            return ok;
        }

    }
}
