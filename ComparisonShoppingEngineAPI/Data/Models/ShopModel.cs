using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Models
{
    public class ShopModel
    {
        public ShopModel()
        {
            ShopProduct = new HashSet<ShopProductModel>();
        }

        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public virtual ICollection<ShopProductModel> ShopProduct { get; set; }
    }
}
