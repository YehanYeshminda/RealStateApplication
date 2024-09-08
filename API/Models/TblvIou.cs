using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblvIou
    {
        public int Id { get; set; }
        public string? BranchName { get; set; }
        public DateTime Date { get; set; }
        public string? TypeName { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime Returnon { get; set; }
        public string? Username { get; set; }
        public decimal Value { get; set; }
        public int Returned { get; set; }
        public DateTime Addon { get; set; }
        public int Addby { get; set; }
    }
}
