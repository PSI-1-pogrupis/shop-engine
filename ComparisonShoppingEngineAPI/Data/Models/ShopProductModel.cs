using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Models
{
    public class ShopProductModel
    {
        public int ShopProductId { get; set; }
        public int ProductId { get; set; }
        public int? ShopId { get; set; }
        public decimal Price { get; set; }
        public DateTime? Date { get; set; }

        public virtual ProductModel Product { get; set; }
        public virtual ShopModel Shop { get; set; }
    }
}
