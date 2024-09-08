using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblLeadStatus
    {
        public int Id { get; set; }
        public string Leadstatus { get; set; } = null!;
        public string Remark { get; set; } = null!;
        public int Status { get; set; }
    }
}
