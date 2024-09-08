
using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class IOUrtnService : IIOUrtnInterface
    {
        private readonly CRMContext _db;

        public IOUrtnService(CRMContext context)
        {
            _db = context;
        }
        public async Task<IEnumerable<TblIourtn>> GetAllIOUrtn()
        {
            return await _db.TblIourtns.ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetAllVIOUrtn()
        {
            return await _db.TblvIourtns.ToListAsync();
        }
    }
}