namespace API.Repos.Dtos.LeadlogDtos
{
    public class LeadlogDto
    {
        public AuthDto AuthDto { get; set; }
        public int Id { get; set; }
        public string Leadid { get; set; } = null!;
        public string Log { get; set; } = null!;
        public string Addby { get; set; } = null!;
        public DateTime Addon { get; set; }
    }
}
