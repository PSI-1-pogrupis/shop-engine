using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Models
{
    public class ShopEntity
    {
        public ShopEntity()
        {
            ShopProducts = new HashSet<ShopProductEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ShopProductEntity> ShopProducts { get; set; }
    }
}
