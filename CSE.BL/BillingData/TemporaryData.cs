using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.BillingData
{
    //temporary class to provide data for charts
    public class TemporaryData
    {
        public TemporaryData()
        {
            //this way the same values are not added to the library every time the window is opened and NumberOfShoppings isn't increased
            for (int i = 1; i < 13; i++)
            {
                MonthSpendingLibrary.NumberOfShoppings = 0;
                MonthSpendingLibrary.YearSpending[i].ClearLog();
            }                

            for(int i = 1; i < 13; i+=3)
            {
                MonthSpendingLibrary.AddToLibrary(i, 12.4m);
                MonthSpendingLibrary.AddToLibrary(i, 25.67m);
                MonthSpendingLibrary.AddToLibrary(i, 16.96m);

                MonthSpendingLibrary.AddToLibrary(i+1, 26.35m);
                MonthSpendingLibrary.AddToLibrary(i+1, 15.19m);
                MonthSpendingLibrary.AddToLibrary(i+1, 20.43m);

                MonthSpendingLibrary.AddToLibrary(i + 2, 41m);
                MonthSpendingLibrary.AddToLibrary(i + 2, 23.78m);
                MonthSpendingLibrary.AddToLibrary(i + 2, 16.47m);
            }
        }
    }
}
