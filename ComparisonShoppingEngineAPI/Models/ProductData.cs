using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Models
{
    public class ProductData
    {
        public string Name { get; set; }

        public string Unit { get; set; }

        public Dictionary<string, decimal> ShopPrices { get; set; }
    }
}
