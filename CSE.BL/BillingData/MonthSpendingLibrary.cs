using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.BillingData
{
    public static class MonthSpendingLibrary
    {
        private static int numberOfShoppings;
        private static decimal totalSpent;
        private static Dictionary<(int, int), MonthSpending> monthList = new Dictionary<(int, int), MonthSpending>();

        public static int NumberOfShoppings
        {
            get
            {
                return numberOfShoppings;
            }
            set
            {
                if (value > -1)
                    numberOfShoppings = value;
            }
        }

        public static decimal TotalSpent 
        { 
            get
            {
                return totalSpent;
            }
            set
            {
                if (value > -1)
                    totalSpent = value;
            }
        }

        public static List<decimal> GetMonthSpending(DateTime date)
        {
            if (!monthList.ContainsKey((date.Year, date.Month)))
                monthList[(date.Year, date.Month)] = new MonthSpending();

            return monthList[(date.Year, date.Month)].ShoppingLog;
        }

        public static void AddToLibrary(DateTime date, decimal value)
        {
            if(!monthList.ContainsKey((date.Year, date.Month)))
            {
                monthList.Add((date.Year, date.Month), new MonthSpending());

                if (monthList.ContainsKey(((date.Year - 1), date.Month)))
                {
                    monthList.Remove(((date.Year - 1), date.Month));
                }
            }            

            monthList[(date.Year, date.Month)].AddToShoppingLog(value);
            TotalSpent += value;
            numberOfShoppings++;      
        }
    }
}
