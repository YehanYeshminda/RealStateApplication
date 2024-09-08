using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblleadlogview
    {
        public int Id { get; set; }
        public string Leadid { get; set; } = null!;
        public string Log { get; set; } = null!;
        public DateTime Addon { get; set; }
        public string? Addby { get; set; }
    }
}
