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

        private const int minLineLength = 10;

        public void ScanProducts(ScannedListManager scannedList, string text)
        {
            string[] tLines = text.Split('\n');
            string productName;
            decimal productPrice;
            string newline;
            decimal discount;

            foreach(string line in tLines)
            {
                if (line.LettersCount() < minLineLength) continue;

                newline = line.ToUpper();
                newline = newline.Replace(',', '.');


                discount = Discount(newline);
                if (discount != 0)
                {
                    if (scannedList.GetCount() > 0)
                    {
                        scannedList.GetItem(scannedList.GetCount() - 1).Discount = discount;
                        continue;
                    }
                    
                }

                if (ReadProduct(newline, out productName, out productPrice))
                    scannedList.AddItem(new ScannedItem(productName, productPrice));
                    

            }
        }

        private decimal Discount(string line)
        {
            if (line.ContainsSimilar("NUOLAIDA", 1))
            {
                return ParseDecimal(line) * -1;
            }
            return 0;
        }

        private bool ReadProduct(string line, out string productName, out decimal productPrice)
        {
            productName = String.Empty;
            productPrice = default;
            char c;

            for (int i = 0; i < line.Length; i++)
            {
                c = line[i];

                if (!Char.IsDigit(c))
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

                if (!Char.IsDigit(c) && c != '.')
                {
                    break;
                }

                numberString += c;
            }


            if(numberString.Length > 3) //more than min possible readed price 
            {
                return decimal.Parse(numberString);
            }

            return 0;
        }

        private decimal ParseDecimal(string line)   // TODO: if dot isnt in the number, it parsed wrong thus return 0
        {
            bool foundNumber = false;
            string numberString = String.Empty;
            char c;

            for (int i = 0; i < line.Length; i++)
            {
                c = line[i];
                if (foundNumber)
                {
                    if (Char.IsDigit(c) || c == '.')
                        numberString += c;
                    else break;
                }
                else
                {
                    if (Char.IsDigit(c) && (line[i+1] == '.' || Char.IsDigit(line[i+1])))
                    {
                        foundNumber = true;
                        numberString += c;
                    }
                }
            }

            if (numberString.Length > 3) //more than min possible readed price 
            {
                return decimal.Parse(numberString);
            }

            return 0;
        }
    }

    
}
