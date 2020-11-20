using System.Collections.Generic;

namespace ComparisonShoppingEngineAPI.DTOs.Product
{
    public class ProductDto
    {
        public string ProductName { get; set; }

        public string ProductUnit { get; set; }

        public Dictionary<string, decimal> ShopPrices { get; set; }
    }
}
