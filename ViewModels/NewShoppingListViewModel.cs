using CSE.BL.ShoppingList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using CSE.BL.Interfaces;
using CSE.BL.Database;

namespace ViewModels
{
    public class NewShoppingListViewModel : BaseViewModel
    {
        private string searchText = "";
        private readonly List<ShoppingItem> Products;

        private ObservableCollection<ShoppingItem> shoppingList;
        private List<ShoppingItem> productList;

        private bool isPopupVisible = false;
        private ShoppingItem selectedItem;
        private int selectedAmount = 1;

        public ObservableCollection<ShoppingItem> ShoppingList
        {
            get { return shoppingList; }
            set
            {
                shoppingList = value;
                OnPropertyChanged(nameof(ShoppingList));
            }
        }

        public List<ShoppingItem> ProductList
        {
            get { return productList; }
            set
            {
                productList = value;
                OnPropertyChanged(nameof(ProductList));
            }
        }

        public string SearchText
        {
            get { return searchText; }

            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public ShoppingItem SelectedItem{
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public string SelectedAmount
        {
            get { return selectedAmount.ToString(); }
            set
            {
                if (int.TryParse(value, out int amount)) selectedAmount = amount;
                else selectedAmount = 1;

                OnPropertyChanged(nameof(SelectedAmount));
            }
        }

        public bool IsPopupVisible
        {
            get { return isPopupVisible; }
            set
            {
                isPopupVisible = value;
                OnPropertyChanged(nameof(IsPopupVisible));
            }
        }

        public ICommand ClosePopupCommand { get; private set; }
        public ICommand AddItemPopupCommand { get; private set; }
        public ICommand AddItemCommand { get; private set; }
        public ICommand RemoveItemCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public NewShoppingListViewModel()
        {
            AddItemCommand = new RelayCommand(AddItem, canExecute => true);
            RemoveItemCommand = new RelayCommand(RemoveItem, canExecute => true);
            SearchCommand = new RelayCommand(SearchList, canExecute => true);
            AddItemPopupCommand = new RelayCommand(AddPopup, canExecute => true);
            ClosePopupCommand = new RelayCommand(ClosePopup, canExecute => true);

            ProductList = new List<ShoppingItem>();
            ShoppingList = new ObservableCollection<ShoppingItem>();

            Products = new List<ShoppingItem>();

            using (IShoppingItemRepository repo = new ShoppingItemRepository())
            {
                Products = repo.GetAll();
            }

            productList = Products;
        }

        private void AddPopup(object parameter)
        {
            if(parameter is ShoppingItem selected)
            {
                SelectedItem = selected;
                IsPopupVisible = true;
            }
        }

        private void AddItem(object parameter)
        {   
            foreach(ShoppingItem item in ShoppingList)
            {
                if (item.Name.Equals(SelectedItem.Name))
                {
                    ShoppingList.Remove(item);
                    item.Amount += selectedAmount;
                    ShoppingList.Add(item);

                    ClosePopup(null);
                    return;
                }
            }

            ShoppingItem newItem = (new ShoppingItem(SelectedItem));
            newItem.Amount = selectedAmount;
            ShoppingList.Add(newItem);

            ClosePopup(null);
        }

        private void RemoveItem(object parameter)
        {
            if (parameter is ShoppingItem selectedItem)
            {
                ShoppingList.Remove(selectedItem);
            }
        }

        private void SearchList(object parameter)
        {

            string text = SearchText;

            var selected = from i in Products
                           where i.Name.Contains(text)
                           select i;

            ProductList = selected.ToList();
        }

        private void ClosePopup(object parameter)
        {
            SelectedAmount = "1";
            SelectedItem = null;
            IsPopupVisible = false;
        }
        
    }
}
