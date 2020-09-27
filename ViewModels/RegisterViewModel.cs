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
            //TODO: Proper error handling

            ErrorMessage = "";

            if (!ValidateEmail(EmailAddress))
            {
                ErrorMessage = "Enter a valid address.";
                ClearEntry();
                return;
            }

            if (!ValidatePassword(Password))
            {
                ErrorMessage = "Enter a valid password.";
                ClearEntry();
                return;
            }

            if (!Password.Equals(ConfirmPassword))
            {
                ErrorMessage = "Passwords don't match";
                ClearEntry();
                return;
            }

            ErrorMessage = "Successfully registered an account.";
        }

        //TODO: Move this somewhere else since they are used across multiple classes
        private bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        private bool ValidatePassword(string password)
        {
            Regex hasNumber = new Regex(@"[0-9]+");
            Regex hasUpperChar = new Regex(@"[A-Z]+");
            Regex hasMinimum8Chars = new Regex(@".{8,}");

            return (hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password));
        }

        private void ClearEntry()
        {
            EmailAddress = "";
            Password = "";
            ConfirmPassword = "";
        }
    }
}
