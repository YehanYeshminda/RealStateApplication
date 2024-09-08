using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblemaincat
    {
        public int Id { get; set; }
        public string? MainCategory { get; set; }
        public string? Remark { get; set; }
        public int? Cid { get; set; }
        public decimal Status { get; set; }
    }
}
