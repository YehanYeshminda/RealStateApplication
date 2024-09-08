namespace API.Repos.Dtos.LeadAssignDtos
{
    public class LeadDependingOnStaffDto
    {
        public int Id { get; set; }
        public string Lead { get; set; }
        public string Staff { get; set; }
        public string AddBy { get; set; }
        public DateTime AddOn { get; set; }
        public int Status { get; set; }
    }
}
