using API.Models;

namespace API.Repos.Interfaces
{
    public interface IPropAssignInterface
    {
        Task<IEnumerable<Tblpropassign>> GetAllPropAssignAll();
        Task<IEnumerable<dynamic>> GetAllPropAssignView();
    }
}
