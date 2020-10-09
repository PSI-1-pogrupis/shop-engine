using CSE.BL;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ViewModels
{
    public class CheckScanningViewModel : BaseViewModel
    {
        private ImageReader imgReader;
        private ItemsScanner itemsScanner;
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
        private BitmapImage imageSrc = new BitmapImage();
        public BitmapImage ImageSrc
        {
            get {
                return imageSrc;
            }
            set
            {
               

                imageSrc = value;
                OnPropertyChanged("ImageSrc");
            }
        }


        public ICommand BrowseCommand { get; set; }

        public CheckScanningViewModel()
        {
            BrowseCommand = new RelayCommand(Browse_Click, canExecute => true);
            imgReader = new ImageReader();
            itemsScanner = new ItemsScanner();
        }

        private void Browse_Click(object obj)
        {
            ReadedText = "";
            BrowseText = "";
            ShopText = "";
            ImageSrc = null;
         
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

        private void ScanThread(Microsoft.Win32.OpenFileDialog dlg)
        {
            Thread.CurrentThread.IsBackground = true;
            ReadedText = imgReader.ReadImage(dlg.FileName);
            ShopTypes shop = itemsScanner.GetShop(ReadedText);
            switch (shop)
            {
                case ShopTypes.IKI:
                    ShopText = ShopTypes.IKI.ToString();
                    break;
                case ShopTypes.MAXIMA:
                    ShopText = ShopTypes.MAXIMA.ToString();
                    break;
                case ShopTypes.LIDL:
                    ShopText = ShopTypes.LIDL.ToString();
                    break;
                case ShopTypes.NORFA:
                    ShopText = ShopTypes.NORFA.ToString();
                    break;
                case ShopTypes.RIMI:
                    ShopText = ShopTypes.RIMI.ToString();
                    break;
                case ShopTypes.UNKNOWN:
                    ShopText = ShopTypes.UNKNOWN.ToString();
                    break;
            }
            ListLabelContent = "";
        }
    }
}
