using DuoVia.FuzzyStrings;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL
{
    public static class StringExtensions
    {
        //public static bool ContainsIgnCase(this string source, string toCheck)
        //{
        //    return source?.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        //}

        public static bool ContainsSimilar(this string source, string toCheck, int tolerance)
        {
            bool subsequenceTolerated = source.LongestCommonSubsequence(toCheck).Item1.Length >= toCheck.Length - tolerance;

            return subsequenceTolerated;
        }

        public static int SymbolsCount(this string source)
        {
            int count = 0;
            foreach (char c in source)
                if (!char.IsDigit(c) && !char.IsWhiteSpace(c)) count++;

            return count;
        }

        /*
         * Levenshtein distance algorithm.
         * Finds the distance (number of single character edits required to transform one string into another) between two strings.
         */
        public static int DistanceTo(this string source, string other)
        {
            int sourceLength = source.Length;
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
                    int cost = (other[j - 1] == source[i - 1]) ? 0 : 1;

                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceLength, otherLength];
        }
    }
}
