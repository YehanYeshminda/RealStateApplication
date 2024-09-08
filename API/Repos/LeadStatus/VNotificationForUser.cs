namespace API.Repos.LeadStatus;

public class VNotificationForUser
{
    public string Notify { get; set; }
    public DateTime Date { get; set; }
    public string Time { get; set; }
    public string Message { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}