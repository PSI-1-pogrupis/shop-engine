using System;

namespace CSE.BL.Database.Models
{
    public partial class ShopProductModel
    {
        public int Id { get; set; }
        public string ShopName { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public DateTime? Date { get; set; }
    }
}
