using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CSE.BL
{
    public class ItemsScanner
    {
        public ShopTypes GetShop(string text)
        {
            string[] tLines = text.Split('\n');
            foreach(ShopTypes shop in (ShopTypes[])Enum.GetValues(typeof(ShopTypes)))
            {
                // I'm assuming the shop title should be in the first 5 lines of the check
                try
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (tLines[i].ContainsIgnCase(shop.ToString())) return shop;
                    }
                }catch(IndexOutOfRangeException e)
                {
                    return ShopTypes.UNKNOWN;
                }
            }
            return ShopTypes.UNKNOWN;
        }

        //TODO: method to scan all shopping items
    }

    public static class ContainsExtension
    {
        public static bool ContainsIgnCase(this string source, string toCheck)
        {
            return source?.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
