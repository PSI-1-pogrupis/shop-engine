using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using CSE.BL.ScannedData;

namespace ViewModels
{
    public class ShoppingHistoryViewModel : BaseViewModel
    {
        private ScannedListManager scannedListManager;
        public string TotalSum { get; set; }
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
                scannedListManager = ScannedListLibrary.GetList(SelectedDate.Date);
                ScannedList = new ObservableCollection<ScannedItem>(scannedListManager.ScannedItems);
                TotalSum = "Suma: " + scannedListManager.TotalSum.ToString();
                OnPropertyChanged(nameof(ScannedList));
                OnPropertyChanged(nameof(TotalSum));
            }
        }

        public ObservableCollection<ScannedItem> ScannedList{ get; set; }

        public ShoppingHistoryViewModel()
        {
            new TempData();
            SelectedDate = DateTime.Now;
            scannedListManager = ScannedListLibrary.GetList(SelectedDate.Date);            
            ScannedList = new ObservableCollection<ScannedItem>(scannedListManager.ScannedItems);
            TotalSum = "Suma: " + scannedListManager.TotalSum.ToString();
        }
    }
}
