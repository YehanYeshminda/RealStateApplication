using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblcompanydetail
    {
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? Mlname { get; set; }
        public string? RegNo { get; set; }
        public string? VatNo { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Logo { get; set; }
        public int? Status { get; set; }
        public string? TaxMethod { get; set; }
    }
}
