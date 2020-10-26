using CSE.BL;
using CSE.BL.Database;
using CSE.BL.Interfaces;
using CSE.BL.ShoppingList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ViewModels
{
    public class ItemSelectionViewModel : BaseViewModel
    {
        public class ItemComparison
        {
            public ShopTypes Shop { get; set; }
            public string PriceDifference { get; set; }
        }

        private string searchText = "";

        private List<ShoppingItemData> productList;
        private List<ItemComparison> shopComparison;

        private ShoppingItemData selectedItem;
        private KeyValuePair<ShopTypes, decimal> selectedShop;
        private int selectedAmount = 1;
        private string selectedName = "";
        private bool isNotSelected = true;

        private readonly MainViewModel mainVM;

        public bool IsNotSelected
        {
            get { return isNotSelected; }
            set
            {
                isNotSelected = value;
                OnPropertyChanged(nameof(IsNotSelected));
            }
        }

        public List<ShoppingItemData> ProductList
        {
            get { return productList; }
            set
            {
                productList = value;
                OnPropertyChanged(nameof(ProductList));
            }
        }

        public List<ItemComparison> ShopComparison
        {
            get { return shopComparison; }
            set
            {
                shopComparison = value;
                OnPropertyChanged(nameof(ShopComparison));
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

        public ShoppingItemData SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (value != null) IsNotSelected = false;
                else IsNotSelected = true;

                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public KeyValuePair<ShopTypes, decimal> SelectedShop
        {
            get { return selectedShop; }
            set
            {
                selectedShop = value;
                FindShopDifferences();
                OnPropertyChanged(nameof(SelectedShop));
            }
        }

        public string SelectedAmount
        {
            get { return selectedAmount.ToString(); }
            set
            {
                if (int.TryParse(value, out int amount)) selectedAmount = amount;
                else selectedAmount = 1;

                FindShopDifferences();
                OnPropertyChanged(nameof(SelectedAmount));
            }
        }

        public string SelectedName
        {
            get { return selectedName; }
            set
            {
                selectedName = value;
                OnPropertyChanged(nameof(SelectedName));
            }
        }

        public ICommand ViewItemCommand { get; private set; }
        public ICommand AddItemCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }

        public ItemSelectionViewModel(MainViewModel _mainVM)
        {
            ViewItemCommand = new RelayCommand(ViewItem, canExecute => true);
            AddItemCommand = new RelayCommand(AddItem, canExecute => CanAddItem());
            SearchCommand = new RelayCommand(SearchList, canExecute => true);

            mainVM = _mainVM;

            SearchList(null);
        }

        private void SearchList(object parameter)
        {
            IsNotSelected = true;

            //TODO: Product list should be retrieved from the database
            using (IShoppingItemRepository repo = new ShoppingItemRepository())
            {
                ProductList = repo.GetAll();
            }

            string text = SearchText;

            var selected = from i in ProductList
                           where i.Name.Contains(text)
                           select i;

            ProductList = selected.ToList();
        }

        private void ViewItem(object parameter)
        {
            if(parameter is ShoppingItemData item)
            {
                SelectedItem = item;
                if (item.ShopPrices.Count > 0) SelectedShop = item.ShopPrices.First();
            }
        }

        private void AddItem(object parameter)
        {
            ShoppingItem item = new ShoppingItem(SelectedItem.Name, SelectedShop.Key, selectedShop.Value, selectedAmount, SelectedItem.Unit);

            mainVM.selectedShoppingList.AddItem(item);
            mainVM.ChangeViewCommand.Execute("EditShoppingList");
        }

        private bool CanAddItem() 
        {
            if (SelectedItem != null) return true;
            else return false;
        }

        private void FindShopDifferences()
        {
            List<ItemComparison> comparison = new List<ItemComparison>();

            foreach(KeyValuePair<ShopTypes, decimal> shop in SelectedItem.ShopPrices)
            {
                if (shop.Key == ShopTypes.UNKNOWN) continue;

                if(shop.Key != SelectedShop.Key)
                {
                    ItemComparison comp = new ItemComparison { Shop = shop.Key, PriceDifference = Math.Round((shop.Value - SelectedShop.Value) * selectedAmount, 2).ToString() };
                    comparison.Add(comp);
                }
            }

            ShopComparison = comparison;
        }
    }
}
