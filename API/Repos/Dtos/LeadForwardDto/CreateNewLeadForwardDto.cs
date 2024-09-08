namespace API.Repos.Dtos.LeadForwardDto
{
    public class CreateNewLeadForwardDto
    {
        public AuthDto AuthDto { get; set; }
        public DateTime Date { get; set; }
        public string Leadid { get; set; } = null!;
        public int Forwardstaffid { get; set; }
        public string Reason { get; set; } = null!;
    }

    public class EditExistingLeadForwardDto
    {
        public AuthDto AuthDto { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Leadid { get; set; } = null!;
        public int Forwardstaffid { get; set; }
        public string Reason { get; set; } = null!;
    }
}
