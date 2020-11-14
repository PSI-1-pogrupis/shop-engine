using CSE.BL.Database;
using CSE.BL.Interfaces;
using CSE.BL.ScannedData;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL
{
    public class ItemDataSelector
    {
        // Finds an item with the most similar name to a given string.
        public ShoppingItemData FindClosestItem(string name, List<ShoppingItemData> dataList)
        {
            string lowerName = name.ToLower();
            int minDist = int.MaxValue;
            ShoppingItemData closestData = null;

            foreach (ShoppingItemData data in dataList)
            {
                int distance = lowerName.DistanceTo(data.Name.ToLower());

                if (distance < minDist)
                {
                    minDist = distance;
                    closestData = data;
                }
            }

            if (minDist > name.Length * 0.25) return null;

            return closestData;
        }
    }
}
