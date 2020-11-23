using System.ComponentModel.DataAnnotations;

namespace ComparisonShoppingEngineAPI.Data.Models
{
    public class ReceiptProduct
    {
        [Key]
        public int ReceiptProductId { get; set; }
        public int ReceiptId { get; set; }
        public int ProductId { get; set; }
        public decimal ProductPrice { get; set; }
        public double Amount { get; set; }
        public virtual Product Product { get; set; }
    }
}
