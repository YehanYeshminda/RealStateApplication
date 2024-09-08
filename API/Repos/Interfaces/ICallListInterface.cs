using API.Models;

namespace API.Repos.Interfaces
{
    public interface ICallListInterface
    {
        Task<IEnumerable<TblCallInsight>> GetAllCallList();
        Task<IEnumerable<TblCallInsight>> GetAllCallListByUserId(string userId);
        Task<TblCallInsight> GetAllCallListByEmail(string email);
        Task<TblCallInsight> ExistingCallInsignt(int id);
        Task<TblCallInsight> ExistingCallInsigntByPhone(string phone);
        Task<List<TblCallInsight>> GetTopNCallInsights(int numberOfItems);
        Task<List<TblCallInsight>> GetCallInsightsByDesignation(DateTime startDate, DateTime endDate, string designation, string userId);
        int UpdateCall(TblCallInsight callInsight);
        int UpdateAndSetCalls(int numberOfCalls, int assignedTo, DateTime assignedOn);
        int AssignCallInsight(List<int> values, string staffId);
    }
}
