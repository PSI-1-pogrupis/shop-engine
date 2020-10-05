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

            SelectedViewModel = (parameter.ToString()) switch
            {
                case "Home":
                    SelectedViewModel = new HomeViewModel();
                    break;
                case "CheckScanning":
                    SelectedViewModel = new CheckScanningViewModel();
                    break;
                case "NewShoppingList":
                    SelectedViewModel = new NewShoppingListViewModel();
                    break;
                case "BillingStatement":
                    SelectedViewModel = new BillingStatementViewModel();
                    break;
                case "ShoppingHistory":
                    SelectedViewModel = new ShoppingHistoryViewModel();
                    break;
                case "Settings":
                    SelectedViewModel = new SettingsViewModel();
                    break;
                case "Login":
                    SelectedViewModel = new LoginViewModel();
                    break;
                case "Register":
                    SelectedViewModel = new RegisterViewModel();
                    break;
                default:
                    SelectedViewModel = new HomeViewModel();
                    break;
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
