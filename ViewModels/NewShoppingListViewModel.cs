using CSE.BL.ShoppingList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace ViewModels
{
    public class NewShoppingListViewModel : BaseViewModel
    {
        private string searchText = "";

        private readonly List<ShoppingItem> Products;

        private ObservableCollection<ShoppingItem> shoppingList;
        private List<ShoppingItem> productList;

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

        public ICommand AddItemCommand { get; private set; }

        public ICommand RemoveItemCommand { get; private set; }

        public ICommand SearchCommand { get; private set; }

        public NewShoppingListViewModel()
        {
            AddItemCommand = new RelayCommand(AddItem, canExecute => true);
            RemoveItemCommand = new RelayCommand(RemoveItem, canExecute => true);
            SearchCommand = new RelayCommand(SearchList, canExecute => true);

            ProductList = new List<ShoppingItem>();
            ShoppingList = new ObservableCollection<ShoppingItem>();

            Products = new List<ShoppingItem>();

            for (int i = 0; i < 20; i++)
            {
                Products.Add(new ShoppingItem("Item " + i.ToString(), 1, UnitTypes.kg));
            }

            productList = Products;
        }

        private void AddItem(object parameter)
        {
            if (parameter is ShoppingItem selectedItem)
            {
                foreach(ShoppingItem item in ShoppingList)
                {
                    if (item.Name.Equals(selectedItem.Name))
                    {
                        ShoppingList.Remove(item);
                        item.Amount += 1;
                        ShoppingList.Add(item);

                        return;
                    }
                }

                ShoppingList.Add(new ShoppingItem(selectedItem));
            }
        }

        private void RemoveItem(object parameter)
        {
            if (parameter is ShoppingItem selectedItem)
            {
                ShoppingList.Remove(selectedItem);

                if (selectedItem.Amount - 1 > 0)
                {
                    selectedItem.Amount -= 1;
                    ShoppingList.Add(selectedItem);
                }
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
    }
}
