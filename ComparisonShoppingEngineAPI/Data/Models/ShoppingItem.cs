using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Models
{
    public class ShoppingItem
    {
        public string Name { get; set; }

        public string Unit { get; set; }

        public double Amount { get; set; }

        public string Shop { get; set; }

        public decimal PricePerUnit{ get; set; }
    }
}
