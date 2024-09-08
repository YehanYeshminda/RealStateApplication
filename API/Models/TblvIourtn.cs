using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblvIourtn
    {
        public int Rtnid { get; set; }
        public int Iouid { get; set; }
        public DateTime Retnon { get; set; }
        public string? BranchName { get; set; }
        public DateTime Addon { get; set; }
        public string? Username { get; set; }
        public string? Desc { get; set; }
    }
}
