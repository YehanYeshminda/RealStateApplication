using API.Models;
using API.Repos.Dtos.CommonDto;

namespace API.Repos.Interfaces
{
    public interface IStaffInterface
    {
        Task<IEnumerable<Tblstaff>> GetAllStaff();
        List<CommonDto> GetAllStaffList();
        Task<IEnumerable<dynamic>> GetAllVStaff();
        Task<Tblstaff> GetStaffByIdAsync(int id);
        List<TblCallInsight> GetStaffAssignedCallsToday(int staffId);
        int RemoveAssignedCalls(int staffId, int count);
    }
}
