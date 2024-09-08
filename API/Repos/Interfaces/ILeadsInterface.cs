using API.Models;
using API.Repos.Dtos.LeadsDtos;

namespace API.Repos.Interfaces
{
    public interface ILeadsInterface
    {
        Task<IEnumerable<Tbllead>> GetAllLeads();
        Task<Tbllead> GetLeadsByIdAsync(string id);
        Task<Tbllead> GetLeadsByNameAsync(string name);
        Task<int> GetOpenLeadCount();
        Task<int> GetLeadCount();
        Task<int> GetConversionCount();
        Task<int> GetHotLeadCount();
        Task<IEnumerable<dynamic>> GetLeadListView();
        Task<IEnumerable<dynamic>> GetLeadListViewForNew();
        Task<IEnumerable<dynamic>> GetAllLeadsSqlRaw();
        Task<Dictionary<string, int>> GetAllLeadsOnWeekDaysSqlRaw(string staffId);
        Task<Dictionary<string, int>> GetAssignedCallsOnWeekDaysSqlRaw(string staffId);
        Task<Dictionary<string, double>> GetAverageCallDurationByDayOfWeek(string staffId);
        Task<Dictionary<string, double>> GetAverageCallDurationByTimeSlot(string staffId);
        Task<Dictionary<string, Dictionary<string, int>>> GetCallDurationCategoriesByTimeSlot(string staffId);

        Task<Dictionary<string, Dictionary<string, int>>> GetCallDurationCategoriesByTimeSlotAndImportanceWithDND(
            string staffId, string designation);

        Task<IEnumerable<dynamic>> GetLeadListViewForNewFiltered(int importance);
        Task<IEnumerable<dynamic>> GetLeadListViewForNewFilteredByStaff(int staffId);
        Task AddNewLeadAsync(Tbllead tbllead);
        int AddNewLead(Tbllead tbllead);
        int updateLead(Tbllead tbllead);
        List<VLeads> GetAllDndLeads();
        Task<List<dynamic>> GetAllLeadsByStaffIdAndImportance(int staffId, int importance);
    }
}
