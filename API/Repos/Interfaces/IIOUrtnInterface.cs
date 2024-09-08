using API.Models;

namespace API.Repos.Interfaces
{
    public interface IIOUrtnInterface
    {
        Task<IEnumerable<TblIourtn>> GetAllIOUrtn();
        Task<IEnumerable<dynamic>> GetAllVIOUrtn();
    }
}
