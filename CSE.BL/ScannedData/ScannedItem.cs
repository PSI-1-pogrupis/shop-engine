using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CSE.BL.ScannedData
{
    [Serializable]
    public class ScannedItem
    {
        private string _name;
        private decimal _price;

        public string Name 
        {
            get
            {
                return _name;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _name = value;
            }
        }
        public decimal Price
        {
            get
            {
                return _price;
            }
            set
            {
                if (value > 0)
                {
                    //PriceString = value.ToString("C", CultureInfo.CreateSpecificCulture("fr-FR"));
                    _price = value;
                }
            }
        }
        private decimal _discount;
        public decimal Discount
        {
            get
            {
                return _discount;
            }
            set
            {
                _discount = value;
            }
        }
        public string PriceString {
            get
            {
                if (Price > 0) return Price.ToString("C", CultureInfo.CreateSpecificCulture("fr-FR"));
                else return "";
            }
        }
        public string DiscountString
        {
            get
            {
                if (Discount != 0) return Discount.ToString("C", CultureInfo.CreateSpecificCulture("fr-FR"));
                else return "";
            }
        }

        // for products that are priced per quantity (kg, unit, litres etc..) if not, it's 0
        public decimal PricePerQuantity { get; set; } 
      
        public ShopTypes Shop { get; set; }

        public ScannedItem BetterPricedItem { get; set; }

        public ScannedItem (string name, decimal price, ShopTypes shop)
        {
            Name = name;
            Price = price;
            Shop = shop;
        }

        public ScannedItem(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public ScannedItem()
        {
        }

    }
}
