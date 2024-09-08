using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblreccheque
    {
        public string Chqid { get; set; } = null!;
        public string? Chqno { get; set; }
        public string? Customercode { get; set; }
        public string? Invno { get; set; }
        public string? Bankid { get; set; }
        public string? Branchid { get; set; }
        public decimal Amount { get; set; }
        public DateTime? Bankdate { get; set; }
        public string? Description { get; set; }
        public int? Cid { get; set; }
        public int Status { get; set; }
        public int? Rtype { get; set; }
        public DateTime? Rdate { get; set; }
        public decimal Used { get; set; }
        public decimal Deposit { get; set; }
        public decimal Ret { get; set; }
        public string? BrId { get; set; }
        public DateTime Addon { get; set; }
        public int Addby { get; set; }
    }
}
