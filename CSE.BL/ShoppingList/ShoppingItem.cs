using CSE.BL.Database;
using System;
using System.Collections.Generic;

namespace CSE.BL.ShoppingList
{
    /*ShoppingItem object holds data to describe 
     * id, shopType name, measurement unit type, amount and price.*/
    [Serializable]
    public class ShoppingItem
    {
        private string name; //name of the item
        private UnitTypes unit; //the unit of measurement for this item
        private double amount; //amount of the item 

        public ShopTypes Shop { get; private set; } // shop where this item is sold
        public decimal Price { get; private set; } // price of the item

        // get/set the name of the item
        public string Name
        {
            get { return name; }
            private set
            {
                if (!string.IsNullOrEmpty(value)) name = value;
            }
        }

        // get/set the amount of the item
        public double Amount
        {
            get { return amount; }
            set
            {
                if (value > 0) amount = value;
            }
        }

        // get/set the measurement unit for the item
        public UnitTypes Unit
        {
            get { return unit; }
            private set
            {
                if (Enum.IsDefined(typeof(UnitTypes), value)) unit = value;
            }
        }

        public ShoppingItem() { }

        //constructor for the ShoppingItem class
        public ShoppingItem(string name, ShopTypes shop, decimal price, double amount, UnitTypes unit)
        {
            this.name = name;
            this.amount = amount;
            this.unit = unit;

            Shop = shop;
            Price = price;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
