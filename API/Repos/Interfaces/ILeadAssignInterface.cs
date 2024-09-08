using API.Models;

namespace API.Repos.Interfaces
{
    public interface ILeadAssignInterface
    {
        Task<TblleadAssign> GetLeadAssignsByStaffId(int staffId, string leadId);
        Task<IEnumerable<TblleadAssign>> GetLeadAssigningsByStaff(int staffId);
        Task<TblleadAssign> GetLeadAssigningsByPrimartId(int id);
    }
}
