using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Models
{
    public class User
    {
        public User()
        {
            Receipts = new HashSet<Receipt>();
        }

        [Key]
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
