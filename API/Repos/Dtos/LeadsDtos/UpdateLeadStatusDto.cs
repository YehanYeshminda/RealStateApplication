namespace API.Repos.Dtos.LeadsDtos;

public class UpdateLeadStatusDto
{
    public AuthDto AuthDto { get; set; }
    public int Status { get; set; }
    public string leadNo { get; set; }
}