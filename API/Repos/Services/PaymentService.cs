using API.Repos.Interfaces;
using Microsoft.Data.SqlClient;

namespace API.Repos.Services
{
    public class PaymentService : IPaymentScheduleInterface
    {
        private readonly IConfiguration _configuration;

        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IEnumerable<dynamic>> GetAllViewPaymentScheduleAsync()
        {
            List<dynamic> paymentSchedules = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM vPurchasePayment", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dynamic paymentSchedule = new System.Dynamic.ExpandoObject();

                            paymentSchedule.PaymentScheduleNo = reader.GetString(reader.GetOrdinal("PaymentScheduleNo"));
                            paymentSchedule.Date = reader.GetDateTime(reader.GetOrdinal("Date"));
                            paymentSchedule.SupplierName = reader.GetString(reader.GetOrdinal("SupplierName"));
                            paymentSchedule.reason = reader.GetString(reader.GetOrdinal("reason"));
                            paymentSchedule.rxpaccount = reader.GetString(reader.GetOrdinal("rxpaccount"));
                            paymentSchedule.amount = reader.GetDecimal(reader.GetOrdinal("amount"));
                            paymentSchedule.paidon = reader.GetDateTime(reader.GetOrdinal("paidon"));
                            paymentSchedule.renewevery = reader.GetString(reader.GetOrdinal("renewevery"));
                            paymentSchedule.renewstatus = reader.GetString(reader.GetOrdinal("renewstatus"));
                            paymentSchedule.status = reader.GetInt32(reader.GetOrdinal("status"));
                            paymentSchedules.Add(paymentSchedule);
                        }
                    }
                }
            }

            return paymentSchedules;
        }

        public async Task<IEnumerable<dynamic>> GetComboDataVExpenses()
        {
            List<dynamic> expensesData = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM vExepnsesAccount", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dynamic expense = new System.Dynamic.ExpandoObject();

                            // Check for null values before reading
                            expense.ID = reader.IsDBNull(reader.GetOrdinal("ID")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("ID"));
                            expense.MainCatID = reader.IsDBNull(reader.GetOrdinal("MainCatID")) ? null : reader.GetString(reader.GetOrdinal("MainCatID"));
                            expense.MainCategory = reader.IsDBNull(reader.GetOrdinal("MainCategory")) ? null : reader.GetString(reader.GetOrdinal("MainCategory"));
                            expense.SubCatID = reader.IsDBNull(reader.GetOrdinal("SubCatID")) ? null : reader.GetString(reader.GetOrdinal("SubCatID"));
                            expense.SubCategory = reader.IsDBNull(reader.GetOrdinal("SubCategory")) ? null : reader.GetString(reader.GetOrdinal("SubCategory"));
                            expense.Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("Status"));

                            expensesData.Add(expense);
                        }
                    }
                }
            }

            return expensesData;
        }

    }
}
