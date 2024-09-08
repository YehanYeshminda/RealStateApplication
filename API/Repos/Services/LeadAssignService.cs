using API.Models;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class LeadAssignService : ILeadAssignInterface
    {
        private readonly CRMContext _db;

        public LeadAssignService(CRMContext context)
        {
            _db = context;
        }

        public async Task<TblleadAssign> GetLeadAssigningsByPrimartId(int id)
        {
            return await _db.TblleadAssigns.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<TblleadAssign>> GetLeadAssigningsByStaff(int staffId)
        {
            return await _db.TblleadAssigns.Where(X => X.Staffid == staffId).ToListAsync();
        }

        public async Task<TblleadAssign> GetLeadAssignsByStaffId(int staffId, string leadId)
        {
            return await _db.TblleadAssigns.FirstOrDefaultAsync(x => x.Staffid == staffId && x.Leadid == leadId);        
        }
    }
}
