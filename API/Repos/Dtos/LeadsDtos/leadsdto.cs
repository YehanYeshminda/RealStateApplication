namespace API.Repos.Dtos.LeadsDtos
{
    public class leadsdto
    {
        public AuthDto AuthDto { get; set; }
        public string Leadno { get; set; } = null!;
        public int Sourceid { get; set; }
        public int Status { get; set; }
        public string? Campainid { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Otherno { get; set; } = null!;
        public int? AssignedTo { get; set; }
        public int? LeadStatus { get; set; }
        public int ContactMethod { get; set; }
        public string? Comment { get; set; }
    }
}
