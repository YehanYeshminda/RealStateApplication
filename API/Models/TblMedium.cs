using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblMedium
    {
        public int Id { get; set; }
        public string? Media { get; set; }
        public string? Remark { get; set; }
        public int? Status { get; set; }
        public int? Cid { get; set; }
    }
}
