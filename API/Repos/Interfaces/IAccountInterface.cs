using API.Models;

namespace API.Repos.Interfaces
{
    public interface IAccountInterface
    {
        Task<IEnumerable<Tblstaff>> Getloginuser(int staffId);
    }
}