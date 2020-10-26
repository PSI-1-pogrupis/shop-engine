using CSE.BL;
using CSE.BL.ScannedData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CSE.BL.BillingData;

namespace ViewModels
{
    public class CheckScanningViewModel : BaseViewModel
    {
        private readonly ImageReader imgReader;
        private readonly ItemsScanner itemsScanner;
        private ScannedListManager scannedListManager;
        private string browseText = "";
        public string BrowseText
        {
            get { return browseText; }
            set
            {
                browseText = value;
                OnPropertyChanged("BrowseText");
            }
        }
        private string readedText = "";
        public string ReadedText
        {
            get { return readedText; }
            set
            {
                readedText = value;
                OnPropertyChanged("ReadedText");
            }
        }
        private string shopText = "";
        public string ShopText
        {
            get { return shopText; }
            set
            {
                shopText = value;
                OnPropertyChanged("ShopText");
            }
        }
        private string listLabelContent;
        public string ListLabelContent
        {
            get { return listLabelContent; }
            set
            {
                listLabelContent = value;
                OnPropertyChanged("ListLabelContent");
            }
        }
        private BitmapSource imageSrc = new BitmapImage();
        public BitmapSource ImageSrc
        {
            get { return imageSrc; }
            set
            {
                imageSrc = value;
                OnPropertyChanged("ImageSrc");
            }
        }
        public ObservableCollection<ScannedItem> ScannedList
        {
            get { return new ObservableCollection<ScannedItem>(scannedListManager.ScannedItems); }
            set
            {
                scannedListManager.ScannedItems = new List<ScannedItem>(value);
                OnPropertyChanged("ScannedList");
            }
        }

        private ShopTypes selectedShop;
        public ShopTypes SelectedShop {
            get { return selectedShop;}
            set
            {
                if(selectedShop != value)
                {
                    selectedShop = value;
                    OnPropertyChanged("SelectedShop");
                }
            }
        }


        public ICommand BrowseCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }

        public CheckScanningViewModel()
        {
            BrowseCommand = new RelayCommand(Browse_Click, canExecute => true);
            ConfirmCommand = new RelayCommand(Confirm_List, canExecute => true);
            imgReader = new ImageReader();
            itemsScanner = new ItemsScanner();
            scannedListManager = new ScannedListManager();
            

        }

        private void Browse_Click(object obj)
        {
            ReadedText = "";
            BrowseText = "";
            ShopText = "";
            ImageSrc = null;
            ScannedList = new ObservableCollection<ScannedItem>();
         
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;"
            };

            Nullable<bool> result = dlg.ShowDialog();

            if (result != null && !String.IsNullOrEmpty(dlg.FileName))
            {
                BrowseText = dlg.FileName;
                ImageSrc = new BitmapImage(new Uri(dlg.FileName));

                ListLabelContent = "Reading...";

                Thread scanThread = new Thread(() => ScanThread(dlg));
                scanThread.Start();
            }
        }

        private void Confirm_List(object obj)
        {
            ScannedListLibrary.AddList(scannedListManager);
            MonthSpendingLibrary.AddToLibrary(DateTime.Now.Month, scannedListManager.TotalSum);
        }

        private void ScanThread(Microsoft.Win32.OpenFileDialog dlg)
        {
            Thread.CurrentThread.IsBackground = true;
            ReadedText = imgReader.ReadImage(dlg.FileName);
            itemsScanner.ScanProducts(scannedListManager, ReadedText);
            ScannedList = new ObservableCollection<ScannedItem>(scannedListManager.ScannedItems);
            

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
