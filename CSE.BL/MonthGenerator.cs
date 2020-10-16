using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CSE.BL
{
    public static class MonthGenerator
    {
        public static List<string> GetListOfAllMonths()
        {
            List<string> MonthList = new List<string>();
            DateTimeFormatInfo dtFI = new DateTimeFormatInfo();
            DateTime currentDate = DateTime.Now;
            DateTime lastYearDate = currentDate.AddYears(-1).AddMonths(1);
            while (lastYearDate <= currentDate)
            {
                MonthList.Add(dtFI.GetMonthName(lastYearDate.Month));
                lastYearDate = lastYearDate.AddMonths(1);
            }

            return MonthList;
        }

        public static List<string> GetListOfLatestMonths()
        {
            List<string> MonthList = new List<string>();
            DateTimeFormatInfo dtFI = new DateTimeFormatInfo();
            DateTime currentDate = DateTime.Now.AddMonths(-1);
            for (int i = 0; i < 2; i++)
            {
                MonthList.Add(dtFI.GetMonthName(currentDate.Month));
                currentDate = currentDate.AddMonths(1);
            }

            return MonthList;
        }
    }
}
