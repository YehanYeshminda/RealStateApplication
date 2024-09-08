using API.Models;

namespace API.Repos.Interfaces
{
    public interface IVendorToServiceInterface
    {
        Task<IEnumerable<Tblvendertoservice>> GetAllVendorsAsync();
        Task<IEnumerable<dynamic>> GetAllVendorsViewAsync();
        Task<Tblvendertoservice> GetVendorsByIdAsync(int id);
    }
}
