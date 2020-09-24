using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ComparisonShoppingEngine
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel selectedViewModel;

        public BaseViewModel SelectedViewModel
        {
            get { return selectedViewModel; }
            set 
            { 
                selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public MainViewModel()
        {
            ChangeViewCommand = new RelayCommand(ChangeViewModel, canExecute => true);
        }
        public ICommand ChangeViewCommand { get; set; }

        private void ChangeViewModel(object parameter)
        {
            //TODO: Change the viewmodel based on the parameter

            return;
        }
    }
}
