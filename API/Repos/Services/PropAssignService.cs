using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class PropAssignService : IPropAssignInterface
    {
        private readonly CRMContext _db;

        public PropAssignService(CRMContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Tblpropassign>> GetAllPropAssignAll()
        {
            return await _db.Tblpropassigns.ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetAllPropAssignView()
        {
            return await _db.TblvPropAssigns.ToListAsync();
        }

    }
}
