using API.Models;
using API.Repos.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class NotificationService : INotificationInterface
    {
        private readonly CRMContext _db;
        private readonly IConfiguration _configuration;

        public NotificationService(CRMContext context, IConfiguration configuration)
        {
            _db = context;
            _configuration = configuration;
        }
        
        public async Task<IEnumerable<TblNotification>> GetAllNotification()
        {
            return await _db.TblNotifications.Where(x => x.Status == 0).ToListAsync();
        }

        public async Task<IEnumerable<TblNotification>> GetAllNotificationForUser(string staffId)
        {
            return await _db.TblNotifications.Where(x => x.Forwardto == staffId && x.Status == 0).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetAllNotificationForUserView(string staffId)
        {
            List<dynamic> paymentSchedules = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM tblvNotification WHERE forwardId = @staffId";


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@staffId", staffId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dynamic paymentSchedule = new System.Dynamic.ExpandoObject();

                            paymentSchedule.Id = reader.GetInt32(reader.GetOrdinal("id"));
                            paymentSchedule.Name = reader.GetString(reader.GetOrdinal("notify"));
                            paymentSchedule.Date = reader.GetDateTime(reader.GetOrdinal("date"));
                            paymentSchedule.Time = reader.GetString(reader.GetOrdinal("time"));
                            paymentSchedule.Message = reader.GetString(reader.GetOrdinal("message"));
                            paymentSchedule.prority = reader.GetInt32(reader.GetOrdinal("priorityid"));
                            paymentSchedule.AddBy = reader.GetString(reader.GetOrdinal("username"));
                            paymentSchedule.AddOn = reader.GetDateTime(reader.GetOrdinal("addon"));
                            paymentSchedule.ForwardTo = reader.GetString(reader.GetOrdinal("forwardto"));
                            paymentSchedule.SnoozeOn = reader.GetDateTime(reader.GetOrdinal("snoozeon"));
                            paymentSchedule.From = reader.GetDateTime(reader.GetOrdinal("from"));
                            paymentSchedules.Add(paymentSchedule);
                        }
                    }
                }
            }

            return paymentSchedules;
        }

        public async Task<DateTime?> GetLatestSnoozeonTime()
        {
            var currentTime = DateTime.Now;

            var nextSnoozeon = await _db.TblNotifications
                .Where(n => n.Snoozeon > currentTime)
                .OrderBy(n => n.Snoozeon)
                .Select(n => n.Snoozeon)
                .FirstOrDefaultAsync();

            return nextSnoozeon;
        }

        public async Task<int> UpdateNotiticationCount(string userId)
        {
            return await _db.TblNotifications.Where(x => x.Forwardto == userId).CountAsync();
        }

        public async Task<TblNotification> GetNotificationByIdAsync(int id)
        {
            return await _db.TblNotifications.FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
