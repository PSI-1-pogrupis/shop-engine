using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Models
{
    public class ProductModel
    {
        public ProductModel()
        {
            ShopProduct = new HashSet<ShopProductModel>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        public virtual ICollection<ShopProductModel> ShopProduct { get; set; }
    }
}
