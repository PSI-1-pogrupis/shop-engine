using ComparisonShoppingEngineAPI.DTOs;
using System;
using System.Collections.Generic;

namespace ComparisonShoppingEngineAPI
{
    public class GetReceiptDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public string Shop { get; set; }
        public ICollection<GetReceiptProductDto> ReceiptProducts { get; set; }
    }
}
