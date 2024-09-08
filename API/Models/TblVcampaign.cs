using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblVcampaign
    {
        public string No { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Datefrom { get; set; }
        public DateTime Dateto { get; set; }
        public string Description { get; set; } = null!;
        public string Totalcost { get; set; } = null!;
        public string Remarks { get; set; } = null!;
        public int Status { get; set; }
        public string? Username { get; set; }
        public DateTime Addon { get; set; }
    }
}
