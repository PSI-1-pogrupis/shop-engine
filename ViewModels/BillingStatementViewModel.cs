using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using CSE.BL;
using System.Windows.Controls;
using CSE.BL.BillingData;
using System.Globalization;

namespace ViewModels
{
    public class BillingStatementViewModel : BaseViewModel
    {
        public SeriesCollection SeriesCollection1 { get; set; }
        public SeriesCollection SeriesCollection2 { get; set; }
        public List<string> LatestMonths { get; set; }
        public List<string> AllMonths { get; set; }
        public string AverageSpendings { get; set; }
        public string MonthDifference { get; set; }
        public Func<double, string> Formatter { get; set; }

        public BillingStatementViewModel()
        {
            //if( !TemporaryData.Loaded)
            //{
            //    TemporaryData.LoadData();
            //    TemporaryData.Loaded = true;
            //}            

            LatestMonths = MonthGenerator.GetListOfLatestMonths();
            AllMonths = MonthGenerator.GetListOfAllMonths();

            SeriesCollection1 = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Month Spendings",
                        Values = new ChartValues<decimal>(ChartDataGenerator.MonthSpending()),
                        Fill = Brushes.Brown,
                        DataLabels = true
                    }
                };

            SeriesCollection2 = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Year Spendings",
                    Values = new ChartValues<decimal>(ChartDataGenerator.YearSpending()),
                    Stroke = Brushes.Brown,
                    Fill = Brushes.Transparent,
                    StrokeThickness = 7,
                    DataLabels = true
                }
            };

            var cultureInfo = CultureInfo.CreateSpecificCulture("fr-FR");

            Formatter = value => value.ToString("F", cultureInfo);

            AverageSpendings = (ChartDataGenerator.OverallAverageSpending()).ToString("C", cultureInfo) + " per shopping";

            MonthDifference = ChartDataGenerator.Difference() > 0 ? ChartDataGenerator.Difference().ToString("C", cultureInfo) + " more than last month" : Math.Abs(ChartDataGenerator.Difference()).ToString("C", cultureInfo) + " less than last month";

        }
    }
}
