using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace ViewModels
{
    public class BillingStatementViewModel : BaseViewModel
    {
        public SeriesCollection SeriesCollection1 { get; set; }
        public SeriesCollection SeriesCollection2 { get; set; }
        public string[] Labels1 { get; set; }
        public string[] Labels2 { get; set; }
        public string AverageSpendings { get; set; }
        public string MonthDifference { get; set; }

        public BillingStatementViewModel()
        {

            SeriesCollection1 = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Month Spendings",
                        Values = new ChartValues<double>{18.78, 23.16},
                        Fill = Brushes.Brown,
                        DataLabels = true
                    }
                };

            Labels1 = new[] {"September", "October"};
            Labels2 = new[] { "August", "September", "October" };

            SeriesCollection2 = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Year Spendings",
                    Values = new ChartValues<double>{ 317, 273, 368 },
                    Stroke = Brushes.Brown,
                    Fill = Brushes.Transparent,
                    StrokeThickness = 7,
                    DataLabels = true          
                }
            };

            double[] array  ={21.87, 18.78, 23.16 };

            AverageSpendings = Math.Round(array.Sum() / array.Length, 2, MidpointRounding.AwayFromZero).ToString() + "€ per shopping";

            double difference = Math.Round(array[2] - array[1], 2, MidpointRounding.AwayFromZero);

            MonthDifference = difference > 0 ? difference.ToString() + "€ more than last month" : Math.Abs(difference).ToString() + "€ less than last month";
        }
    }
}
