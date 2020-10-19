using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.BillingData
{
    public class MonthSpending
    {
        private List<decimal> shoppingsLog;

        public MonthSpending()
        {
            shoppingsLog = new List<decimal>();
        }

        public List<decimal> ShoppingLog
        {
            get
            {
                return shoppingsLog;
            }
        }

        public void AddToShoppingLog(decimal amount)
        {
            shoppingsLog.Add(amount);
        }

        public void RemoveFromShoppingLog(int index)
        {
            if (index < shoppingsLog.Count)
            {
                shoppingsLog.RemoveAt(index);
            }
        }

        public void ClearLog()
        {
            shoppingsLog.Clear();
        }
    }
}
