using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tbluser
    {
        public int Userid { get; set; }
        public string? Username { get; set; }
        public string? Loginname { get; set; }
        public string? Firstname { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime? Logintime { get; set; }
        public DateTime? Logouttime { get; set; }
        public decimal? Fullaccess { get; set; }
        public int Superuser { get; set; }
        public string? Password { get; set; }
        public int? Cid { get; set; }
        public decimal Status { get; set; }
        public decimal Openbalance { get; set; }
        public decimal Discount { get; set; }
        public int Ud { get; set; }
        public string? Usercode { get; set; }
        public string? Hash { get; set; }
        public string? Userimage { get; set; }
        public string? Passport { get; set; }
        public DateTime? VisaIssuedate { get; set; }
        public DateTime? VisaExpiredate { get; set; }
    }
}
