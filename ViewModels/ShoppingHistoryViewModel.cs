using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using CSE.BL.ScannedData;

namespace ViewModels
{
    public class ShoppingHistoryViewModel : BaseViewModel
    {
        public string Total { get; set; }
        private DateTime selectedDate;

        public DateTime SelectedDate
        {
            get
            {
                return selectedDate;
            }
            set
            {
                selectedDate = value;
                ScannedLists = new ObservableCollection<ScannedListManager>(ScannedListLibrary.GetLists(SelectedDate));
                Total = "Total: " + ScannedListLibrary.TotalSum.ToString("C", CultureInfo.CreateSpecificCulture("fr-FR"));
                OnPropertyChanged(nameof(ScannedLists));
                OnPropertyChanged(nameof(Total));
            }
        }

        public ObservableCollection<ScannedListManager> ScannedLists { get; set; }

        public ShoppingHistoryViewModel()
        {
            new TempData();
            SelectedDate = DateTime.Now;
            ScannedLists = new ObservableCollection<ScannedListManager>(ScannedListLibrary.GetLists(SelectedDate));
            Total = "Total: " + ScannedListLibrary.TotalSum.ToString("C", CultureInfo.CreateSpecificCulture("fr-FR"));
        }
    }
}
