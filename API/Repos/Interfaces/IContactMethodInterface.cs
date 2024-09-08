using API.Models;

namespace API.Repos.Interfaces
{
    public interface IContactMethodInterface
    {
        Task<TblPreferedContactMethod> GetContactMethod(int id);
    }
}
