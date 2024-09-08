using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblcurrentacc
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? BankCode { get; set; }
        public string? BranchCode { get; set; }
        public string? Instruction { get; set; }
        public string? AccNo { get; set; }
        public decimal Balance { get; set; }
        public int? Cid { get; set; }
        public decimal Status { get; set; }
        public decimal RetDed { get; set; }
        public DateTime? Date1 { get; set; }
    }
}
