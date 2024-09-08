namespace API.Repos.Dtos;

public class GetAllNotificationAll
{
    public AuthDto authDto { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}