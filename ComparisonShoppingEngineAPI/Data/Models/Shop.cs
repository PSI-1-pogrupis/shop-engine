using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
