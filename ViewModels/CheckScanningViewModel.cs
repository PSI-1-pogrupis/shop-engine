using CSE.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace ViewModels
{
    public class CheckScanningViewModel : BaseViewModel
    {
        private ImageReader imgReader;
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
        private string labelContent;
        public string LabelContent
        {
            get { return labelContent; }
            set
            {
                labelContent = value;
                OnPropertyChanged("LabelContent");
            }
        }


        public ICommand BrowseCommand { get; set; }

        public CheckScanningViewModel()
        {
            BrowseCommand = new RelayCommand(Browse_Click, canExecute => true);
            imgReader = new ImageReader();
        }

        private void Browse_Click(object obj)
        {
            ReadedText = "";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;";

            Nullable<bool> result = dlg.ShowDialog();

            if (result != null)
            {
                BrowseText = dlg.FileName;

                LabelContent = "Reading...";

                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    ReadedText = imgReader.ReadImage(dlg.FileName);
                    LabelContent = "";
                }).Start();
            }

           


        }
    }
}
