using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblLeadforward
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Leadid { get; set; } = null!;
        public string Forwardstaffid { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public string Forwardfromid { get; set; } = null!;
        public int Addby { get; set; }
        public DateTime Addon { get; set; }
    }
}
