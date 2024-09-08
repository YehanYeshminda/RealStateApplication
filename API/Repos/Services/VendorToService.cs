using API.Models;
using API.Repos.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class VendorToService : IVendorToServiceInterface
    {
        private readonly CRMContext _db;
        private readonly IConfiguration _configuration;

        public VendorToService(CRMContext context, IConfiguration configuration)
        {
            _db = context;
            _configuration = configuration;
        }
        public async Task<IEnumerable<Tblvendertoservice>> GetAllVendorsAsync()
        {
            return await _db.Tblvendertoservices.ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetAllVendorsViewAsync()
        {
            List<dynamic> paymentSchedules = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM vVendorToServiceList", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dynamic paymentSchedule = new System.Dynamic.ExpandoObject();

                            paymentSchedule.Id = reader.GetInt32(reader.GetOrdinal("id"));
                            paymentSchedule.SupplierName = reader.GetString(reader.GetOrdinal("SupplierName"));
                            paymentSchedule.TypeName = reader.GetString(reader.GetOrdinal("TypeName"));
                            paymentSchedule.Status = reader.GetInt32(reader.GetOrdinal("status"));
                            paymentSchedule.AddOn = reader.GetDateTime(reader.GetOrdinal("addon"));
                            paymentSchedule.AddBy = reader.GetString(reader.GetOrdinal("addBy"));
                            paymentSchedules.Add(paymentSchedule);
                        }
                    }
                }
            }

            return paymentSchedules;
        }

        public async Task<Tblvendertoservice> GetVendorsByIdAsync(int id)
        {
            return await _db.Tblvendertoservices.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
