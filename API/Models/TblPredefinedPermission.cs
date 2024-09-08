using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblPredefinedPermission
    {
        public int Id { get; set; }
        public string? Role { get; set; }
        public string? Module { get; set; }
        public string? Event { get; set; }
        public int? HasPermission { get; set; }
        public int? Status { get; set; }
    }
}
