using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblSupportStaff
    {
        public int Id { get; set; }
        public int Meetid { get; set; }
        public int Staffid { get; set; }
        public int Addby { get; set; }
        public DateTime Addon { get; set; }
    }
}
