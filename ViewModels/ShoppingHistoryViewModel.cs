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
        public ScannedListManager ScannedListManager { get; set; }
        public string Title { get; set; }
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
                ScannedListManager = ScannedListLibrary.GetList(SelectedDate.Date);
                ScannedList = new ObservableCollection<ScannedItem>(ScannedListManager.ScannedItems);
                OnPropertyChanged(nameof(ScannedList));
                OnPropertyChanged(nameof(Title));
            }
        }

        public ObservableCollection<ScannedItem> ScannedList{ get; set; }

        public ShoppingHistoryViewModel()
        {
            new TempData();
            SelectedDate = DateTime.Now;
            ScannedListManager = ScannedListLibrary.GetList(SelectedDate.Date);            
            ScannedList = new ObservableCollection<ScannedItem>(ScannedListManager.ScannedItems);
        }
    }
}
