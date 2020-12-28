using ComparisonShoppingEngineAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.DTOs.Optimization
{
    public class OptimizedShopListDto
    {
        public string Shop { get; set; }
        public List<ShoppingItem> ItemList { get;  set; }
        public decimal ListPrice { get; set; } = 0;
        public bool HasAllProducts { get; set; } = true;

    }
}
