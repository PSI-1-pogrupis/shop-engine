using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.BillingData
{
    public static class MonthSpendingLibrary
    {
        private static int numberOfShoppings;
        private static MonthSpending[] yearSpending = InitializeArray(13);

        public static MonthSpending[] YearSpending
        {
            get
            {
                return yearSpending;
            }
        }

        public static List<decimal> GetMonthSpending(int index)
        {
            return yearSpending[index].ShoppingLog;
        }

        public static int NumberOfShoppings
        {
            get
            {
                return numberOfShoppings;
            }
            set
            {
                if(value > -1)
                    numberOfShoppings = value;
            }
        }

        public static void AddToLibrary(int monthNumber, decimal value)
        {
            if (monthNumber > 0 && monthNumber < 13)
            {
                yearSpending[monthNumber].AddToShoppingLog(value);
                numberOfShoppings++;
            }
        }

        public static void RemoveFromLibrary(int monthNumber)
        {
            var length = yearSpending[monthNumber].ShoppingLog.Count;
            if (length > 0)
            {                
                yearSpending[monthNumber].RemoveFromShoppingLog(length-1);
                numberOfShoppings--;
            }
        }

        private static MonthSpending[] InitializeArray(int length)
        {
            MonthSpending[] array = new MonthSpending[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = new MonthSpending();
            }

            return array;
        }
    }
}
