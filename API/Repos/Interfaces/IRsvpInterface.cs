using static API.Repos.Services.RsvpService;

namespace API.Repos.Interfaces
{
    public interface IRsvpInterface
	{
        Task<(bool success, string error)> InsertDataAsync(string tableName, Dictionary<string, object> data);
        Task<IEnumerable<object>> GetExistingRsvpTypeAll();
        Task<RsvpType> GetExistingTypeId(int id);
    }
}

