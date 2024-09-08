using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class CompanyDetail
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? Mlname { get; set; }
        public string RegNo { get; set; } = null!;
        public string VatNo { get; set; } = null!;
        public string? Address { get; set; }
        public string Phone { get; set; } = null!;
        public string? Mobile { get; set; }
        public string? Fax { get; set; }
        public string Email { get; set; } = null!;
        public string? Website { get; set; }
        public byte[]? Logo { get; set; }
        public int Status { get; set; }
        public string? TaxMethod { get; set; }
    }
}
