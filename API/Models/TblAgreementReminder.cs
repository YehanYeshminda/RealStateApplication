using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblAgreementReminder
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Custcode { get; set; }
        public int Agreementtype { get; set; }
        public DateTime Enddate { get; set; }
        public DateTime Remindon { get; set; }
        public string Remarks { get; set; } = null!;
        public int Status { get; set; }
        public int Addby { get; set; }
        public DateTime Addon { get; set; }
    }
}
