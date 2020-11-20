using System.Collections.Generic;

namespace ComparisonShoppingEngineAPI.DTOs.Product
{
    public class ProductDto
    {
        public string Name { get; set; }

        public string Unit { get; set; }

        public Dictionary<string, decimal> ShopPrices { get; set; }
    }
}
