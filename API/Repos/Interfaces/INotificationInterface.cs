using API.Models;

namespace API.Repos.Interfaces
{
    public interface INotificationInterface
    {
        Task<IEnumerable<TblNotification>> GetAllNotificationForUser(string staffId);
        Task<IEnumerable<dynamic>> GetAllNotificationForUserView(string staffId);
        Task<DateTime?> GetLatestSnoozeonTime();
        Task<IEnumerable<TblNotification>> GetAllNotification();
        Task<int> UpdateNotiticationCount(string userId);
        Task<TblNotification> GetNotificationByIdAsync(int id);
    }
}
