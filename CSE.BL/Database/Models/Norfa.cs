using System;

namespace CSE.BL.Database.Models
{
    public partial class Norfa
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public double Price { get; set; }
        public DateTime? Date { get; set; }
    }
}
