using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Views
{
    /// <summary>
    /// Interaction logic for DukView.xaml
    /// </summary>
    public partial class DukView : UserControl
    {
        public DukView()
        {
            InitializeComponent();
        }

        private void Button_EmailUs_Click(object sender, RoutedEventArgs e)
        {
            dukScrollViewer.ScrollToEnd();
        }
    }
}
