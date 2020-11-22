using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.DTOs
{
    public class ScannedItemDto
    {
        public string Name { get; set; }

        public string Shop { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public decimal PricePerQuantity { get; set; }
    }
}
