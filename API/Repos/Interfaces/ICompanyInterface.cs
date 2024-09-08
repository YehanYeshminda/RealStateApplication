using API.Models;

namespace API.Repos.Interfaces
{
    public interface ICompanyInterface
    {
        Task<IEnumerable<Tblcompany>> GetAllCompaniesAync();
        Task<Tblcompany?> GetCompanyByIdAsync(int id);
    }
}
