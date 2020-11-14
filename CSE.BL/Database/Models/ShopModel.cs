using System.Collections.Generic;

namespace CSE.BL.Database.Models
{
    public partial class ShopModel
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
