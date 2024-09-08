using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblvExpense
    {
        public string Id { get; set; } = null!;
        public DateTime? VDate { get; set; }
        public string? SupplierName { get; set; }
        public string? MainCategory { get; set; }
        public string? Description { get; set; }
        public decimal CashPaid { get; set; }
        public decimal ChequePaid { get; set; }
        public string? ChequeNo { get; set; }
        public string? Cid { get; set; }
        public decimal Status { get; set; }
        public string? Username { get; set; }
        public string ReceiptNo { get; set; } = null!;
        public DateTime? RDate { get; set; }
        public string? BranchName { get; set; }
        public decimal TotalValue { get; set; }
        public decimal Vatp { get; set; }
        public decimal Vat { get; set; }
        public decimal NetTotal { get; set; }
        public decimal Paid { get; set; }
        public int? AccountId { get; set; }
        public string? SubCategory { get; set; }
    }
}
