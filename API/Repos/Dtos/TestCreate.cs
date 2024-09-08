namespace API.Repos.Dtos
{
    public class TestCreate
    {
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
