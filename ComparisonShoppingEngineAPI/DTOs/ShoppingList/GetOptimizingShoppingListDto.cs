using ComparisonShoppingEngineAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.DTOs.ShoppingList
{
    public class GetOptimizingShoppingListDto
    {
        public List<GetShoppingItemDto> ShoppingList { get; set; }

        public List<string> AllowedShops { get; set; }

        public bool OnlyReplaceUnspecifiedShops { get; set; }
    }
}
