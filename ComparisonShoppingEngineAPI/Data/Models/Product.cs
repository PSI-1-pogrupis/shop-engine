using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComparisonShoppingEngineAPI.Data.Models
{
    public class Product
    {
        public Product()
        {
            ShopProduct = new HashSet<ShopProduct>();
        }
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        public virtual ICollection<ShopProduct> ShopProduct { get; set; }
    }
}
