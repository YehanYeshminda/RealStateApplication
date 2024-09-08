using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblStaffperformance
    {
        public int Id { get; set; }
        public string? StaffName { get; set; }
        public int? LeadConvertedCount { get; set; }
        public int? CallMadeCount { get; set; }
        public int? MeetingsPlanned { get; set; }
    }
}
