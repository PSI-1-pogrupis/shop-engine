﻿using ComparisonShoppingEngineAPI.Data.Utilities;
using ComparisonShoppingEngineAPI.DTOs;
using DuoVia.FuzzyStrings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Services.OCRService
{
    public class ItemsScanner : IItemScannerService
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

        public string Shop { get; set; } = "MAXIMA";
       
        public async Task<List<ScannedItemDto>> ScanProductsAsync(string text)
        {
            return await Task.Run(() => ScanProducts(text));
        }

        public List<ScannedItemDto> ScanProducts(string text)
        {
            string[] tLines = text.Split('\n');
            string productName;
            decimal productPrice;
            string newline;
            decimal discount;
            decimal pricePerQuantity = 0;
            ScannedItemDto tempScannedItem = null;

            List<ScannedItemDto> scannedList = new List<ScannedItemDto>();

            foreach (string line in tLines)
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
                    if (scannedList.Count > 0)
                    {
                        if (Math.Abs(discount) > scannedList[^1].Price) continue;
                        if (scannedList[^1].Discount == 0)
                        {
                            scannedList[^1].Discount = discount;
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
                        tempScannedItem.Shop = Shop;
                        Debug.WriteLine(Shop);
                        scannedList.Add(tempScannedItem);
                        tempScannedItem = null;
                        continue;
                    }
                }

                if (ReadProduct(newline, out productName, out productPrice))
                {
                    scannedList.Add(new ScannedItemDto() { Name = productName, Price = productPrice, Shop = Shop });
                }
                else
                {
                    tempScannedItem = new ScannedItemDto() { Name = productName, Price = 0 };
                }

            }

            return scannedList;
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
                        break;
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

        public void SeedShops(List<ScannedItemDto> scannedList, string shop)
        {
            foreach(ScannedItemDto product in scannedList)
            {
                product.Shop = shop;
            }
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
                catch (FormatException)
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
                    if (char.IsDigit(c) && (line[i + 1] == '.' || char.IsDigit(line[i + 1])))
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
                catch (FormatException)
                {
                    return 0;
                }
                
            }

            return 0;
        }
    }


}
