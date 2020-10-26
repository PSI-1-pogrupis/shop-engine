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
                int distance = DistanceToString(lowerName, data.Name.ToLower());

                if (distance < minDist)
                {
                    minDist = distance;
                    closestData = data;
                }
            }

            if (minDist > name.Length * 0.25) return null;

            return closestData;
        }

        // Levenshtein distance algorithm. Finds the distance (number of single character edits required to transform one string into another) between two strings.
        private int DistanceToString(string str, string other)
        {
            int sourceLength = str.Length;
            int otherLength = other.Length;
            int[,] distance = new int[sourceLength + 1, otherLength + 1];

            if (sourceLength == 0)
            {
                return otherLength;
            }

            if (otherLength == 0)
            {
                return sourceLength;
            }

            for (int i = 0; i <= sourceLength; distance[i, 0] = i++) { };

            for (int j = 0; j <= otherLength; distance[0, j] = j++) { };

            for (int i = 1; i <= sourceLength; i++)
            {
                for (int j = 1; j <= otherLength; j++)
                {
                    int cost = (other[j - 1] == str[i - 1]) ? 0 : 1;

                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceLength, otherLength];
        }
    }
}
