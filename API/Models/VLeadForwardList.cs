using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class VLeadForwardList
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Leadid { get; set; } = null!;
        public string? ForwardTo { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime Addon { get; set; }
        public string? ForwardFrom { get; set; }
        public string Name { get; set; } = null!;
    }
}
