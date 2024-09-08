using API.Models;

namespace API.Repos.Interfaces;

public interface IArchviedLeadInterface
{
    Task<List<Tbllead>> GetAllArchivedLeads();
    Task<List<Tbllead>> GetAllArchivedLeadsDependingOnDate(DateTime startTime, DateTime endTime);
    Task<Tbllead> GetAllArchivedLeadsById(string leadNo);
}