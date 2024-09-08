using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class VPropertyDevelopmentList
    {
        public string Id { get; set; } = null!;
        public DateTime Date { get; set; }
        public string PropertyName { get; set; } = null!;
        public string? SupplierName { get; set; }
        public string Propertyno { get; set; } = null!;
        public int Expenseaccount { get; set; }
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal Cashpaid { get; set; }
        public decimal Banktransfer { get; set; }
        public string? BankCode { get; set; }
        public decimal Chequepaid { get; set; }
        public string? ChequeId { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime Addon { get; set; }
        public string? AddBy { get; set; }
    }
}
