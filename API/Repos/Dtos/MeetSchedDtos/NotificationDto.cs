namespace API.Repos.Dtos.MeetSchedDtos
{
    public class NotificationDto
    {
        public AuthDto AuthDto { get; set; }
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; } = null!;
        public string Message { get; set; } = null!;
        public int Priorityid { get; set; }
        public string Addby { get; set; } = null!;
        public DateTime Addon { get; set; }
        public string Forwardto { get; set; } = null!;
        public string Snoozeon { get; set; } = null!;
    }
}
