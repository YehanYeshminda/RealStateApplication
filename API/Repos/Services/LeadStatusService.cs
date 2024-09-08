using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class LeadStatusService : ILeadStatusInterface
    {
        private readonly CRMContext _db;

        public LeadStatusService(CRMContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<TblLeadStatus>> GetAllLeadStatus()
        {
            return await _db.TblLeadStatuses.ToListAsync();
        }

        public async Task<TblLeadStatus> GetLeadStatusBYId(int id)
        {
            return await _db.TblLeadStatuses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TblLeadStatus> GetLeadStatusByName(string name)
        {
            return await _db.TblLeadStatuses.FirstOrDefaultAsync(x => x.Leadstatus == name);
        }
    }
}
