using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblPropertyAssign
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Spid { get; set; }
        public int Propertyid { get; set; }
        public string Custcode { get; set; } = null!;
        public DateTime Validtill { get; set; }
        public string Advnote { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Status { get; set; }
        public string Addby { get; set; } = null!;
        public DateTime Addon { get; set; }
    }
}
