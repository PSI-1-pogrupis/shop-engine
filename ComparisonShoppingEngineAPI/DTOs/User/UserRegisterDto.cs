using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.DTOs.User
{
    public class UserRegisterDto
    {
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
