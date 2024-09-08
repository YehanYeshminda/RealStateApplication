using API.Models;

namespace API.Repos.Interfaces
{
    public interface IPreferedContactMethodInterface
    {
        Task<IEnumerable<TblPreferedContactMethod>> GetAllContactMethods();
        Task<TblPreferedContactMethod> GetContactMethodById(int id);
    }
}
