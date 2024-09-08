using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblIou
    {
        public int Id { get; set; }
        public int Branchid { get; set; }
        public DateTime Date { get; set; }
        public int Issueto { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime Returnon { get; set; }
        public int Approvedby { get; set; }
        public decimal Value { get; set; }
        public int Returned { get; set; }
        public int Addby { get; set; }
        public DateTime Addon { get; set; }
    }
}
