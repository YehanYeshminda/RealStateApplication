namespace API.Repos.Dtos.LeadsDtos;

public class VLeads
{
    public string LeadNo { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string OtherNo { get; set; }
    public DateTime ReceivedOn { get; set; }
    public DateTime AssignOn { get; set; }
    public int Called { get; set; }
    public int Status { get; set; }
    public string Source { get; set; }
    public string StaffName { get; set; }
    public string ContactMethod { get; set; }
    public string LeadStatus { get; set; }
    public string CampaignId { get; set; }
    public string Comments { get; set; }
    public int Importance { get; set; }
}