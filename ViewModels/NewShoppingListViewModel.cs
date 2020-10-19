using CSE.BL.ShoppingList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Linq;
using System.Security.Cryptography.Xml;
using CSE.BL;

namespace ViewModels
{
    public class NewShoppingListViewModel : BaseViewModel
    {
        private readonly MainViewModel mainVM;
        private ShoppingListManager manager;

        private ShoppingListManager optimizedList;
        private ObservableCollection<ShoppingItem> shoppingList;
        private List<string> listShops;
        private List<string> selectedShops;
        private List<string> availableShops;

        private string selectedName = "";
        private double estimatedPrice = 0;
        private double optimizedListPriceDifference = 0;
        private bool showOptimizedList = false;
        private bool onlyReplaceUnspecifiedShops = false;
        private readonly bool editMode = false;

        public ShoppingListManager OptimizedList {
            get { return optimizedList; }
            set
            {
                optimizedList = value;
                OnPropertyChanged(nameof(OptimizedList));
            }
        }

        public ObservableCollection<ShoppingItem> ObservableShoppingList
        {
            get => shoppingList;
            set
            {
                shoppingList = value;
                OnPropertyChanged(nameof(ObservableShoppingList));
            }
        }

        public string SelectedName
        {
            get { return selectedName; }
            set
            {
                selectedName = value;
                manager.Name = value;
                OnPropertyChanged(nameof(SelectedName));
            }
        }

        public List<string> ListShops
        {
            get { return listShops; }
            set
            {
                listShops = value;
                OnPropertyChanged(nameof(ListShops));
            }
        }

        public List<string> AvailableShops
        {
            get { return availableShops; }
            set
            {
                availableShops = value;
                OnPropertyChanged(nameof(AvailableShops));
            }
        }

        public bool IsSelected { get; set; }

        public double EstimatedPrice
        {
            get { return estimatedPrice; }
            set
            {
                estimatedPrice = value;
                OnPropertyChanged(nameof(EstimatedPrice));
            }
        }

        public double OptimizedListPriceDifference
        {
            get { return optimizedListPriceDifference; }
            set
            {
                optimizedListPriceDifference = value;
                OnPropertyChanged(nameof(OptimizedListPriceDifference));
            }
        }

        public bool ShowOverview
        {
            get { return !showOptimizedList; }
        }

        public bool ShowOptimizedList
        {
            get { return showOptimizedList; }
            set
            {
                showOptimizedList = value;
                OnPropertyChanged(nameof(ShowOptimizedList));
                OnPropertyChanged(nameof(ShowOverview));
            }
        }

        public bool OnlyReplaceUnspecifiedShops
        {
            get { return onlyReplaceUnspecifiedShops; }
            set
            {
                onlyReplaceUnspecifiedShops = value;
                OnPropertyChanged(nameof(OnlyReplaceUnspecifiedShops));
            }
        }

        public ICommand SaveShoppingListCommand { get; private set; }
        public ICommand AddItemCommand { get; private set; }
        public ICommand RemoveItemCommand { get; private set; }
        public ICommand OptimizeShoppingListCommand { get; private set; }
        public ICommand UpdateSelectedShopsCommand { get; private set; }
        public ICommand ReplaceShoppingListCommand { get; private set; }
        public ICommand CancelOptimizationCommand { get; private set; }
        public NewShoppingListViewModel(MainViewModel _mainVM, bool _editMode)
        {
            mainVM = _mainVM;
            editMode = _editMode;

            AddItemCommand = new RelayCommand(AddItem, canExecute => true);
            RemoveItemCommand = new RelayCommand(RemoveItem, canExecute => true);
            SaveShoppingListCommand = new RelayCommand(SaveShoppingList, canExecute => CanSaveShoppingList());
            OptimizeShoppingListCommand = new RelayCommand(OptimizeShoppingList, canExecute => CanOptimizeList());
            UpdateSelectedShopsCommand = new RelayCommand(UpdateSelectedShops, canExecute => true);
            ReplaceShoppingListCommand = new RelayCommand(ReplaceShoppingList, canExecute => true);
            CancelOptimizationCommand = new RelayCommand(CancelOptimization, canExecute => true);

            if (editMode)
            {
                manager = mainVM.selectedShoppingList;
                manager.Name = mainVM.selectedShoppingList.Name;
                SelectedName = manager.Name;
            }
            else
            {
                manager = new ShoppingListManager();
                SelectedName = "New Shopping List";
            }

            selectedShops = new List<string>();
            ObservableShoppingList = new ObservableCollection<ShoppingItem>(manager.ShoppingList);
            UpdateOverview();
            GetAvailableShops();

            ShowOptimizedList = false;
        }

        private void AddItem(object parameter)
        {
            mainVM.selectedShoppingList = manager;
            mainVM.ChangeViewCommand.Execute("ItemSelection");
        }

        private void RemoveItem(object parameter)
        {
            if (parameter is ShoppingItem item)
            {
                ObservableShoppingList.Remove(item);
                manager.RemoveItem(item);
                UpdateOverview();
            }

            GetAvailableShops();
        }

        private bool CanSaveShoppingList()
        {
            if (ObservableShoppingList.Count() == 0) return false;

            return true;
        }

        private void SaveShoppingList(object parameter)
        {
            if (editMode) mainVM.loadedShoppingLists.Remove(mainVM.selectedShoppingList);

            mainVM.loadedShoppingLists.Insert(0, manager);
            mainVM.selectedShoppingList = manager;

            ShoppingListResourceProcessor.SaveLists(mainVM.loadedShoppingLists);

            mainVM.ChangeViewCommand.Execute("ViewShoppingList");
        }

        private void UpdateOverview()
        {
            ListShops = manager.UniqueShops;
            EstimatedPrice = Math.Round(manager.EstimatedPrice, 2);
        }

        private void GetAvailableShops()
        {
            List<string> shops = new List<string>();

            foreach(ShoppingItem item in manager.ShoppingList)
            {
                foreach((string, double) shop in item.ShopPrices)
                {
                    if (shop.Item1.Equals("ANY") || shops.Contains(shop.Item1)) continue;

                    shops.Add(shop.Item1);
                }
            }

            AvailableShops = shops;
        }

        private void OptimizeShoppingList(object parameter)
        {
            ListOptimizer optimizer = new ListOptimizer();

            OptimizedList = optimizer.GetLowestPriceList(manager, selectedShops, OnlyReplaceUnspecifiedShops);

            OptimizedListPriceDifference = Math.Round(manager.EstimatedPrice - OptimizedList.EstimatedPrice, 2);
            ShowOptimizedList = true;
        }

        private bool CanOptimizeList()
        {
            if (selectedShops.Count > 0) return true;
            else return false;
        }

        private void UpdateSelectedShops(object parameter)
        {
            if(parameter is string shop)
            {
                if (IsSelected)
                    selectedShops.Add(shop);
                else selectedShops.Remove(shop);
            }
        }

        private void ReplaceShoppingList(object parameter)
        {
            manager = OptimizedList;
            manager.Name = SelectedName;

            ObservableShoppingList = new ObservableCollection<ShoppingItem>(manager.ShoppingList);
            EstimatedPrice = manager.EstimatedPrice;
            ListShops = manager.UniqueShops;

            CancelOptimization(null);
        }

        private void CancelOptimization(object parameter)
        {
            GetAvailableShops();
            ShowOptimizedList = false;
            selectedShops = new List<string>();
            OnlyReplaceUnspecifiedShops = false;
        }
    }
}
