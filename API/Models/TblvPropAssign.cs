using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblvPropAssign
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? TypeName { get; set; }
        public string? CustName { get; set; }
        public DateTime Validtill { get; set; }
        public string Advnotno { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Addon { get; set; }
        public string? Username { get; set; }
    }
}
