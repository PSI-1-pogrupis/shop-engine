using CSE.BL.ScannedData;
using DuoVia.FuzzyStrings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CSE.BL
{
    public class ItemsScanner
    {
        //public ShopTypes GetShop(string text)
        //{
        //    string[] tLines = text.Split('\n');
        //    foreach(ShopTypes shop in (ShopTypes[])Enum.GetValues(typeof(ShopTypes)))
        //    {
        //        // I'm assuming the shop title should be in the first 5 lines of the check
        //        try
        //        {
        //            for (int i = 0; i < 4; i++)
        //            {
        //                if (tLines[i].ContainsIgnCase(shop.ToString())) return shop;
        //            }
        //        }catch(IndexOutOfRangeException)
        //        {
        //            return ShopTypes.UNKNOWN;
        //        }
        //    }
        //    return ShopTypes.UNKNOWN;
        //}

        //TODO: method to scan all shopping items

        private const int minLineLength = 5;

        public void ScanProducts(ScannedListManager scannedList, string text)
        {
            string[] tLines = text.Split('\n');
            string productName;
            decimal productPrice;
            string newline;
            decimal discount;
            decimal pricePerQuantity = 0;
            ScannedItem tempScannedItem = null;

            foreach(string line in tLines)
            {
                if (line.SymbolsCount() < minLineLength) continue;

                newline = line.ToUpper();
                newline = newline.Replace(',', '.');
                newline = newline.Replace('\'', '.');
                newline = newline.Replace('”', '-');
                newline = newline.Replace('„', '-');
                newline = newline.Replace('“', '-');
                newline = newline.Replace('"', '-');
                newline = newline.Replace('—', '-');


                discount = Discount(newline);
                if (discount != 0)
                {
                    if (scannedList.GetCount() > 0)
                    {
                        if (scannedList.GetItem(scannedList.GetCount() - 1).Discount == 0)
                        {
                            scannedList.GetItem(scannedList.GetCount() - 1).Discount = discount;
                        }
                        continue;
                    }
                    
                }

                
                if (tempScannedItem != null)
                {
                    // check if its "0,225 kg x 4,35 EUR / kg 1,20 A" or "VNT..." type of line
                    productPrice = QuantitiesAndPricesLine(newline, out pricePerQuantity);
                    if (productPrice != 0)
                    {
                        tempScannedItem.Price = productPrice;
                        tempScannedItem.PricePerQuantity = pricePerQuantity;
                        scannedList.AddItem(tempScannedItem);
                        tempScannedItem = null;
                        continue;
                    }
                }

                


                if (ReadProduct(newline, out productName, out productPrice))
                    scannedList.AddItem(new ScannedItem(productName, productPrice));
                else
                    tempScannedItem = new ScannedItem(productName, 0);

            }
        }

        private decimal QuantitiesAndPricesLine(string newline, out decimal pricePerQuantity)
        {
            char c;
            pricePerQuantity = 0;

            int pos = newline.IndexOf("EUR");
            if(pos > 7)
            {
                for (int i = pos-7; i < pos; i++)
                {
                    c = newline[i];
                    if (char.IsDigit(c))
                    {
                        pricePerQuantity = ParseDecimal(newline, i);
                    }
                }
            }

            if((newline.Contains("EUR") && (newline.Contains("X") || newline.Contains(")("))) || newline.Contains("VNT")){
                for (int i = newline.Length - 10; i < newline.Length; i++)  // so i would read only the last number (price)
                {
                    c = newline[i];
                    if (char.IsDigit(c))
                    {
                        return ParseDecimal(newline, i);
                    }
                }
            }
            return 0;
        }

        private decimal Discount(string line)
        {
            //if (line.ContainsSimilar("NUOL", 1)) -- bad cuz sometimes confuzes products with discounts
            if(line.Contains("NUOL"))
            {
                return ParseDecimal(line) * -1;
            }
            return 0;
        }

        private bool ReadProduct(string line, out string productName, out decimal productPrice)
        {
            productName = string.Empty;
            productPrice = default;
            char c;

            for (int i = 0; i < line.Length; i++)
            {
                c = line[i];

                if (!char.IsDigit(c) || line.Length - i > 8)
                    productName += c;
                else
                {
                    productPrice = ParseDecimal(line, i);

                    if (productPrice != 0)
                    {
                        if (productName.ContainsSimilar("UŽST", 1) && productPrice == 0.1m) return false; // deposit bottle
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        private decimal ParseDecimal(string line, int pos) // TODO: if dot isnt in the number, it parsed wrong thus return 0
        {
            string numberString = string.Empty;
            char c;

            for(int i = pos; i < line.Length; i++)
            {
                c = line[i];

                if (!char.IsDigit(c) && c != '.')
                {
                    break;
                }

                numberString += c;
            }


            if(numberString.Length > 3) //more than min possible readed price 
            {
                try
                {
                    return decimal.Parse(numberString);
                }
                catch (System.FormatException)
                {
                    return 0;
                }
            }

            return 0;
        }

        private decimal ParseDecimal(string line)   // TODO: if dot isnt in the number, it parsed wrong thus return 0
        {
            bool foundNumber = false;
            string numberString = string.Empty;
            char c;

            for (int i = 0; i < line.Length; i++)
            {
                c = line[i];
                if (foundNumber)
                {
                    if (char.IsDigit(c) || c == '.')
                        numberString += c;
                    else break;
                }
                else
                {
                    if (char.IsDigit(c) && (line[i+1] == '.' || char.IsDigit(line[i+1])))
                    {
                        foundNumber = true;
                        numberString += c;
                    }
                }
            }

            if (numberString.Length > 3) //more than min possible readed price 
            {
                try
                {
                    return decimal.Parse(numberString);
                }
                catch (System.FormatException)
                {
                    return 0;
                }
                
            }

            return 0;
        }
    }

}
