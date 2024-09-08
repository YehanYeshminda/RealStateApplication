using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tbluserlevel
    {
        public int Id { get; set; }
        public string? Userlevel { get; set; }
        public string? Remark { get; set; }
        public int Status { get; set; }
        public string? Cid { get; set; }
    }
}
