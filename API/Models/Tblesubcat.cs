using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblesubcat
    {
        public int Id { get; set; }
        public string? SubCategory { get; set; }
        public string? Remark { get; set; }
        public int? Cid { get; set; }
        public decimal Status { get; set; }
    }
}
