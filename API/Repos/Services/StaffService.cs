using System.Data;
using API.Models;
using API.Repos.Dtos.CommonDto;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class StaffService : IStaffInterface
    {
        private readonly CRMContext _db;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StaffService(CRMContext context, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _db = context;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }
         

        public async Task<Tblstaff> GetStaffByIdAsync(int id)
        {
            return await _db.Tblstaffs.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<IEnumerable<Tblstaff>> GetAllStaff()
        {
            return await _db.Tblstaffs.ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetAllVStaff()
        {
            List<dynamic> vstaffs = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM tblVstaffs where status = 0", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var filePath = GetFilePath();
                            dynamic vstaff = new System.Dynamic.ExpandoObject();
                            vstaff.Id = reader.GetInt32(reader.GetOrdinal("id"));
                            vstaff.Name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name"));
                            vstaff.Designation = reader.IsDBNull(reader.GetOrdinal("designation")) ? null : reader.GetString(reader.GetOrdinal("designation"));
                            vstaff.Mobileno = reader.IsDBNull(reader.GetOrdinal("mobileno")) ? null : reader.GetString(reader.GetOrdinal("mobileno"));
                            vstaff.Addon = reader.GetDateTime(reader.GetOrdinal("addon"));
                            vstaff.Status = reader.GetInt32(reader.GetOrdinal("status"));
                            vstaff.Parentid = reader.IsDBNull(reader.GetOrdinal("parentid")) ? null : reader.GetString(reader.GetOrdinal("parentid"));
                            vstaff.FirstName = reader.IsDBNull(reader.GetOrdinal("firstname")) ? null : reader.GetString(reader.GetOrdinal("firstname"));
                            vstaff.LastName = reader.IsDBNull(reader.GetOrdinal("lastname")) ? null : reader.GetString(reader.GetOrdinal("lastname"));
                            vstaff.Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email"));
                            vstaff.Password = reader.IsDBNull(reader.GetOrdinal("password")) ? null : reader.GetString(reader.GetOrdinal("password"));
                            vstaff.VisaIssuedate = reader.GetDateTime(reader.GetOrdinal("visaIssuedate"));
                            vstaff.Passport = reader.IsDBNull(reader.GetOrdinal("passport")) ? null : reader.GetString(reader.GetOrdinal("passport"));
                            vstaff.Userimage = reader.IsDBNull(reader.GetOrdinal("userimage")) ? null : reader.GetString(reader.GetOrdinal("userimage"));
                            vstaffs.Add(vstaff);
                        }
                    }
                }
            }

            return vstaffs;
        }

        private string GetFilePath()
        {
            return _webHostEnvironment.WebRootPath + "\\upload\\staff";
        }

        public List<CommonDto> GetAllStaffList()
        {
            List<CommonDto> branches = new List<CommonDto>();

            DAL dAL = new DAL(_configuration);
            SqlParameter[] parameters = new SqlParameter[]
            {
            };
            
            DataTable result = dAL.ExecuteStoredProcedure("StaffListNameId", parameters);

            foreach (DataRow item in result.Rows)
            {
                CommonDto branch = new CommonDto
                {
                    textValue = item["name"].ToString(),
                    value = Convert.ToInt32(item["id"])
                };
                
                branches.Add(branch);
            }
                
            return branches;
        }

        public List<TblCallInsight> GetStaffAssignedCallsToday(int staffId)
        {
            List<TblCallInsight> callInsights = new List<TblCallInsight>();

            DAL dAL = new DAL(_configuration);
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserId", staffId)
            };

            DataTable result = dAL.ExecuteStoredProcedure("GetAllCallsAssignedForUser", parameters);

            foreach (DataRow item in result.Rows)
            {
                
                TblCallInsight branch = new TblCallInsight
                {
                    Id = item["Id"] != DBNull.Value ? Convert.ToInt32(item["Id"]) : 0,
                    FirstName = item["FirstName"] != DBNull.Value ? Convert.ToString(item["FirstName"]) : "",
                    LastName = item["LastName"] != DBNull.Value ? Convert.ToString(item["LastName"]) : "",
                    Email = item["email"] != DBNull.Value ? Convert.ToString(item["email"]) : "",
                    PhoneNo = item["PhoneNo"] != DBNull.Value ? Convert.ToString(item["PhoneNo"]) : "",
                    PhoneNo2 = item["PhoneNo2"] != DBNull.Value ? Convert.ToString(item["PhoneNo2"]) : "",
                    AssignedTo = item["AssignedTo"] != DBNull.Value ? Convert.ToString(item["AssignedTo"]) : "",
                    AddOn = item["AddOn"] != DBNull.Value ? Convert.ToDateTime(item["AddOn"]) : null,
                    Status = item["Status"] != DBNull.Value ? Convert.ToInt32(item["Status"]) : 0,
                    CalledOn = item["CalledOn"] != DBNull.Value ? Convert.ToDateTime(item["CalledOn"]) : null,
                    CallEndedOn = item["CallEndedOn"] != DBNull.Value ? Convert.ToDateTime(item["CallEndedOn"]) : null,
                    AssignedOn = item["AssignedOn"] != DBNull.Value ? Convert.ToDateTime(item["AssignedOn"]) : null
                };
            
                callInsights.Add(branch);
            }

            return callInsights;
        }

        public int RemoveAssignedCalls(int staffId, int count)
        {
            DAL dAL = new DAL(_configuration);
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@StaffId", staffId),
                new SqlParameter("@count", count)
            };

            int res = dAL.ExecuteNonQueryStoredProcedure("DestroyAssignedCalled", parameters);

            return res;
        }
    }
}
