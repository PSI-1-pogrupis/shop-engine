﻿using System;
using System.Collections.Generic;

namespace CSE.BL.ShoppingList
{
    /*ShoppingItem object holds data to describe 
     * id, shopType name, measurement unit type, amount and price.*/
    [Serializable]
    public class ShoppingItem
    {
        public Dictionary<string, double> dictionary;//dictionary for saving same item prices over different shops
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

        public string SelectedShopName
        {
            get { return SelectedShop.Item1; }
        }

        public double Price
        {
            get { return Amount * SelectedShop.Item2; }
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
            dictionary.Add(shop.ToString(), price);
            this.id = id;
            this.shop = shop;
            this.name = name;
            this.amount = amount;
            this.unit = unit;
            this.price = price;
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
            dictionary.Add(shop.ToString(), price);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
