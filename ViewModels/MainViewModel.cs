using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel selectedViewModel;
        private BaseViewModel statusPanel;
        
        public BaseViewModel SelectedViewModel
        {
            get { return selectedViewModel; }
            set 
            { 
                selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }
        public BaseViewModel StatusPanel
        {
            get { return statusPanel; }
            set
            {
                statusPanel = value;
                OnPropertyChanged(nameof(StatusPanel));
            }
        }

        public MainViewModel()
        {
            ChangeViewCommand = new RelayCommand(ChangeViewModel, canExecute => true);
            ChangeStatusPanelCommand = new RelayCommand(ChangeStatusPanel, canExecute => true);
            SelectedViewModel = new HomeViewModel();
            StatusPanel = null;
        }
        public ICommand ChangeViewCommand { get; set; }

        public ICommand ChangeStatusPanelCommand { get; set; }

        private void ChangeViewModel(object parameter)
        {
            StatusPanel = null;

            //TODO: replace this ugly code
            if(parameter.ToString() == "CheckScanning")
            {
                SelectedViewModel = new CheckScanningViewModel();
            }else if(parameter.ToString() == "Login")
            {
                SelectedViewModel = new LoginViewModel();
            }else if(parameter.ToString() == "Register")
            {
                SelectedViewModel = new RegisterViewModel();
            }

            return;
        }

        private void ChangeStatusPanel(object parameter)
        {
            if (StatusPanel == null) StatusPanel = new UserLoggedOutViewModel(this);
            else StatusPanel = null;
        }
    }
}
