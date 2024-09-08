using API.Models;

namespace API.Repos.Interfaces
{
    public interface IMeetingInterface
    {
        Task<IEnumerable<TblMeeting>> GetAllMeeting();
        Task<IEnumerable<dynamic>> GetAllVMeeting();
        Task<IEnumerable<TblMeeting>> GetAllMeetingByUserId(int id);
        Task<TblMeeting> GetMeetingByIdAsync(int id);
    }
}
