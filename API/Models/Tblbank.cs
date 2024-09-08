using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblbank
    {
        public string BankId { get; set; } = null!;
        public string BankName { get; set; } = null!;
        public int Status { get; set; }
    }
}
