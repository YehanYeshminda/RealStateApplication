using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblSupplier
    {
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public int? CreditPeriod { get; set; }
        public int? Staff { get; set; }
        public decimal Status { get; set; }
        public string? Cid { get; set; }
        public string? VatNo { get; set; }
    }
}
