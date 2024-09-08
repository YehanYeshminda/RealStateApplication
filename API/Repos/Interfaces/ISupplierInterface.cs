using API.Models;

namespace API.Repos.Interfaces
{
    public interface ISupplierInterface
    {
        Task<IEnumerable<TblSupplier>> GetAllSuppliersAsync();
        Task<IEnumerable<dynamic>> GetAllVSuppliers();
        Task<TblSupplier> GetSupplierByIdAsync(int id);
    }
}
