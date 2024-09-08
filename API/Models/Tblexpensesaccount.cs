using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tblexpensesaccount
    {
        public int Id { get; set; }
        public string MainCatId { get; set; } = null!;
        public string SubCatId { get; set; } = null!;
        public int Status { get; set; }
    }
}
