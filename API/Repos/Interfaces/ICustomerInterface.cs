
using API.Models;

namespace API.Repos.Interfaces
{
    public interface ICustomerInterface
    {
        Task<IEnumerable<Tblcustomer>> GetAllCustomerAsync();
    }
}