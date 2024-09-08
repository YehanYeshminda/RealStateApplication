using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class VPurchasePayment
    {
        public string PaymentScheduleNo { get; set; } = null!;
        public DateTime Date { get; set; }
        public string? SupplierName { get; set; }
        public string Reason { get; set; } = null!;
        public string Rxpaccount { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime Paidon { get; set; }
        public string Renewevery { get; set; } = null!;
        public string Renewstatus { get; set; } = null!;
        public int Status { get; set; }
    }
}
