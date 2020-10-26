using CSE.BL.ShoppingList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Linq;
using System.Security.Cryptography.Xml;
using CSE.BL;
using CSE.BL.Interfaces;
using CSE.BL.Database;
using System.Data;
using System.Net.NetworkInformation;

namespace ViewModels
{
    public class NewShoppingListViewModel : BaseViewModel
    {
        private readonly MainViewModel mainVM;
        private ShoppingListManager manager;

        private ShoppingListManager optimizedList;
        private ObservableCollection<ShoppingItem> shoppingList;
        private List<ShoppingItemData> dataList;
        private List<ShopTypes> listShops;
        private List<ShopTypes> selectedShops;
        private List<ShopTypes> availableShops;

        private string selectedName = "";
        private decimal estimatedPrice = 0;
        private decimal optimizedListPriceDifference = 0;
        private bool showOptimizedList = false;
        private bool onlyReplaceUnspecifiedShops = false;
        private readonly bool editMode = false;

        public ShoppingListManager OptimizedList {
            get { return optimizedList; }
            set
            {
                optimizedList = value;
                OnPropertyChanged(nameof(OptimizedList));
                OnPropertyChanged(nameof(OptimizedListEstimatedPrice));
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

        public List<ShopTypes> ListShops
        {
            get { return listShops; }
            set
            {
                listShops = value;
                OnPropertyChanged(nameof(ListShops));
            }
        }

        public List<ShopTypes> AvailableShops
        {
            get { return availableShops; }
            set
            {
                availableShops = value;
                OnPropertyChanged(nameof(AvailableShops));
            }
        }

        public bool IsSelected { get; set; }

        public decimal EstimatedPrice
        {
            get { return estimatedPrice; }
            set
            {
                estimatedPrice = Math.Round(value, 2);
                OnPropertyChanged(nameof(EstimatedPrice));
            }
        }

        public decimal OptimizedListEstimatedPrice
        {
            get {
                if (optimizedList != null)
                    return Math.Round(optimizedList.EstimatedPrice, 2);
                else return 0;
            }
        }

        public decimal OptimizedListPriceDifference
        {
            get { return optimizedListPriceDifference; }
            set
            {
                optimizedListPriceDifference = Math.Round(value, 2);
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
        public ICommand SaveAsNewCommand { get; private set; }
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
            SaveAsNewCommand = new RelayCommand(SaveAsNew, canExecute => true);
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

            selectedShops = new List<ShopTypes>();
            ObservableShoppingList = new ObservableCollection<ShoppingItem>(manager.ShoppingList);

            dataList = new List<ShoppingItemData>();
            
            using (IShoppingItemRepository repo = new ShoppingItemRepository())
            {
                foreach(ShoppingItem item in manager.ShoppingList)
                {
                    ShoppingItemData data = repo.Find(item.Name);

                    if (data != null) dataList.Add(data);
                }
            }
            
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
                for(int i = 0; i < ObservableShoppingList.Count; i++)
                {
                    if(ObservableShoppingList[i] == item)
                    {
                        ObservableShoppingList.RemoveAt(i);
                        manager.RemoveItem(i);
                        dataList.RemoveAt(i);
                        UpdateOverview();
                        GetAvailableShops();
                        return;
                    }
                }
            }
        }

        private bool CanSaveShoppingList()
        {
            if (ObservableShoppingList.Count() == 0) return false;

            return true;
        }

        private void SaveShoppingList(object parameter)
        {
            if (editMode && mainVM.loadedShoppingLists.Contains(mainVM.selectedShoppingList)) mainVM.loadedShoppingLists.Remove(mainVM.selectedShoppingList);

            mainVM.loadedShoppingLists.Insert(0, manager);
            mainVM.selectedShoppingList = manager;

            ShoppingListResourceProcessor.SaveLists(mainVM.loadedShoppingLists);

            mainVM.ChangeViewCommand.Execute("ViewShoppingList");
        }

        private void UpdateOverview()
        {
            ListShops = manager.UniqueShops;
            EstimatedPrice = manager.EstimatedPrice;
        }

        private void GetAvailableShops()
        {
            List<ShopTypes> shops = new List<ShopTypes>();

            foreach(ShoppingItemData item in dataList)
            {
                foreach(KeyValuePair<ShopTypes, decimal> shop in item.ShopPrices)
                {
                    if (shop.Key == ShopTypes.UNKNOWN || shops.Contains(shop.Key)) continue;

                    shops.Add(shop.Key);
                }
            }

            AvailableShops = shops;
        }

        private void OptimizeShoppingList(object parameter)
        {
            ListOptimizer optimizer = new ListOptimizer();

            OptimizedList = optimizer.GetLowestPriceList(manager, dataList, selectedShops, OnlyReplaceUnspecifiedShops);

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
            if(parameter is ShopTypes shop)
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

        private void SaveAsNew(object parameter)
        {
            ShoppingListManager newManager = new ShoppingListManager(OptimizedList.ShoppingList);
            newManager.Name = SelectedName + "_OPT";

            mainVM.loadedShoppingLists.Remove(manager);
            mainVM.loadedShoppingLists.Insert(0, newManager);
            ShoppingListResourceProcessor.SaveLists(mainVM.loadedShoppingLists);
            mainVM.loadedShoppingLists.Add(manager);

            CancelOptimization(null);
        }

        private void CancelOptimization(object parameter)
        {
            GetAvailableShops();
            ShowOptimizedList = false;
            selectedShops = new List<ShopTypes>();
            OnlyReplaceUnspecifiedShops = false;
        }
    }
}
