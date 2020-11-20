using System;
using System.ComponentModel.DataAnnotations;

namespace ComparisonShoppingEngineAPI.Data.Models
{
    public class ShopProduct
    {
        [Key]
        public int ShopProductId { get; set; }
        public int ProductId { get; set; }
        public int? ShopId { get; set; }
        public decimal Price { get; set; }
        public DateTime? Date { get; set; }

        public virtual Product Product { get; set; }
        public virtual Shop Shop { get; set; }
    }
}
