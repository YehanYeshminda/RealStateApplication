using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblvNotification
    {
        public int Id { get; set; }
        public string? Notify { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; } = null!;
        public string Message { get; set; } = null!;
        public int Priorityid { get; set; }
        public string? Forwardto { get; set; }
        public string? Username { get; set; }
        public DateTime Addon { get; set; }
        public DateTime Snoozeon { get; set; }
        public DateTime From { get; set; }
        public string ForwardId { get; set; } = null!;
    }
}
