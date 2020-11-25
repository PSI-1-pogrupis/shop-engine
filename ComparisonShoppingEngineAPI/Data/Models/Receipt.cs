using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComparisonShoppingEngineAPI.Data.Models
{
    public class Receipt
    {
        public Receipt()
        {
            ReceiptProducts = new HashSet<ReceiptProduct>();
        }
        [Key]
        public int ReceiptId { get; set; }
        public int UserId { get; set; }
        public int ShopId { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<ReceiptProduct> ReceiptProducts { get; set; }
        public virtual Shop Shop { get; set; }
    }
}
