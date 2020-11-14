﻿using System.Collections.Generic;

namespace CSE.BL.Database.Models
{
    public partial class ProductModel
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
