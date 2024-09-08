using API.Models;

namespace API.Repos.Interfaces
{
    public interface IPropDevInterface
    {
        Task<IEnumerable<Tblpropdev>> GetAllPropDevAll();
        Task<IEnumerable<dynamic>> GetViewAllPropDevAll();
    }
}
