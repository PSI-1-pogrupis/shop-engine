using System.Collections.Generic;

namespace ComparisonShoppingEngineAPI.DTOs
{
    public class AddReceiptDto
    {
        public IEnumerable<AddReceiptProductDto> Products { get; set; }
    }
}
