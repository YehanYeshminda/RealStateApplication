using System.Data;
using API.Models;
using API.Repos.Dtos.CommonDto;
using API.Repos.Dtos.LeadlogDtos;
using API.Repos.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class LeadForwardService : ILeadForwardInterface
    {
        private readonly CRMContext _db;
        private readonly IConfiguration _configuration;

        public LeadForwardService(CRMContext context, IConfiguration configuration)
        {
            _db = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<TblLeadforward>> GetAllLeadForwards()
        {
            return await _db.TblLeadforwards.ToListAsync();
        }

        public List<BankInfoDto> GetBankInfoByLeadId()
        {
            List<BankInfoDto> branches = new List<BankInfoDto>();

            DAL dAL = new DAL(_configuration);
            SqlParameter[] parameters = new SqlParameter[]
            {
            };
            
            DataTable result = dAL.ExecuteStoredProcedure("GetAllLeadIdName", parameters);

            foreach (DataRow item in result.Rows)
            {
                BankInfoDto branch = new BankInfoDto
                {
                    textValue = item["name"].ToString(),
                    value = item["leadno"].ToString(),
                };
                
                branches.Add(branch);
            }
                
            return branches;
        }

        public async Task<IEnumerable<dynamic>> GetLeadForwardListView()
        {
            List<dynamic> paymentSchedules = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM vLeadForwardList", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dynamic paymentSchedule = new System.Dynamic.ExpandoObject();

                            paymentSchedule.Id = reader.IsDBNull(reader.GetOrdinal("id")) ? 0 : reader.GetInt32(reader.GetOrdinal("id"));
                            paymentSchedule.Date = reader.IsDBNull(reader.GetOrdinal("date")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("date"));
                            paymentSchedule.LeadId = reader.IsDBNull(reader.GetOrdinal("leadid")) ? null : reader.GetString(reader.GetOrdinal("leadid"));
                            paymentSchedule.ForwardTo = reader.IsDBNull(reader.GetOrdinal("forwardTo")) ? null : reader.GetString(reader.GetOrdinal("forwardTo"));
                            paymentSchedule.Reason = reader.IsDBNull(reader.GetOrdinal("reason")) ? null : reader.GetString(reader.GetOrdinal("reason"));
                            paymentSchedule.AddOn = reader.IsDBNull(reader.GetOrdinal("addon")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("addon"));
                            paymentSchedule.ForwardFrom = reader.IsDBNull(reader.GetOrdinal("forwardFrom")) ? null : reader.GetString(reader.GetOrdinal("forwardFrom"));
                            //paymentSchedule.AddBy = reader.IsDBNull(reader.GetOrdinal("AddBy")) ? null : reader.GetString(reader.GetOrdinal("AddBy"));

                            paymentSchedules.Add(paymentSchedule);
                        }

                    }
                }
            }

            return paymentSchedules;
        }

        public async Task<TblLeadforward> GetLeadForwardsById(int id)
        {
            return await _db.TblLeadforwards.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TblLeadforward> GetLeadForwardsByLeadId(string id)
        {
            return await _db.TblLeadforwards.FirstOrDefaultAsync(x => x.Leadid == id);
        }

        public async Task<GetLeadLogDto> GetLeadForwardWithLog(string id)
        {
            var existingLog = await _db.TblLeadlogs.Where(x => x.Leadid == id).OrderByDescending(x => x.Addon).FirstOrDefaultAsync();
            var existingLead = await _db.Tblleads.FirstOrDefaultAsync(x => x.Leadno == id);

            var newItem = new GetLeadLogDto
            {
                Date = existingLead.Assignon.ToString(),
                Name = existingLead.Name,
                Log = existingLog.Log
            };

            return newItem;
        }
    }
}
