using System.Collections.Generic;

namespace ComparisonShoppingEngineAPI.DTOs
{
    public class AddReceiptDto
    {
        public string Shop { get; set; }
        public decimal Total { get; set; }
        public IEnumerable<AddReceiptProductDto> Products { get; set; }
    }
}
