namespace API.Repos.Dtos.ArchivedLead;

public class ArchivedLeadGetAllDto
{
    public AuthDto authDto { get; set; }
    public int PageSize { get; set; }
    public int Page { get; set; }
}