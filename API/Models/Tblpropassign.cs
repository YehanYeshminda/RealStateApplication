using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblpropassign
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Salesperson { get; set; }
        public int Customerid { get; set; }
        public DateTime Validtill { get; set; }
        public string Advnotno { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Addon { get; set; }
        public int Addby { get; set; }
    }
}
