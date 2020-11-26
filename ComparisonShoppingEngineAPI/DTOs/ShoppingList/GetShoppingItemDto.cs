using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.DTOs.ShoppingList
{
    public class GetShoppingItemDto
    {
        public string Name { get; set; }

        public double Amount { get; set; }

        public string Shop { get; set; }
    }
}
