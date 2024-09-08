using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class IouService : IIOUInterface
    {
        private readonly CRMContext _db;

        public IouService(CRMContext context)
        {
            _db = context;
        }
        public async Task<IEnumerable<TblIou>> GetAlliou()
        {
            return await _db.TblIous.ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetAllViou()
        {
            return await _db.TblvIous.ToListAsync();
        }
    }
}