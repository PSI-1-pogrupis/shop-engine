using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.ShoppingList
{
    /*This class creates and holds a list of ShoppingItem objects and 
     * each object holds its own data and applicable methods.*/
    [Serializable]
    public class ShoppingListManager
    {
        private string name;
        public List<string> UniqueShops { get; set; }
        public double EstimatedPrice { get; private set; }

        //constructor for ShoppingListManager class
        public ShoppingListManager()
        {
            ShoppingList = new List<ShoppingItem>();
            UniqueShops = new List<string>();
        }

        public ShoppingListManager(IEnumerable<ShoppingItem> collection) : base()
        {
            ShoppingList = new List<ShoppingItem>(collection);
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    name = value;
            }
        }

        public List<ShoppingItem> ShoppingList { get; set; }

        //method that gets the item from the shopping list at the given index
        public ShoppingItem GetItem(int index)
        {
            if (!CheckIndex(index))
                return null;

            return ShoppingList[index];
        }

        public int Count() { return ShoppingList.Count; }

        //method that adds given item to the list and returns value 'true' if the input was correct
        public bool AddItem(ShoppingItem item)
        {
            bool ok = false;

            if (item != null)
            {
                foreach(ShoppingItem itm in ShoppingList)
                {
                    if(item.Name.Equals(itm.Name) && item.SelectedShop.Equals(itm.SelectedShop))
                    {
                        itm.Amount += item.Amount;
                        EstimatedPrice += item.Price;

                        return true;
                    }
                }

                EstimatedPrice += item.Price;

                ShoppingList.Add(item);
                FindUniqueShops();
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
                ShoppingList[index].Name = item.Name;
                ShoppingList[index].Amount = item.Amount;
                ShoppingList[index].Unit = item.Unit;

                FindUniqueShops();
                ok = true;
            }

            return ok;
        }

        //method to delete item from the list at given index. Returns 'true' if index was valid
        public bool RemoveItem(int index)
        {
            bool ok = false;

            if (CheckIndex(index))
            {
                ShoppingItem item = ShoppingList[index];

                EstimatedPrice -= item.Price;

                ShoppingList.Remove(item);
                FindUniqueShops();
                ok = true;
            }

            return ok;
        }

        //method to remove an item from the list
        public bool RemoveItem(ShoppingItem item)
        {
            if (!ShoppingList.Contains(item)) return false;

            ShoppingList.Remove(item);

            EstimatedPrice -= item.Price;
            FindUniqueShops();

            return true;
        } 

        //method to check if the given index is valid
        private bool CheckIndex(int index)
        {
            bool ok = false;

            if ((index >= 0) && index < ShoppingList.Count)
            {
                ok = true;
            }

            return ok;
        }

        private void FindUniqueShops()
        {
            UniqueShops = new List<string>();

            foreach(ShoppingItem item in ShoppingList)
            {
                if (!UniqueShops.Contains(item.SelectedShop.Item1)) UniqueShops.Add(item.SelectedShop.Item1);
            }
        }

    }
}
