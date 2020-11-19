using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Models
{
    public class Shop
    {
        public Shop()
        {
            ShopProduct = new HashSet<ShopProduct>();
        }
        [Key]
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public virtual ICollection<ShopProduct> ShopProduct { get; set; }
    }
}
