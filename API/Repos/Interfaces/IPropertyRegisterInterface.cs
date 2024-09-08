using API.Models;

namespace API.Repos.Interfaces
{
    public interface IPropertyRegisterInterface
    {
        Task<IEnumerable<Tblpropertyregister>> GetAllPropertyRegisterAll();
        Task<IEnumerable<dynamic>> GetViewPropertyRegisterAll();
    }
}
