using ComparisonShoppingEngineAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Dtos.Product
{
    public class GetProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public string ProductUnit { get; set; }

        public Dictionary<string, decimal> ShopPrices { get; set; }
    }
}
