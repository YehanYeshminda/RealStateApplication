using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class SupplierService : ISupplierInterface
    {
        private readonly CRMContext _db;

        public SupplierService(CRMContext context)
        {
            _db = context;
        }
        public async Task<IEnumerable<TblSupplier>> GetAllSuppliersAsync()
        {
            return await _db.TblSuppliers.ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetAllVSuppliers()
        {
            return await _db.Tblvvendors.ToListAsync();
        }

        public async Task<TblSupplier> GetSupplierByIdAsync(int id)
        {
            return await _db.TblSuppliers.FirstOrDefaultAsync(x => x.SupplierId == id);
        }
    }
}
