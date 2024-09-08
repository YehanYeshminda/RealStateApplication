using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblcontrol
    {
        public int Cid { get; set; }
        public int Grnid { get; set; }
        public int PurchaseReturnId { get; set; }
        public int InvoiceNo { get; set; }
        public int PoNo { get; set; }
        public int AdvrptNo { get; set; }
        public int CashMovement { get; set; }
        public int IssueNoteNo { get; set; }
        public int BankTr { get; set; }
        public int? BarCode { get; set; }
        public int? LeadNo { get; set; }
        public int? PaymentScheduleNo { get; set; }
        public int Id { get; set; }
        public int? LeadStats { get; set; }
        public int? CallStats { get; set; }
        public int? CallsLeftStats { get; set; }
    }
}
