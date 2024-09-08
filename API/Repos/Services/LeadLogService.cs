using API.Models;
using API.Repos.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class LeadLogService : ILeadLogInterface
    {
        private readonly CRMContext _db;
        private readonly IConfiguration _configuration;

        public LeadLogService(CRMContext context, IConfiguration configuration)
        {
            _db = context;
            _configuration = configuration;
        }
        public async Task<IEnumerable<TblLeadlog>> GetAllLeadLogs()
        {
            return await _db.TblLeadlogs.ToListAsync();
        }

        public async Task<IEnumerable<TblLeadlog>> GetLeadLogByLeadId(string id)
        {
            return await _db.TblLeadlogs.Where(x => x.Leadid == id).OrderByDescending(x => x.Addon).ToListAsync();
        }

        public async Task AddNewLeadLogAsync(TblLeadlog leadlog)
        {
            await _db.TblLeadlogs.AddAsync(leadlog);
        }

        public int AddLog(TblLeadlog tblLeadlog)
        {
            DAL dAL = new DAL(_configuration);
            SqlParameter[] sQlParameters = new SqlParameter[]
            {
                new SqlParameter("@leadid", tblLeadlog.Leadid),
                new SqlParameter("@log", tblLeadlog.Log),
                new SqlParameter("@addby", tblLeadlog.Addby),
                new SqlParameter("@addon", tblLeadlog.Addon),
            };

            int res = dAL.ExecuteNonQueryStoredProcedure("InsertIntoLeadLog", sQlParameters);
            return res;
        }
    }
}
