using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblCustomerRequirement
    {
        public int Id { get; set; }
        public int Requirementid { get; set; }
        public int Custid { get; set; }
        public int Addby { get; set; }
        public DateTime Addon { get; set; }
        public int Status { get; set; }
    }
}
