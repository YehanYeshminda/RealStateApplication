using System.Data;
using API.Models;
using API.Repos.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class CallListService : ICallListInterface
    {
        private readonly CRMContext _db;
        private readonly IConfiguration _configuration;

        public CallListService(CRMContext context, IConfiguration configuration)
        {
            _db = context;
            _configuration = configuration;
        }

        public async Task<TblCallInsight> ExistingCallInsignt(int id)
        {
            return await _db.TblCallInsights.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TblCallInsight> ExistingCallInsigntByPhone(string phone)
        {
            TblCallInsight user = new TblCallInsight();
            DAL dAL = new DAL(_configuration);
            SqlParameter[] sQlParameters = new SqlParameter[]
            {
                new SqlParameter("@PhoneNumber", phone)
            };
            DataTable result = dAL.ExecuteStoredProcedure("GetExistingCallInsightByPhoneNumber", sQlParameters);

            if (result.Rows.Count > 0)
            {
                DataRow item = result.Rows[0];
                user.Id = item["Id"] != DBNull.Value ? Convert.ToInt32(item["Id"]) : 0;
                user.FirstName = item["FirstName"] != DBNull.Value ? Convert.ToString(item["FirstName"]) : "";
                user.LastName = item["LastName"] != DBNull.Value ? Convert.ToString(item["LastName"]) : "";
                user.Email = item["Email"] != DBNull.Value ? Convert.ToString(item["Email"]) : "";
                user.PhoneNo = item["PhoneNo"] != DBNull.Value ? Convert.ToString(item["PhoneNo"]) : "";
                user.PhoneNo2 = item["PhoneNo2"] != DBNull.Value ? Convert.ToString(item["PhoneNo2"]) : "";
                user.AssignedTo = item["AssignedTo"] != DBNull.Value ? Convert.ToString(item["AssignedTo"]) : "";
                user.PhoneNo2 = item["PhoneNo2"] != DBNull.Value ? Convert.ToString(item["PhoneNo2"]) : "";
                user.AddOn = item["AddOn"] != DBNull.Value ? Convert.ToDateTime(item["AddOn"]) : DateTime.Now;
                user.CalledOn = item["calledOn"] != DBNull.Value ? Convert.ToDateTime(item["calledOn"]) : DateTime.Now;
                user.CallEndedOn = item["callEndedOn"] != DBNull.Value ? Convert.ToDateTime(item["callEndedOn"]) : DateTime.Now;
                user.Status = item["Status"] != DBNull.Value ? Convert.ToInt32(item["Status"]) : 0;
                user.AssignedOn = item["AssignedOn"] != DBNull.Value ? Convert.ToDateTime(item["AssignedOn"]) : DateTime.Now;
            }

            return user;
        }

        public async Task<IEnumerable<TblCallInsight>> GetAllCallList()
        {
            return await _db.TblCallInsights.Where(x => x.Status == 0).OrderBy(x => x.AddOn).ToListAsync();
        }

        public async Task<TblCallInsight> GetAllCallListByEmail(string email)
        {
            return await _db.TblCallInsights.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IEnumerable<TblCallInsight>> GetAllCallListByUserId(string userId)
        {
            return await _db.TblCallInsights.Where(x => x.AssignedTo == userId).ToListAsync();
        }

        public async Task<List<TblCallInsight>> GetTopNCallInsights(int numberOfItems)
        {
            var idArray = await _db.TblCallInsights
                .Where(x => x.Status == 0)
                .OrderBy(x => x.Id)
                .Select(x => x.Id)
                .Take(numberOfItems)
                .ToListAsync();

            var topNCallInsights = await _db.TblCallInsights
                .Where(x => idArray.Contains(x.Id))
                .ToListAsync();

            return topNCallInsights;
        }

        public async Task<List<TblCallInsight>> GetCallInsightsByDesignation(DateTime startDate, DateTime endDate, string designation, string userId)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (var command = new SqlCommand("GetCallInsightsByParameters", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime)
                    {
                        Value = startDate
                    });

                    command.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime)
                    {
                        Value = endDate
                    });

                    if (designation != "CEO")
                    {
                        command.Parameters.Add(new SqlParameter("@AssignedTo", SqlDbType.NVarChar, 50)
                        {
                            Value = userId
                        });
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@AssignedTo", SqlDbType.NVarChar, 50)
                        {
                            Value = DBNull.Value
                        });
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var results = new List<TblCallInsight>();

                        while (reader.Read())
                        {
                            var callInsight = new TblCallInsight
                            {
                                Id = reader["Id"] as int? ?? 0,
                                FirstName = reader["FirstName"] as string ?? "",
                                LastName = reader["LastName"] as string ?? "",
                                Email = reader["Email"] as string ?? "",
                                PhoneNo = reader["PhoneNo"] as string ?? "",
                                PhoneNo2 = reader["PhoneNo2"] as string ?? "",
                                AssignedTo = reader["AssignedTo"] as string ?? "",
                                AddOn = reader["AddOn"] as DateTime? ?? DateTime.MinValue,
                                CalledOn = reader["CalledOn"] as DateTime? ?? DateTime.MinValue,
                                CallEndedOn = reader["CallEndedOn"] as DateTime? ?? DateTime.MinValue,
                                Status = reader["Status"] as int? ?? 0,
                                AssignedOn = reader["AssignedOn"] as DateTime? ?? DateTime.MinValue
                            };

                            results.Add(callInsight);
                        }

                        return results;
                    }
                }
            }
        }

        public int UpdateCall(TblCallInsight callInsight)
        {
            DAL dAL = new DAL(_configuration);

            SqlParameter[] sQlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", callInsight.Id),
                new SqlParameter("@FirstName", callInsight.FirstName),
                new SqlParameter("@LastName", callInsight.LastName),
                new SqlParameter("@Email", callInsight.Email),
                new SqlParameter("@PhoneNo", callInsight.PhoneNo),
                new SqlParameter("@PhoneNo2", callInsight.PhoneNo2),
                new SqlParameter("@AssignedTo", callInsight.AssignedTo),
                new SqlParameter("@AddOn", callInsight.AddOn),
                new SqlParameter("@CalledOn", callInsight.CalledOn),
                new SqlParameter("@CallEndedOn", callInsight.CallEndedOn),
                new SqlParameter("@Status", callInsight.Status),
                new SqlParameter("@AssignedOn", callInsight.AssignedOn),
            };

            int res = dAL.ExecuteNonQueryStoredProcedure("UpdateCallInsight", sQlParameters);
            return res;
        }

        public int UpdateAndSetCalls(int numberOfCalls, int assignedTo, DateTime assignedOn)
        {
            DAL dAL = new DAL(_configuration);

            SqlParameter[] sQlParameters = new SqlParameter[]
            {
                new SqlParameter("@AssignedTo", assignedTo),
                new SqlParameter("@AssignedOn", assignedOn),
                new SqlParameter("@NumberOfItems", numberOfCalls),
            };

            int res = dAL.ExecuteNonQueryStoredProcedure("UpdateAndSetCallsForUsers", sQlParameters);
            return res;
        }

        public int AssignCallInsight(List<int> values, string staffId)
        {
            DAL dAL = new DAL(_configuration);

            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            foreach (int value in values)
            {
                table.Rows.Add(value);
            }

            SqlParameter[] sQlParameters = new SqlParameter[]
            {
                new SqlParameter("@CallInsightIds", table)
                {
                    TypeName = "dbo.CallInsightIdListType",
                    SqlDbType = SqlDbType.Structured 
                },
                new SqlParameter("@AssignStaff", staffId),
            };

            int res = dAL.ExecuteNonQueryStoredProcedure("AssignCallInsight", sQlParameters);
            return res;
        }
    }
}
