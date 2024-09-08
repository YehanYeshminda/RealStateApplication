using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblNotification
    {
        public int Id { get; set; }
        public int Notify { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; } = null!;
        public string Message { get; set; } = null!;
        public int Priorityid { get; set; }
        public int Addby { get; set; }
        public DateTime Addon { get; set; }
        public string Forwardto { get; set; } = null!;
        public DateTime Snoozeon { get; set; }
        public DateTime From { get; set; }
        public int Status { get; set; }
    }
}
