using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblbranch
    {
        public int Cid { get; set; }
        public int BrId { get; set; }
        public string? BranchName { get; set; }
        public string? PrintTitle { get; set; }
        public string? PrintAddress { get; set; }
        public string Phone { get; set; } = null!;
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public int Status { get; set; }
        public decimal CashBook { get; set; }
        public string? Brcode { get; set; }
        public decimal CashBookH { get; set; }
    }
}
