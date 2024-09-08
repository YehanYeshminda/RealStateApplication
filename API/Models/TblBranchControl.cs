using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblBranchControl
    {
        public int Cid { get; set; }
        public int BrId { get; set; }
        public int CustomerId { get; set; }
        public int VoucherNo { get; set; }
        public int InvoiceNo { get; set; }
        public int ExpVou { get; set; }
        public int Ppno { get; set; }
        public int RoomAdv { get; set; }
        public int ChqId { get; set; }
        public int AdvRec { get; set; }
        public int OtherIncome { get; set; }
        public int ReturnNo { get; set; }
    }
}
