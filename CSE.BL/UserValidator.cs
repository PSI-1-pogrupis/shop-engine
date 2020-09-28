using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CSE.BL
{
    public class UserValidator
    {
        public bool ValidateEmail(string email, ref string error)
        {
            Regex validateEmail = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

            if (!validateEmail.IsMatch(email)) error += "Invalid email.\n";

            if (error.Length == 0) return true;
            else return false;
        }

        public bool ValidatePassword(string password, ref string error)
        {
            Regex hasNumber = new Regex(@"[0-9]+");
            Regex hasUpperChar = new Regex(@"[A-Z]+");
            Regex hasMinimum8Chars = new Regex(@".{8,}");

            if (!hasNumber.IsMatch(password)) error += "Password must contain a number.\n";
            if (!hasUpperChar.IsMatch(password)) error += "Password must contain a capital letter.\n";
            if (!hasMinimum8Chars.IsMatch(password)) error += "Password must contain atleast 8 characters.\n";

            if (error.Length == 0) return true;
            else return false;
        }
    }
}
