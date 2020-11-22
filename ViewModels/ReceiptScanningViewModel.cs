using CSE.BL;
using CSE.BL.ScannedData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CSE.BL.BillingData;
using CSE.BL.Interfaces;
using CSE.BL.Database;
using CSE.BL.Database.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ReceiptScanningViewModel : BaseViewModel
    {
        private readonly ImageReader _imgReader;
        private readonly ItemsScanner _itemsScanner;
        private readonly MainViewModel _mainVM;
        private readonly ScannedListManager _scannedListManager;
        private string _browseText = "";

        public string BrowseText
        {
            get { return _browseText; }
            set
            {
                _browseText = value;
                OnPropertyChanged(nameof(BrowseText));
            }
        }
        private string _readText = "";
        public string ReadText
        {
            get { return _readText; }
            set
            {
                _readText = value;
                OnPropertyChanged(nameof(ReadText));
            }
        }
        private string _shopText = "";
        public string ShopText
        {
            get { return _shopText; }
            set
            {
                _shopText = value;
                OnPropertyChanged(nameof(ShopText));
            }
        }
        private string _listLabelContent;
        public string ListLabelContent
        {
            get { return _listLabelContent; }
            set
            {
                _listLabelContent = value;
                OnPropertyChanged(nameof(ListLabelContent));
            }
        }
        private BitmapSource _imageSrc = new BitmapImage();
        public BitmapSource ImageSrc
        {
            get { return _imageSrc; }
            set
            {
                _imageSrc = value;
                OnPropertyChanged(nameof(ImageSrc));
            }
        }
        public ObservableCollection<ScannedItem> ScannedList
        {
            get { return new ObservableCollection<ScannedItem>(_scannedListManager.ScannedItems); }
            set
            {
                _scannedListManager.ScannedItems = new List<ScannedItem>(value);
                OnPropertyChanged(nameof(ScannedList));
            }
        }

        private ShopTypes _selectedShop;
        public ShopTypes SelectedShop
        {
            get { return _selectedShop; }
            set
            {
                if (_selectedShop != value)
                {
                    _selectedShop = value;
                    OnPropertyChanged(nameof(SelectedShop));
                }
            }
        }


        public ICommand BrowseCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }

        public ReceiptScanningViewModel(MainViewModel mainVM)
        {
            BrowseCommand = new RelayCommand(Browse_Click, canExecute => true);

            ConfirmCommand = new RelayCommand(Confirm_List, canExecute => CanConfirmList());

            _imgReader = new ImageReader();
            _itemsScanner = new ItemsScanner();
            _scannedListManager = new ScannedListManager();
            _mainVM = mainVM;


        }

        private void Confirm_Click(object obj)
        {
            _itemsScanner.SeedShops(_scannedListManager, SelectedShop);
            _mainVM.ProductsListToCompare = _scannedListManager;
            _mainVM.ChangeViewCommand.Execute("ProductsComparison");
        }

        private async void Browse_Click(object obj)
        {
            ReadText = "";
            BrowseText = "";
            ShopText = "";
            ImageSrc = null;
            ScannedList = new ObservableCollection<ScannedItem>();

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;"
            };

            bool? result = dlg.ShowDialog();

            if (result != null && !string.IsNullOrEmpty(dlg.FileName))
            {
                BrowseText = dlg.FileName;
                ImageSrc = new BitmapImage(new Uri(dlg.FileName));

                ListLabelContent = "Reading...";

                await ScanImage(dlg);
            }
        }

        private void Confirm_List(object obj)
        {
            ScannedListLibrary.AddList(_scannedListManager);
            MonthSpendingLibrary.AddToLibrary(DateTime.Now, _scannedListManager.TotalSum);

            using (IShoppingItemRepository repo = new ShoppingItemRepository(new MysqlShoppingItemGateway()))
            {
                ItemDataSelector selector = new ItemDataSelector();

                List<ShoppingItemData> items = repo.GetAll();

                foreach (ScannedItem item in ScannedList)
                {
                    ShoppingItemData data = selector.FindClosestItem(item.Name, items);

                    if (data == null) continue;

                    if (SelectedShop == ShopTypes.UNKNOWN) continue;

                    if (!data.ShopPrices.ContainsKey(SelectedShop))
                    {
                        data.ShopPrices.Add(SelectedShop, item.Price);
                        continue;
                    }

                    data.ShopPrices[SelectedShop] = item.Price;
                }

                repo.SaveChanges();
            }
            Confirm_Click(obj);
        }

        private bool CanConfirmList()
        {
            if (ScannedList.Count <= 0 || SelectedShop == ShopTypes.UNKNOWN) return false;
            else return true;
        }


        private async Task ScanImage(Microsoft.Win32.OpenFileDialog dlg)
        {
            ReadText = await _imgReader.ReadImageAsync(dlg.FileName);
            await _itemsScanner.ScanProducts(_scannedListManager, ReadText);
            ScannedList = new ObservableCollection<ScannedItem>(_scannedListManager.ScannedItems);

            //ShopTypes shop = itemsScanner.GetShop(ReadedText);
            //switch (shop)
            //{
            //    case ShopTypes.IKI:
            //        ShopText = ShopTypes.IKI.ToString();
            //        break;
            //    case ShopTypes.MAXIMA:
            //        ShopText = ShopTypes.MAXIMA.ToString();
            //        break;
            //    case ShopTypes.LIDL:
            //        ShopText = ShopTypes.LIDL.ToString();
            //        break;
            //    case ShopTypes.NORFA:
            //        ShopText = ShopTypes.NORFA.ToString();
            //        break;
            //    case ShopTypes.RIMI:
            //        ShopText = ShopTypes.RIMI.ToString();
            //        break;
            //    case ShopTypes.UNKNOWN:
            //        ShopText = "";
            //        break;
            //}
            ListLabelContent = "";
        }
    }


}
