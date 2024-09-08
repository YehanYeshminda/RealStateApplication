namespace API.Repos.Dtos.CallCenterDtos
{
    public class UpdateLeadCallCenterDto
    {
        public AuthDto AuthDto { get; set; }
        public int LeadStatus { get; set; }
        public string LeadNo { get; set; }
        public string Remark { get; set; }
    }
}
