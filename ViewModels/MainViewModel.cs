using CSE.BL.ShoppingList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel selectedViewModel;
        private BaseViewModel statusPanel;

        public ShoppingListManager selectedShoppingList;
        public List<ShoppingListManager> loadedShoppingLists;

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
            loadedShoppingLists = new List<ShoppingListManager>();
            selectedShoppingList = null;
            SelectedViewModel = new HomeViewModel(this);
            StatusPanel = null;
        }
        public ICommand ChangeViewCommand { get; set; }

        public ICommand ChangeStatusPanelCommand { get; set; }

        private void ChangeViewModel(object parameter)
        {
            StatusPanel = null;

            SelectedViewModel = (parameter.ToString()) switch
            {
                "Home" => new HomeViewModel(this),
                "CheckScanning" => new CheckScanningViewModel(this),
                "ProductsComparison" => new ProductsComparisonViewModel(),
                "NewShoppingList" => new NewShoppingListViewModel(this, false),
                "EditShoppingList" => new NewShoppingListViewModel(this, true),
                "ItemSelection" => new ItemSelectionViewModel(this),
                "BillingStatement" => new BillingStatementViewModel(),
                "ShoppingHistory" => new ShoppingHistoryViewModel(),
                "Settings" => new SettingsViewModel(),
                "Login" => new LoginViewModel(),
                "Register" => new RegisterViewModel(),
                "Faq" => new FaqViewModel(),
                _ => new HomeViewModel(this),
            };
            return;
        }

        private void ChangeStatusPanel(object parameter)
        {
            if (StatusPanel == null) StatusPanel = new UserLoggedOutViewModel(this);
            else StatusPanel = null;
        }

    }
}