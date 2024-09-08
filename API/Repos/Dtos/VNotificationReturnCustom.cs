namespace API.Repos.Dtos;

public class VNotificationReturnCustom
{
    public int Id { get; set; }
    public string FromFirstName { get; set; }
    public string FromLastName { get; set; }
    public DateTime DateAdded { get; set; }
    public string Time { get; set; }
    public string Message { get; set; }
    public string PriorityId { get; set; }
    public string AddBy { get; set; }
    public DateTime AddonTime { get; set; }
    public string ForwardTo { get; set; }
    public DateTime SnoozeOn { get; set; }
    public DateTime FromTime { get; set; }
}