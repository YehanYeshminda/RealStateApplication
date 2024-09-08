using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class VExepnsesAccount
    {
        public int Id { get; set; }
        public string MainCatId { get; set; } = null!;
        public string? MainCategory { get; set; }
        public string SubCatId { get; set; } = null!;
        public string? SubCategory { get; set; }
        public int Status { get; set; }
    }
}
