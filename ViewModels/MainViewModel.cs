using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ViewModels
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
            selectedViewModel = new HomeViewModel();
        }
        public ICommand ChangeViewCommand { get; set; }

        private void ChangeViewModel(object parameter)
        {

            if(parameter.ToString() == "CheckScanning")
            {
                SelectedViewModel = new CheckScanningViewModel();
            }

            return;
        }
    }
}
