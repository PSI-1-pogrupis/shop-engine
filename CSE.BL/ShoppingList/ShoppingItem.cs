using CSE.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.ShoppingList
{
    /*ShoppingItem object holds data to describe 
     * the name, amount, and measurement unit type*/

    [Serializable]
    public class ShoppingItem : IShoppingItem
    {
        private int? id;//item id
        private ShopTypes shop; //item shop place
        private string name; //name of the item
        private UnitTypes unit; //the unit of measurement for this item
        private double amount; //amount of the item 
        private double price; //price of the item

        public int? Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        // get/set index of shop
        public ShopTypes Shop
        {
            get
            {
                return shop;
            }
            set
            {
                shop = value;
            }
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

        // get/set the price of the item
        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                if (value > 0)
                {
                    price = value;
                }
            }
        }

        public ShoppingItem()
        {
        }

        //constructor for the ShoppingItem class
        public ShoppingItem(int? id, ShopTypes shop, string name, double amount, UnitTypes unit, double price)
        {
            this.id = id;
            this.shop = shop;
            this.name = name;
            this.amount = amount;
            this.unit = unit;
            this.price = price;
        }

        public ShoppingItem(IShoppingItem item)
        {
            if (item != null)
            {
                id = item.Id;
                shop = item.Shop;
                name = item.Name;
                amount = item.Amount;
                unit = item.Unit;
                price = item.Price;
            }
        }

        public ShoppingItem(ShoppingItem item)
        {
            if (item != null)
            {
                id = item.id;
                shop = item.shop;
                name = item.name;
                amount = item.amount;
                unit = item.unit;
                price = item.price;
            }
        }

        public override string ToString()
        {
            return name;
        }
    }
}
