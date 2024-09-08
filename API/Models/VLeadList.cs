using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class VLeadList
    {
        public string Leadno { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Otherno { get; set; } = null!;
        public DateTime Recievedon { get; set; }
        public DateTime Assignon { get; set; }
        public int Called { get; set; }
        public int Status { get; set; }
        public string? Source { get; set; }
        public string? Staffname { get; set; }
        public string? ContactMethod { get; set; }
        public string? Leadstatus { get; set; }
        public string Campainid { get; set; } = null!;
        public string? Comments { get; set; }
        public int Importance { get; set; }
    }
}
