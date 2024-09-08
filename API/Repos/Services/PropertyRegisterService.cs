using API.Models;
using API.Repos.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class PropertyRegisterService : IPropertyRegisterInterface
    {
        private readonly CRMContext _db;
        private readonly IConfiguration _configuration;

        public PropertyRegisterService(CRMContext context, IConfiguration configuration)
        {
            _db = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Tblpropertyregister>> GetAllPropertyRegisterAll()
        {
            return await _db.Tblpropertyregisters.ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetViewPropertyRegisterAll()
        {
            List<dynamic> paymentSchedules = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM vPropertyRegisterList", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dynamic paymentSchedule = new System.Dynamic.ExpandoObject();

                            paymentSchedule.Id = reader.GetString(reader.GetOrdinal("id"));
                            paymentSchedule.PropertyName = reader.GetString(reader.GetOrdinal("propertname"));
                            paymentSchedule.PropertyType = reader.GetString(reader.GetOrdinal("propertytype"));
                            paymentSchedule.PropertyCatergory = reader.GetString(reader.GetOrdinal("propertyCat"));
                            paymentSchedule.PropertySubCatergory = reader.GetString(reader.GetOrdinal("propertySubCat"));
                            paymentSchedule.TypeName = reader.GetString(reader.GetOrdinal("TypeName"));
                            paymentSchedule.Nationality = reader.GetString(reader.GetOrdinal("nationality"));
                            paymentSchedule.Address = reader.GetString(reader.GetOrdinal("address"));
                            paymentSchedule.geolocation = reader.GetString(reader.GetOrdinal("geolocation"));
                            paymentSchedule.SupplierName = reader.GetString(reader.GetOrdinal("SupplierName"));
                            paymentSchedule.CostAnually = reader.GetDecimal(reader.GetOrdinal("costanually"));
                            paymentSchedule.Othercost = reader.GetDecimal(reader.GetOrdinal("othercost"));
                            paymentSchedule.Rulesregulations = reader.GetString(reader.GetOrdinal("rulesregulations"));
                            paymentSchedule.Status = reader.GetInt32(reader.GetOrdinal("status"));
                            paymentSchedule.Sellingprice = reader.GetDecimal(reader.GetOrdinal("sellingprice"));
                            paymentSchedule.Minsellingprice = reader.GetDecimal(reader.GetOrdinal("minsellingprice"));
                            paymentSchedule.Anualcostforbuyer = reader.GetDecimal(reader.GetOrdinal("anualcostforbuyer"));
                            paymentSchedule.Deposit = reader.GetDecimal(reader.GetOrdinal("deposit"));
                            paymentSchedule.Contacttype = reader.GetInt32(reader.GetOrdinal("contacttype"));
                            paymentSchedule.Socialmedia = reader.GetInt32(reader.GetOrdinal("socialmedia"));
                            paymentSchedule.Mainimg = reader.GetString(reader.GetOrdinal("mainimg"));
                            paymentSchedule.Otherimg = reader.GetString(reader.GetOrdinal("otherimg"));
                            paymentSchedule.Dateofpurchorrent = reader.GetDateTime(reader.GetOrdinal("dateofpurchorrent"));
                            paymentSchedule.Renewdate = reader.GetDateTime(reader.GetOrdinal("renewdate"));
                            paymentSchedule.Venderpaymentdate = reader.GetDateTime(reader.GetOrdinal("venderpaymentdate"));
                            paymentSchedule.Addon = reader.GetDateTime(reader.GetOrdinal("addon"));
                            paymentSchedule.Paymentscheduleno = reader.GetString(reader.GetOrdinal("paymentscheduleno"));
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
