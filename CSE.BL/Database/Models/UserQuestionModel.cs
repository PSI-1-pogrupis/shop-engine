using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.Database.Models
{
    public partial class UserQuestionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Question { get; set; }
    }
}
