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

        public static int LettersCount(this string source)
        {
            int count = 0;
            foreach (char c in source)
                if (Char.IsLetter(c)) count++;

            return count;
        }
    }
}
