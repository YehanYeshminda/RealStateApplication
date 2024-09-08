using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblVstaff
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? VisaIssuedate { get; set; }
        public string Name { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string Mobileno { get; set; } = null!;
        public string? Parentid { get; set; }
        public string? Addby { get; set; }
        public DateTime Addon { get; set; }
        public int Status { get; set; }
        public string? Userid { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Userimage { get; set; }
        public string? Passport { get; set; }
    }
}
