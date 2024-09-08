using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblleadAssign
    {
        public int Id { get; set; }
        public string Leadid { get; set; } = null!;
        public int Staffid { get; set; }
        public int Addby { get; set; }
        public DateTime Addon { get; set; }
        public int Status { get; set; }
    }
}
