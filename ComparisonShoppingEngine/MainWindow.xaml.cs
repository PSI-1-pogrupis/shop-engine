using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComparisonShoppingEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            // Future feature: confirm logout operation and close program

            // Close program
            Application.Current.Shutdown();
        }

        private void HeaderGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Method to drag window left click drag on Header panel
            DragMove();
        }

        private void SearchTest_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Check if search input it not empty and Enter key is pressed
            if (Key.Enter == e.Key && !SearchText.Equals(""))
            {
                // Display text in content grid
                txt.Text = "Paieška atlikta!"+"\nGauti duomenys: "+SearchText.Text;
                SearchText.Text = "Search...";
            }
        }

        private void Help_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // "Need any help?" Label pressed
        }

        private void NewShoppingList_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}