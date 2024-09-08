using API.Models;

namespace API.Repos.Interfaces
{
    public interface IIOUInterface
    {
        Task<IEnumerable<TblIou>> GetAlliou();
        Task<IEnumerable<dynamic>> GetAllViou();
    }
}


