using CSE.BL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ViewModels
{
    public class FaqViewModel : BaseViewModel
    {
        private string name = "";
        private string emailAddress = "";
        private string question = "";
        private string errorMessage = "";

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string EmailAddress
        {
            get { return emailAddress; }
            set
            {
                emailAddress = value;
                OnPropertyChanged(nameof(EmailAddress));
            }
        }

        public string Question
        {
            get { return question; }
            set
            {
                question = value;
                OnPropertyChanged(nameof(Question));
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

        public FaqViewModel()
        {
            SendCommand = new RelayCommand(Send, canExecute => true);
        }

        public ICommand SendCommand { get; set; }

        private void Send(object parameter)
        {
            UserValidator validator = new UserValidator();
            string message = "";

            if (validator.ValidateEmail(EmailAddress, ref message) & !Name.Equals("") & !Question.Equals(""))
            {
                //TODO: Send question
                message = "Sent successfully!";
                ClearEntry();
            }
            else
            {
                message += "\nEmpty fields.";
            }

            ErrorMessage = message;
        }

        private void ClearEntry()
        {
            Name = "";
            EmailAddress = "";
            Question = "";
        }
    }
}
