using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblCampainMedium
    {
        public int Id { get; set; }
        public string Campainno { get; set; } = null!;
        public string Mediaid { get; set; } = null!;
        public int Addby { get; set; }
        public DateTime Addon { get; set; }
    }
}
