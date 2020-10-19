using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSE.BL.BillingData
{
    public static class ChartDataGenerator
    {
        //calculates the average of all the purchases ever made
        public static decimal OverallAverageSpending()
        {
            if (MonthSpendingLibrary.NumberOfShoppings > 0)
                return Math.Round(YearSpending().Sum() / MonthSpendingLibrary.NumberOfShoppings, 2, MidpointRounding.AwayFromZero);
            else
                return 0;
        }

        //calculates the average of given list
        public static decimal AverageSpending(List<decimal> list)
        {
            if (list.Count() > 0)
                return Math.Round(list.Sum() / list.Count(), 2, MidpointRounding.AwayFromZero);
            else
                return 0;
        }

        //calculates and gives data to the bar chart in BillingStatement class
        public static List<decimal> MonthSpending()
        {
            var monthIndex = DateTime.Now.AddMonths(-1).Month;
            List<decimal> list = new List<decimal>();
            list.Add(AverageSpending(MonthSpendingLibrary.GetMonthSpending(monthIndex)));
            list.Add(AverageSpending(MonthSpendingLibrary.GetMonthSpending(monthIndex+1)));

            return list;
        }

        //calculates and gives data to the line chart in BillingStatement class
        public static List<decimal> YearSpending()
        {
            List<decimal> list = new List<decimal>();
            var monthIndex = DateTime.Now.AddMonths(1).Month;            

            while(monthIndex < 13)
            {
                list.Add((MonthSpendingLibrary.GetMonthSpending(monthIndex)).Sum());
                monthIndex++;
            }
            monthIndex = 1;

            while(list.Count < 12)
            {
                list.Add((MonthSpendingLibrary.GetMonthSpending(monthIndex)).Sum());
                monthIndex++;
            }

            return list;
        }

        //calculates the difference between the average spending of current and previous month
        public static decimal Difference()
        {
            return MonthSpending().ElementAt(1) - MonthSpending().ElementAt(0);
        }
    }
}
