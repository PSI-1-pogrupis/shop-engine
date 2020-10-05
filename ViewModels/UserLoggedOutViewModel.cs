using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ViewModels
{
    public class UserLoggedOutViewModel : BaseViewModel
    {
        private readonly MainViewModel mainVM;

        public UserLoggedOutViewModel(MainViewModel vm)
        {
            mainVM = vm;

            ChangeViewCommand = mainVM.ChangeViewCommand;
        }

        public ICommand ChangeViewCommand { get; set; }
    }
}
