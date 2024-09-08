using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class VVendorToServiceList
    {
        public int Id { get; set; }
        public string? SupplierName { get; set; }
        public string? TypeName { get; set; }
        public int Status { get; set; }
        public DateTime Addon { get; set; }
        public string? AddBy { get; set; }
    }
}
