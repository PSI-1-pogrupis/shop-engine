using CSE.BL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string emailAddress = "";
        private string password = "";
        private string confirmPassword = "";
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

        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
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

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(Register, canExecute => true);
        }

        public ICommand RegisterCommand { get; set; }

        private void Register(object parameter)
        {
            UserValidator validator = new UserValidator();
            string message = "";

            if (!Password.Equals(ConfirmPassword))
            {
                message = "Passwords do not match.\n";
                ClearEntry();
            }

            if (validator.ValidateEmail(EmailAddress, ref message) & validator.ValidatePassword(Password, ref message))
            {
                //TODO: Register the user into the database
                message = "Successfully registered an account.";
            }
            else
            {
                ClearEntry();
            }

            ErrorMessage = message;
        }

        private void ClearEntry()
        {
            Password = "";
            ConfirmPassword = "";
        }
    }
}
