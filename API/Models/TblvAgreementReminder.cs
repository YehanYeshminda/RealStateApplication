using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblvAgreementReminder
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? CustName { get; set; }
        public string? TypeName { get; set; }
        public DateTime Enddate { get; set; }
        public DateTime Remindon { get; set; }
        public string Remarks { get; set; } = null!;
        public int Status { get; set; }
        public string? Username { get; set; }
        public DateTime Addon { get; set; }
    }
}
