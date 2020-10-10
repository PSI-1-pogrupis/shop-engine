using CSE.BL;
using CSE.BL.ShoppingList;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private List<ShoppingListManager> shoppingLists;
        private MainViewModel mainVM;

        public List<ShoppingListManager> ShoppingLists
        {
            get { return shoppingLists; }
            set
            {
                shoppingLists = value;
                OnPropertyChanged(nameof(ShoppingLists));
            }
        }

        public ICommand ViewShoppingListCommand { get; private set; }
        public ICommand EditShoppingListCommand { get; private set; }
        public ICommand DeleteShoppingListCommand { get; private set; }

        public HomeViewModel(MainViewModel _mainVM)
        {
            mainVM = _mainVM;

            ViewShoppingListCommand = new RelayCommand(ViewShoppingList, canExecute => true);
            EditShoppingListCommand = new RelayCommand(EditShoppingList, canExecute => true);
            DeleteShoppingListCommand = new RelayCommand(DeleteShoppingList, canExecute => true);

            LoadSavedLists();
        }

        private void LoadSavedLists()
        {
            List<ShoppingListManager> loadedLists = new List<ShoppingListManager>();

            try
            {
                loadedLists = ShoppingListResourceProcessor.LoadLists();
            }
            catch (Exception e)
            {
                Console.WriteLine("No lists found.");
            }
            

            mainVM.loadedShoppingLists = loadedLists;
            ShoppingLists = loadedLists;
        }

        private void ViewShoppingList(object parameter)
        {

        }

        private void EditShoppingList(object parameter)
        {
            if(parameter is ShoppingListManager manager)
            {
                mainVM.selectedShoppingList = manager;

                mainVM.ChangeViewCommand.Execute("EditShoppingList");
            }
        }

        private void DeleteShoppingList(object parameter)
        {
            if (parameter is ShoppingListManager manager)
            {
                mainVM.loadedShoppingLists.Remove(manager);

                ShoppingListResourceProcessor.SaveLists(mainVM.loadedShoppingLists);

                LoadSavedLists();
            }
        }
    }
}
