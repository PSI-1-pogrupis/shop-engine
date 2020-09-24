using System;
using System.Collections.Generic;
using System.Text;

namespace ComparisonShoppingEngine
{
    /*ShoppingItem object holds data to describe 
     the name, amount, and measurement unit type*/
    class ShoppingItem
    {
        private string name; //name of the item
        private double amount; //amount of the item 
        private UnitTypes unit; //the unit of measurement for this item

        // get/set the name of the item
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

        // get/set the amount of the item
        public double Amount
        {
            get
            {
                return amount;
            }
            set
            {
                if (value > 0)
                {
                    amount = value;
                }
            }
        }

        // get/set the measurement unit for the item
        public UnitTypes Unit
        {
            get
            {
                return unit;
            }
            set
            {
                if (Enum.IsDefined(typeof(UnitTypes), value))
                    unit = value;
            }
        }

    }
}
