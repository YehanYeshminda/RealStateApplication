using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class CompanyService : ICompanyInterface
    {
        private readonly CRMContext _db;

        public CompanyService(CRMContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Tblcompany>> GetAllCompaniesAync()
        {
            return await _db.Tblcompanies.ToListAsync();
        }

        public async Task<Tblcompany?> GetCompanyByIdAsync(int id)
        {
            return await _db.Tblcompanies.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
