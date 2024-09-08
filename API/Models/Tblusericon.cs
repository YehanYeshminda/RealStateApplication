using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblusericon
    {
        public int Id { get; set; }
        public string? UserIcon { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
    }
}
