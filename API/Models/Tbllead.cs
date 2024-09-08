using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class Tbllead
    {
        public string Leadno { get; set; } = null!;
        public int Sourceid { get; set; }
        public string Campainid { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Otherno { get; set; } = null!;
        public int Assigned { get; set; }
        public int Staffid { get; set; }
        public DateTime Recievedon { get; set; }
        public DateTime Assignon { get; set; }
        public int Importance { get; set; }
        public int Called { get; set; }
        public int Status { get; set; }
        public int? ContactMethod { get; set; }
        public bool? CrossSegmentLead { get; set; }
        public string? Attending { get; set; }
        public int? RsvpTypeId { get; set; }
        public string? Comments { get; set; }
        public string? InterestedIn { get; set; }
        public int? PlanToDo { get; set; }
        public DateTime? PlanToDoWhen { get; set; }
        public int? IsLost { get; set; }
        public DateTime AddedOn { get; set; }
        public int? IsInterested { get; set; }
    }
}
