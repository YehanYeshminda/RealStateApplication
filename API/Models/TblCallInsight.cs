using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblCallInsight
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNo { get; set; }
        public string? PhoneNo2 { get; set; }
        public string? AssignedTo { get; set; }
        public DateTime? AddOn { get; set; }
        public DateTime? CalledOn { get; set; }
        public DateTime? CallEndedOn { get; set; }
        public int? Status { get; set; }
        public DateTime? AssignedOn { get; set; }
    }
}
