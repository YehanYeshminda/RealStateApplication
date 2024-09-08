using System;
using System.Collections.Generic;

namespace API.Models
{
    public partial class TblMeeting
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Date { get; set; }
        public int Staffid { get; set; }
        public string Reason { get; set; } = null!;
        public int Custid { get; set; }
        public DateTime Meetdate { get; set; }
        public string Meettime { get; set; } = null!;
        public string Venue { get; set; } = null!;
        public string Remarks { get; set; } = null!;
        public int Addby { get; set; }
        public DateTime Addon { get; set; }
        public int Status { get; set; }
        public string Conclusion { get; set; } = null!;
    }
}
