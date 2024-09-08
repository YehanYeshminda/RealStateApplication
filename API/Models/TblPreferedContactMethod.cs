using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblPreferedContactMethod
    {
        public int Id { get; set; }
        public string? ContactMethod { get; set; }
        public string? Remark { get; set; }
        public int? Status { get; set; }
        public int? Cid { get; set; }
    }
}
