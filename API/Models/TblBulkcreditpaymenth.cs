using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblBulkcreditpaymenth
    {
        public string VoucherNo { get; set; } = null!;
        public DateTime? Date1 { get; set; }
        public string CustomerCode { get; set; } = null!;
        public decimal CashPaid { get; set; }
        public string? ChequeId { get; set; }
        public decimal ChequePaid { get; set; }
        public int? Rtnid { get; set; }
        public decimal Rtnpaid { get; set; }
        public decimal Balance { get; set; }
        public int UserId { get; set; }
        public int Cid { get; set; }
        public int? Status { get; set; }
        public DateTime? RDate { get; set; }
        public int? BrId { get; set; }
    }
}
