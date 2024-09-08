using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblAgreementtype
    {
        public int TypeId { get; set; }
        public string? TypeName { get; set; }
        public string? Remark { get; set; }
        public int? Status { get; set; }
        public int? Cid { get; set; }
    }
}
