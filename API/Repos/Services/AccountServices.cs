using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class AccountServices : IAccountInterface
    {
        private readonly CRMContext _db;

        public AccountServices(CRMContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Tblstaff>> Getloginuser(int staffId)
        {
            return await _db.Tblstaffs.Where(x => x.Id == staffId).AsNoTracking().ToListAsync();
        }
    }
}
