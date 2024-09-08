namespace API.Repos.Dtos.CallCenterDtos
{
    public class UpdateScheduleForLeadDto
    {
        public AuthDto AuthDto { get; set; }
        public string LeadNo { get; set; }
        public DateTime ScheuledTime { get; set; }
        public string Description { get; set; }
        public int AssignedStaff { get; set; }
        public string OriginalTime { get; set; }
        public DateTime OriginalDate { get; set; }
    }
}
