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
        private ScannedListManager scannedListManager;
        public string Total { get; set; }
        private DateTime selectedDate;
        private CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("fr-FR");

        public DateTime SelectedDate
        {
            get
            {
                return selectedDate;
            }
            set
            {
                selectedDate = value;
                scannedListManager = ScannedListLibrary.GetList(SelectedDate.Date);
                ScannedList = new ObservableCollection<ScannedItem>(scannedListManager.ScannedItems);
                Total = "Total: " + scannedListManager.TotalSum.ToString("C", cultureInfo);
                OnPropertyChanged(nameof(ScannedList));
                OnPropertyChanged(nameof(Total));
            }
        }

        public ObservableCollection<ScannedItem> ScannedList{ get; set; }

        public ShoppingHistoryViewModel()
        {
            new TempData();
            SelectedDate = DateTime.Now;
            scannedListManager = ScannedListLibrary.GetList(SelectedDate.Date);            
            ScannedList = new ObservableCollection<ScannedItem>(scannedListManager.ScannedItems);
            Total = "Total: " + scannedListManager.TotalSum.ToString("C", cultureInfo);
        }
    }
}
