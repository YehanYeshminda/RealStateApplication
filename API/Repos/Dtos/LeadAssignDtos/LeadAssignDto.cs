namespace API.Repos.Dtos.LeadAssignDtos
{
    public class LeadAssignDeleteDto
    {
        public AuthDto AuthDto { get; set; }
        public string Lead { get; set; }
        public int Staff { get; set; }
        public string Remark { get; set; }
        public int? Id { get; set; }
    }

    public class LeadAssignDto
    {
        public AuthDto AuthDto { get; set; }
        public List<string> Leadid { get; set; }
        public int Staffid { get; set; }
        public string Remark { get; set; }
    }
}
