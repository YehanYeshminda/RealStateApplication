using API.Repos.Dtos;
using API.Repos.LeadStatus;

namespace API.Repos.Notification;

public interface INotification
{
    List<VNotificationForUser> GetAllNotificationsForUser(int id, DateTime startDate, DateTime endDate);
    List<VNotificationReturnCustom> GetAllNotificationsForUserForAll(bool isAdmin, int? id);
}