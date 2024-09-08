using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblcompany
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? Web { get; set; }
        public int Status { get; set; }
        public decimal CashBookH { get; set; }
        public decimal ServiceCharge { get; set; }
        public int AutoBulkInvoice { get; set; }
        public string? BarcodeTitle { get; set; }
        public string? CompanyLogo { get; set; }
    }
}
