using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblcity
    {
        public int Id { get; set; }
        public string? City { get; set; }
        public string? Remark { get; set; }
        public int? Cid { get; set; }
        public decimal Status { get; set; }
    }
}
