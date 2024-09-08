using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblBulkcreditpaymentd
    {
        public string? VoucherNo { get; set; }
        public string? InvoiceNo { get; set; }
        public DateTime? InvDate { get; set; }
        public decimal Credit { get; set; }
        public decimal CashPay { get; set; }
        public decimal ChequePay { get; set; }
        public decimal Rtnnote { get; set; }
        public int Status { get; set; }
        public decimal Advance { get; set; }
    }
}
