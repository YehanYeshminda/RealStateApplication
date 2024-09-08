using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblDndLead
    {
        public int Id { get; set; }
        public string? LeadId { get; set; }
        public DateTime? AddOn { get; set; }
        public string? LeadName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Otherno { get; set; }
        public int? Assigned { get; set; }
        public int? Staffid { get; set; }
        public DateTime? Recievedon { get; set; }
        public DateTime? Assignon { get; set; }
        public int? Importance { get; set; }
        public int? Called { get; set; }
        public int? Status { get; set; }
        public int? ContactMethod { get; set; }
        public int? CrossSegmentLead { get; set; }
        public string? Comments { get; set; }
        public int? IsInterested { get; set; }
        public DateTime? AddedOn { get; set; }
    }
}
