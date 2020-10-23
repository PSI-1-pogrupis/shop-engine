using CSE.BL.ScannedData;
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

        private const int minLineLength = 20;

        public void ScanProducts(ScannedListManager scannedList, string text)
        {
            string[] tLines = text.Split('\n');
            string productName;
            float productPrice;
            string newline;

            foreach(string line in tLines)
            {
                if (line.Length < minLineLength) continue;

                newline = line.ToUpper();
                newline = newline.Replace(',', '.');

                Debug.WriteLine(newline);

                // CheckNuolaida()

                if (ReadProduct(newline, out productName, out productPrice))
                    scannedList.AddItem(new ScannedItem(productName, productPrice));
                    

            }
        }


        private bool ReadProduct(string line, out string productName, out float productPrice)
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
                    productPrice = ParseNumber(line, i);

                    if (productPrice != 0) return true;
                    return false;
                }
            }
            return false;
        }

        private float ParseNumber(string line, int pos)
        {
            string numberString = string.Empty;
            char c;

            for(int i = pos; i < line.Length; i++)
            {
                c = line[i];

                if (!Char.IsDigit(c) && c != ',' && c != '.')
                {
                    break;
                }

                numberString += c;
            }


            if(numberString.Length > 3) //more than min possible readed price 
            {
                return float.Parse(numberString);
            }

            return 0;
        }
    }

    public static class ContainsExtension
    {
        public static bool ContainsIgnCase(this string source, string toCheck)
        {
            return source?.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
