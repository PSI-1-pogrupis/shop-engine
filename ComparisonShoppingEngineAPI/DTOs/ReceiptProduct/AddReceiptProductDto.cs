namespace ComparisonShoppingEngineAPI.DTOs
{
    public class AddReceiptProductDto
    {
        public string Name { get; set; }
        public string Shop { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal PricePerQuantity { get; set; }
    }
}
