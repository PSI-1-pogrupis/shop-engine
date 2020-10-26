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

        public BillingStatementViewModel()
        {
            if( !TemporaryData.Loaded)
            {
                TemporaryData.LoadData();
                TemporaryData.Loaded = true;
            }
                 
 
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

            AverageSpendings = (ChartDataGenerator.OverallAverageSpending()).ToString() + "€ per shopping";

            MonthDifference = ChartDataGenerator.Difference() > 0 ? ChartDataGenerator.Difference().ToString() + "€ more than last month" : Math.Abs(ChartDataGenerator.Difference()).ToString() + "€ less than last month";         
        }
    }
}
