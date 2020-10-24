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

        public Dictionary<ShopTypes, double> ShopPrices { get; set; } //dictionary for saving same item prices over different shops
        public KeyValuePair<ShopTypes, double> SelectedShop { get; set; }

        public ShopTypes SelectedShopName
        {
            get { return SelectedShop.Key; }
        }

        public double Price
        {
            get { return Amount * SelectedShop.Value; }
        }

        // get/set the name of the item
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (!string.IsNullOrEmpty(value)) name = value;
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
                if (Enum.IsDefined(typeof(UnitTypes), value)) unit = value;
            }
        }

        public ShoppingItem()
        {
        }

        //constructor for the ShoppingItem class
        public ShoppingItem(string name, double amount, UnitTypes unit, Dictionary<ShopTypes, double> prices)
        {
            ShopPrices = prices;
            this.name = name;
            this.amount = amount;
            this.unit = unit;
        }

        public ShoppingItem(ShoppingItem item)
        {
            if (item != null)
            {
                name = item.name;
                amount = item.amount;
                unit = item.unit;
                ShopPrices = item.ShopPrices;
            }

        }

        public override string ToString()
        {
            return name;
        }
    }
}
