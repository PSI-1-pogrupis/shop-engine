namespace ComparisonShoppingEngineAPI.DTOs
{
    public class GetReceiptProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal PricePerQuantity { get; set; }
    }
}
