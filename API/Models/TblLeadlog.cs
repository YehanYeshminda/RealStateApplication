using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblLeadlog
    {
        public int Id { get; set; }
        public string Leadid { get; set; } = null!;
        public string Log { get; set; } = null!;
        public int Addby { get; set; }
        public DateTime Addon { get; set; }
    }
}
