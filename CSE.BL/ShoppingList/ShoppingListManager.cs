using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CSE.BL.ShoppingList
{
    /*This class creates and holds a list of ShoppingItem objects and 
     * each object holds its own data and applicable methods.*/
    [Serializable]
    public class ShoppingListManager
    {
        private string name;
        public List<ShopTypes> UniqueShops { get; set; }

        //constructor for ShoppingListManager class
        public ShoppingListManager()
        {
            ShoppingList = new List<ShoppingItem>();
            UniqueShops = new List<ShopTypes>();
        }

        public ShoppingListManager(IEnumerable<ShoppingItem> collection) : base()
        {
            ShoppingList = new List<ShoppingItem>(collection);

            UpdateInformation();
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
                    if(item.Name.Equals(itm.Name) && item.Shop.Equals(itm.Shop))
                    {
                        itm.Amount += item.Amount;
                        UpdateInformation();

                        return true;
                    }
                }

                ShoppingList.Add(item);
                UpdateInformation();

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

                ShoppingList.Remove(item);
                UpdateInformation();

                ok = true;
            }

            return ok;
        }

        //method to remove an item from the list
        public bool RemoveItem(ShoppingItem item)
        {
            if (!ShoppingList.Contains(item)) return false;

            ShoppingList.Remove(item);

            UpdateInformation();

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
            UniqueShops = new List<ShopTypes>();

            foreach(ShoppingItem item in ShoppingList)
            {
                if (item.Shop == ShopTypes.UNKNOWN) continue;

                if (!UniqueShops.Contains(item.Shop)) UniqueShops.Add(item.Shop);
            }
        }

        public void UpdateInformation()
        {
            FindUniqueShops();
        }
    }
}
