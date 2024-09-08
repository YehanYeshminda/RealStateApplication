using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblstaff
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string Mobileno { get; set; } = null!;
        public int Parentid { get; set; }
        public int Addby { get; set; }
        public DateTime Addon { get; set; }
        public int Status { get; set; }
        public int Userid { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public int? MonthlyTarget { get; set; }
        public int? CallsMonthlyTarget { get; set; }
    }
}
