using API.Models;
using API.Repos.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class PropDevService : IPropDevInterface
    {
        private readonly CRMContext _db;
        private readonly IConfiguration _configuration;

        public PropDevService(CRMContext context, IConfiguration configuration)
        {
            _db = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Tblpropdev>> GetAllPropDevAll()
        {
            return await _db.Tblpropdevs.ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetViewAllPropDevAll()
        {
            List<dynamic> paymentSchedules = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM vPropertyDevelopmentList", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dynamic paymentSchedule = new System.Dynamic.ExpandoObject();

                            paymentSchedule.Id = reader.GetString(reader.GetOrdinal("id"));
                            paymentSchedule.Date = reader.GetDateTime(reader.GetOrdinal("date"));
                            paymentSchedule.PropertyName = reader.GetString(reader.GetOrdinal("PropertyName"));
                            paymentSchedule.SupplierName = reader.GetString(reader.GetOrdinal("SupplierName"));
                            paymentSchedule.Propertyno = reader.GetString(reader.GetOrdinal("propertyno"));
                            paymentSchedule.Expenseaccount = reader.GetInt32(reader.GetOrdinal("expenseaccount"));
                            paymentSchedule.Description = reader.GetString(reader.GetOrdinal("description"));
                            paymentSchedule.Amount = reader.GetDecimal(reader.GetOrdinal("amount"));
                            paymentSchedule.Cashpaid = reader.GetDecimal(reader.GetOrdinal("cashpaid"));
                            paymentSchedule.Banktransfer = reader.GetDecimal(reader.GetOrdinal("banktransfer"));
                            paymentSchedule.BankCode = reader.GetString(reader.GetOrdinal("BankCode"));
                            paymentSchedule.Chequepaid = reader.GetDecimal(reader.GetOrdinal("chequepaid"));
                            paymentSchedule.ChequeId = reader.GetString(reader.GetOrdinal("ChequeId"));
                            paymentSchedule.ApprovedBy = reader.GetString(reader.GetOrdinal("ApprovedBy"));
                            paymentSchedule.Addon = reader.GetDateTime(reader.GetOrdinal("addon"));
                            paymentSchedule.AddBy = reader.GetString(reader.GetOrdinal("AddBy"));
                            paymentSchedules.Add(paymentSchedule);
                        }
                    }
                }
            }

            return paymentSchedules;
        }
    }
}
