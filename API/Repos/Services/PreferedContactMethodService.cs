using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class PreferedContactMethodService : IPreferedContactMethodInterface
    {
        private readonly CRMContext _db;

        public PreferedContactMethodService(CRMContext context)
        {
            _db = context;
        }
        public async Task<IEnumerable<TblPreferedContactMethod>> GetAllContactMethods()
        {
            return await _db.TblPreferedContactMethods.ToListAsync();
        }

        public async Task<TblPreferedContactMethod> GetContactMethodById(int id)
        {
            return await _db.TblPreferedContactMethods.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
