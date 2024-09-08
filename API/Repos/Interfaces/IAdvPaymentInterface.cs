using API.Models;

namespace API.Repos.Interfaces
{
    public interface IAdvPaymentInterface
    {
        Task<IEnumerable<TblAdvPayment>> GetAllAdv();
        Task<IEnumerable<dynamic>> GetViewAllAdv();
    }
}
