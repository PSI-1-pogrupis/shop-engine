using System.ComponentModel.DataAnnotations;

namespace ComparisonShoppingEngineAPI.Data.Models
{
    public class ReceiptProduct
    {
        [Key]
        public int ReceiptProductId { get; set; }
        public int ReceiptId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal PricePerQuantity { get; set; }
        public virtual Product Product { get; set; }
    }
}
