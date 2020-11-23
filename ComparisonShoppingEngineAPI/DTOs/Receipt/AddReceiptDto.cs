using System.Collections.Generic;
using ComparisonShoppingEngineAPI.DTOs;

namespace ComparisonShoppingEngineAPI.DTOs
{
    public class AddReceiptDto
    {
        public int ShopId { get; set; }
        public decimal Total { get; set; }
        public IEnumerable<AddReceiptProductDto> Products { get; set; }
    }
}
