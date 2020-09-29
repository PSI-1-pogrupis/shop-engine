using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string emailAddress = "";
        private string password = "";
        private string errorMessage = "";

        public string EmailAddress
        {
            get { return emailAddress; }
            set 
            { 
                emailAddress = value;
                OnPropertyChanged(nameof(EmailAddress));
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login, canExecute => true);
        }

        public ICommand LoginCommand { get; set; }

        private void Login(object parameter)
        {
            //TODO: Proper error handling. 

            ErrorMessage = "";

            if(EmailAddress.Equals("admin@gmail.com") && Password.Equals("password"))
            {
                //TODO: Connect to sql to validate the user and load information
                ErrorMessage = "Successful login.";
                return;
            }
            else
            {
                Password = "";
                ErrorMessage = "User not found.";
                return;
            }
        }

    }
}
