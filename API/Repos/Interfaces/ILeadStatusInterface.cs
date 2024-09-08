using API.Models;

namespace API.Repos.Interfaces
{
    public interface ILeadStatusInterface
    {
        Task<IEnumerable<TblLeadStatus>> GetAllLeadStatus();
        Task<TblLeadStatus> GetLeadStatusBYId(int id);
        Task<TblLeadStatus> GetLeadStatusByName(string name);
    }
}
