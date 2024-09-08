using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblvendertoservice
    {
        public int Id { get; set; }
        public int Venderid { get; set; }
        public int Serviceid { get; set; }
        public int Status { get; set; }
        public int Addby { get; set; }
        public DateTime Addon { get; set; }
    }
}
