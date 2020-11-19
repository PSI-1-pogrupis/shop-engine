using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Models
{
    public class ShopProductEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? ShopId { get; set; }
        public decimal Price { get; set; }
        public DateTime? Date { get; set; }

        public virtual ProductEntity Product { get; set; }
        public virtual ShopEntity Shop { get; set; }
    }
}
