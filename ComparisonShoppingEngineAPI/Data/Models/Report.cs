using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Problem { get; set; }
        public DateTime? Date { get; set; }
    }
}
