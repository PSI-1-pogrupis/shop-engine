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
            public string Shop { get; set; }
            public string PriceDifference { get; set; }
        }

        private string searchText = "";

        private List<ShoppingItem> productList;
        private List<ItemComparison> shopComparison;

        private ShoppingItem selectedItem;
        private (string, double) selectedShop;
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

        public List<ShoppingItem> ProductList
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

        public ShoppingItem SelectedItem
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

        public (string, double) SelectedShop
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
            ProductList = new List<ShoppingItem>();

            for (int i = 0; i < 20; i++)
            {
                ProductList.Add(new ShoppingItem("Item " + i.ToString(), 1, UnitTypes.kg));
            }

            string text = SearchText;

            var selected = from i in ProductList
                           where i.Name.Contains(text)
                           select i;

            ProductList = selected.ToList();
        }

        private void ViewItem(object parameter)
        {
            if(parameter is ShoppingItem item)
            {
                SelectedItem = item;
                if (item.ShopPrices.Count > 0) SelectedShop = item.ShopPrices[0];
            }
        }

        private void AddItem(object parameter)
        {
            ShoppingItem item = new ShoppingItem(SelectedItem)
            {
                Amount = selectedAmount,
                SelectedShop = SelectedShop
            };

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

            foreach((string, double) shop in SelectedItem.ShopPrices)
            {
                if (shop.Item1.Equals("ANY")) continue;

                if(shop.Item1 != SelectedShop.Item1)
                {
                    ItemComparison comp = new ItemComparison { Shop = shop.Item1, PriceDifference = Math.Round((shop.Item2 - SelectedShop.Item2) * selectedAmount, 2).ToString() };
                    comparison.Add(comp);
                }
            }

            ShopComparison = comparison;
        }
    }
}
