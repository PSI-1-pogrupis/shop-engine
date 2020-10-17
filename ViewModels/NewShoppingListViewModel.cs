using CSE.BL.ShoppingList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Linq;
using System.Security.Cryptography.Xml;

namespace ViewModels
{
    public class NewShoppingListViewModel : BaseViewModel
    {
        private readonly MainViewModel mainVM;
        private readonly ShoppingListManager manager;
        private ObservableCollection<ShoppingItem> shoppingList;
        private List<string> listShops;

        private string selectedName = "";
        private double estimatedPrice = 0;
        private readonly bool editMode = false;

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

        public double EstimatedPrice
        {
            get { return estimatedPrice; }
            set
            {
                estimatedPrice = value;
                OnPropertyChanged(nameof(EstimatedPrice));
            }
        }

        public ICommand SaveShoppingListCommand { get; private set; }
        public ICommand AddItemCommand { get; private set; }
        public ICommand RemoveItemCommand { get; private set; }
        public NewShoppingListViewModel(MainViewModel _mainVM, bool _editMode)
        {
            mainVM = _mainVM;
            editMode = _editMode;

            AddItemCommand = new RelayCommand(AddItem, canExecute => true);
            RemoveItemCommand = new RelayCommand(RemoveItem, canExecute => true);
            SaveShoppingListCommand = new RelayCommand(SaveShoppingList, canExecute => CanSaveShoppingList());

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

            ObservableShoppingList = new ObservableCollection<ShoppingItem>(manager.ShoppingList);
            UpdateOverview();
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
    }
}
