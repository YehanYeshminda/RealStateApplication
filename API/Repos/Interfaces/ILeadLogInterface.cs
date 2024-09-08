using API.Models;

namespace API.Repos.Interfaces
{
    public interface ILeadLogInterface
    {
        Task<IEnumerable<TblLeadlog>> GetAllLeadLogs();
        Task<IEnumerable<TblLeadlog>> GetLeadLogByLeadId(string id);
        Task AddNewLeadLogAsync(TblLeadlog leadlog);
        int AddLog(TblLeadlog tblLeadlog);
    }
}
