using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblpropertyCategory
    {
        public int Id { get; set; }
        public string? PropertyCat { get; set; }
        public string? Remark { get; set; }
        public int? Cid { get; set; }
        public decimal Status { get; set; }
    }
}
