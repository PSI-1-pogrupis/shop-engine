using ComparisonShoppingEngineAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Dtos.Product
{
    public class AddProductDto
    {
        public string Name { get; set; }

        public string Unit { get; set; }

        public Dictionary<string, decimal> ShopPrices { get; set; }
    }
}
