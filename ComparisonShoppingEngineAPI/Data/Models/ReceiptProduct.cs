using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Models
{
    public class ReceiptProduct
    {
        [Key]
        public int ReceiptProductId { get; set; }
        public int ReceiptId { get; set; }
        public int ProductId { get; set; }
        public decimal ProductPrice { get; set; }
        public virtual Product Product { get; set; }
    }
}
