﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CSE.BL.ScannedData
{
    [Serializable]
    public class ScannedItem
    {
        private string name;
        private decimal price;

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
        public decimal Price
        {
            get
            {
                return price;
            }
            set
            {
                if (value > 0)
                {
                    PriceString = value.ToString("C", CultureInfo.CreateSpecificCulture("fr-FR"));
                    price = value;
                }
            }
        }
        private decimal discount;
        public decimal Discount
        {
            get
            {
                return discount;
            }
            set
            {
                DiscountString = value.ToString("C", CultureInfo.CreateSpecificCulture("fr-FR"));
                discount = value;
            }
        }
        public string PriceString { get; set; }
        public string DiscountString { get; set; }

        public ShopTypes Shop { get; set; }

        public ScannedItem BetterPricedItem { get; set; }

        public ScannedItem (string name, decimal price, ShopTypes shop)
        {
            Name = name;
            Price = price;
            Shop = shop;
        }

        public ScannedItem()
        {
        }

    }
}
