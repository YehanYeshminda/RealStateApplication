using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblIourtn
    {
        public int Rtnid { get; set; }
        public int Iouid { get; set; }
        public DateTime Retnon { get; set; }
        public int Brid { get; set; }
        public DateTime Addon { get; set; }
        public int Addby { get; set; }
        public string? Desc { get; set; }
    }
}
