using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblCusttoRequirment
    {
        public int Id { get; set; }
        public int Requirementid { get; set; }
        public int Custid { get; set; }
        public string Addby { get; set; } = null!;
        public DateTime Addon { get; set; }
    }
}
