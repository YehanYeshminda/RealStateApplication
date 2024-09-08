using System.Data;
using API.Models;
using API.Repos.Dtos.LeadsDtos;
using API.Repos.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class LeadsService : ILeadsInterface
    {
        private readonly CRMContext _db;
        private readonly IConfiguration _configuration;

        public LeadsService(CRMContext context, IConfiguration configuration)
        {
            _db = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Tbllead>> GetAllLeads()
        {
            return await _db.Tblleads.ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetAllLeadsOnWeekDaysSqlRaw(string staffId)
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

            DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);
            DateTime currentDate = dubaiTime;

            DateTime weekStart = currentDate.AddDays(-(int)currentDate.DayOfWeek);
            DateTime weekEnd = weekStart.AddDays(6);

            Dictionary<string, int> leadsByDayOfWeek = new Dictionary<string, int>
            {
                {"Sunday", 0},
                {"Monday", 0},
                {"Tuesday", 0},
                {"Wednesday", 0},
                {"Thursday", 0},
                {"Friday", 0},
                {"Saturday", 0}
            };

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var query = $"SELECT * FROM tblleads WHERE importance = 7 AND staffid = @staffid AND assigned = @staffid AND assignon >= @WeekStart AND assignon <= @WeekEnd";

                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@WeekStart", weekStart);
                    command.Parameters.AddWithValue("@WeekEnd", weekEnd);
                    command.Parameters.AddWithValue("@staffid", staffId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            DateTime assignon = reader.GetDateTime(reader.GetOrdinal("assignon"));
                            string dayOfWeek = assignon.DayOfWeek.ToString();
                            leadsByDayOfWeek[dayOfWeek]++;
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return leadsByDayOfWeek;
        }
        
        public async Task<Dictionary<string, double>> GetAverageCallDurationByDayOfWeek(string staffId)
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

            DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);
            
            DateTime currentDate = dubaiTime;
        
            DateTime weekStart = currentDate.AddDays(-(int)currentDate.DayOfWeek);
            DateTime weekEnd = weekStart.AddDays(6);
        
            Dictionary<string, double> averageCallDurationByDayOfWeek = new Dictionary<string, double>
            {
                {"Sunday", 0},
                {"Monday", 0},
                {"Tuesday", 0},
                {"Wednesday", 0},
                {"Thursday", 0},
                {"Friday", 0},
                {"Saturday", 0}
            };
        
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var query = $"SELECT calledOn, callEndedOn FROM tblCallInsights WHERE AssignedTo = @staffId AND calledOn >= @weekStart AND callEndedOn <= @weekEnd";
        
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@weekStart", weekStart);
                    command.Parameters.AddWithValue("@weekEnd", weekEnd);
                    command.Parameters.AddWithValue("@staffId", staffId);
        
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            DateTime calledOn = reader.GetDateTime(reader.GetOrdinal("calledOn"));
                            DateTime callEndedOn = reader.GetDateTime(reader.GetOrdinal("callEndedOn"));
        
                            string dayOfWeek = calledOn.DayOfWeek.ToString();
                            double callDurationMinutes = (callEndedOn - calledOn).TotalMinutes;
        
                            averageCallDurationByDayOfWeek[dayOfWeek] += callDurationMinutes;
                        }
                    }
                }
        
                await connection.CloseAsync();
            }
        
            foreach (var dayOfWeek in averageCallDurationByDayOfWeek.Keys.ToList())
            {
                if (averageCallDurationByDayOfWeek[dayOfWeek] > 0)
                {
                    averageCallDurationByDayOfWeek[dayOfWeek] /= 60;
                    averageCallDurationByDayOfWeek[dayOfWeek] = Math.Round(averageCallDurationByDayOfWeek[dayOfWeek], 3);
                }
            }
        
            return averageCallDurationByDayOfWeek;
        }
        
        public async Task<Dictionary<string, Dictionary<string, int>>> GetCallDurationCategoriesByTimeSlot(string staffId)
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

            DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);
            
            DateTime currentDate = dubaiTime;

            DateTime todayStart = currentDate.Date.AddHours(8);
            DateTime todayEnd = currentDate.Date.AddHours(18);

            Dictionary<string, Dictionary<string, int>> callDurationCategoriesByTimeSlot = new Dictionary<string, Dictionary<string, int>>
            {
                {"EightAM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0} }},
                {"NineAM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0} }},
                {"TenAM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0} }},
                {"ElevenAM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0} }},
                {"TwelvePM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0} }},
                {"OnePM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0} }},
                {"TwoPM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0} }},
                {"ThreePM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0} }},
                {"FourPM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0} }},
                {"FivePM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0} }},
                {"sixPM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0} }}
            };

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var query = $"SELECT calledOn, callEndedOn FROM tblCallInsights WHERE AssignedTo = @staffId AND calledOn >= @todayStart AND callEndedOn <= @todayEnd";

                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@todayStart", todayStart);
                    command.Parameters.AddWithValue("@todayEnd", todayEnd);
                    command.Parameters.AddWithValue("@staffId", staffId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            DateTime calledOn = reader.GetDateTime(reader.GetOrdinal("calledOn"));
                            DateTime callEndedOn = reader.GetDateTime(reader.GetOrdinal("callEndedOn"));
                            
                            double callDurationMinutes = (callEndedOn - calledOn).TotalMinutes;
                            
                            string timeSlot = GetTimeSlot(calledOn);
                            
                            string durationCategory = GetDurationCategory(callDurationMinutes);
                            
                            callDurationCategoriesByTimeSlot[timeSlot][durationCategory]++;
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return callDurationCategoriesByTimeSlot;
        }
        
        public async Task<Dictionary<string, Dictionary<string, int>>> GetCallDurationCategoriesByTimeSlotAndImportanceWithDND(string staffId, string designation)
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

            DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);
            
            DateTime currentDate = dubaiTime;

            DateTime todayStart = currentDate.Date.AddHours(8);
            DateTime todayEnd = currentDate.Date.AddHours(18);

            Dictionary<string, Dictionary<string, int>> callDurationCategoriesByTimeSlotAndImportance = new Dictionary<string, Dictionary<string, int>>
            {
                {"EightAM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0}, { "IsDND", 0 } }},
                {"NineAM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0},{ "IsDND", 0 } }},
                {"TenAM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0},{ "IsDND", 0 } }},
                {"ElevenAM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0},{ "IsDND", 0 } }},
                {"TwelvePM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0},{ "IsDND", 0 } }},
                {"OnePM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0},{ "IsDND", 0 } }},
                {"TwoPM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0},{ "IsDND", 0 } }},
                {"ThreePM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0},{ "IsDND", 0 } }},
                {"FourPM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0},{ "IsDND", 0 } }},
                {"FivePM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0},{ "IsDND", 0 } }},
                {"sixPM", new Dictionary<string, int> { {"LessThanMinute", 0}, {"OneToFiveMinutes", 0}, {"GreaterThanFive", 0},{ "IsDND", 0 } }}
            };

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var callInsightsQuery = "";
                var leadsQuery = "";

                if (designation == "CEO")
                {
                    callInsightsQuery = $"SELECT calledOn, callEndedOn FROM tblCallInsights WHERE calledOn IS NOT NULL AND callEndedOn IS NOT NULL AND calledOn >= @todayStart AND callEndedOn <= @todayEnd";
                    leadsQuery = $"SELECT importance, AddedOn FROM tblleads WHERE importance IS NOT NULL AND AddedOn IS NOT NULL";
                }
                else
                {
                    callInsightsQuery = $"SELECT calledOn, callEndedOn FROM tblCallInsights WHERE AssignedTo IS NOT NULL AND calledOn IS NOT NULL AND callEndedOn IS NOT NULL AND AssignedTo = @staffId AND calledOn >= @todayStart AND callEndedOn <= @todayEnd";
                    leadsQuery = $"SELECT importance, AddedOn FROM tblleads WHERE staffid = @staffId AND importance IS NOT NULL AND AddedOn IS NOT NULL";
                }

                await connection.OpenAsync();
                
                using (SqlCommand callInsightsCommand = new SqlCommand(callInsightsQuery, connection))
                {
                    callInsightsCommand.Parameters.AddWithValue("@todayStart", todayStart);
                    callInsightsCommand.Parameters.AddWithValue("@todayEnd", todayEnd);

                    if (designation != "CEO")
                    {
                        callInsightsCommand.Parameters.AddWithValue("@staffId", staffId);
                    }

                    using (SqlDataReader insightsReader = await callInsightsCommand.ExecuteReaderAsync())
                    {
                        while (await insightsReader.ReadAsync())
                        {
                            DateTime? calledOn = insightsReader.IsDBNull(insightsReader.GetOrdinal("calledOn")) ? (DateTime?)null : insightsReader.GetDateTime(insightsReader.GetOrdinal("calledOn"));
                            DateTime? callEndedOn = insightsReader.IsDBNull(insightsReader.GetOrdinal("callEndedOn")) ? (DateTime?)null : insightsReader.GetDateTime(insightsReader.GetOrdinal("callEndedOn"));

                            double callDurationMinutes = 0; // Initialize to 0

                            if (calledOn.HasValue && callEndedOn.HasValue)
                            {
                                callDurationMinutes = (callEndedOn.Value - calledOn.Value).TotalMinutes;
                            }

                            string timeSlot = GetTimeSlot(calledOn);
                            string durationCategory = GetDurationCategory(callDurationMinutes);

                            if (timeSlot != null)
                            {
                                // Ensure that the dictionary entry exists, and if not, create it
                                if (!callDurationCategoriesByTimeSlotAndImportance.ContainsKey(timeSlot))
                                {
                                    callDurationCategoriesByTimeSlotAndImportance[timeSlot] = new Dictionary<string, int>
                                    {
                                        {"LessThanMinute", 0},
                                        {"OneToFiveMinutes", 0},
                                        {"GreaterThanFive", 0},
                                        {"IsDND", 0}
                                    };
                                }

                                callDurationCategoriesByTimeSlotAndImportance[timeSlot][durationCategory]++;
                            }
                        }
                    }
                }
                
                using (SqlCommand leadsCommand = new SqlCommand(leadsQuery, connection))
                {
                    if (designation != "CEO")
                    {
                        leadsCommand.Parameters.AddWithValue("@staffId", staffId);
                    }
                    
                    using (SqlDataReader leadsReader = await leadsCommand.ExecuteReaderAsync())
                    {
                        while (await leadsReader.ReadAsync())
                        {
                            int importance = leadsReader.GetInt32(leadsReader.GetOrdinal("importance"));
                            DateTime addedOn = leadsReader.GetDateTime(leadsReader.GetOrdinal("AddedOn"));

                            if (importance == 18 && addedOn >= todayStart && addedOn <= todayEnd)
                            {
                                string timeSlot = GetTimeSlot(addedOn);
                                callDurationCategoriesByTimeSlotAndImportance[timeSlot]["IsDND"]++;
                            }
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return callDurationCategoriesByTimeSlotAndImportance;
        }

        
        private string GetTimeSlotTime(DateTime? callTime)
        {
            if (callTime.HasValue)
            {
                if (callTime.Value.Hour >= 8 && callTime.Value.Hour < 9)
                {
                    return "8AM";
                }
                else if (callTime.Value.Hour >= 9 && callTime.Value.Hour < 10)
                {
                    return "9AM";
                }
                else if (callTime.Value.Hour >= 10 && callTime.Value.Hour < 11)
                {
                    return "10AM";
                }
                else if (callTime.Value.Hour >= 11 && callTime.Value.Hour < 12)
                {
                    return "11AM";
                }
                else if (callTime.Value.Hour >= 12 && callTime.Value.Hour < 13)
                {
                    return "12PM";
                }
                else if (callTime.Value.Hour >= 13 && callTime.Value.Hour < 14)
                {
                    return "1PM";
                }
                else if (callTime.Value.Hour >= 14 && callTime.Value.Hour < 15)
                {
                    return "2PM";
                }
                else if (callTime.Value.Hour >= 15 && callTime.Value.Hour < 16)
                {
                    return "3PM";
                }
                else if (callTime.Value.Hour >= 16 && callTime.Value.Hour < 17)
                {
                    return "4PM";
                }
                else if (callTime.Value.Hour >= 17 && callTime.Value.Hour < 18)
                {
                    return "5PM";
                }
                else
                {
                    return "6PM";
                }
            }

            return "";
        }

        // Helper function to determine the category for call duration
        private string GetDurationCategory(double callDurationMinutes)
        {
            if (callDurationMinutes < 1)
            {
                return "LessThanMinute";
            }
            else if (callDurationMinutes >= 1 && callDurationMinutes <= 5)
            {
                return "OneToFiveMinutes";
            }
            else
            {
                return "GreaterThanFive";
            }
        }
        
        public async Task<Dictionary<string, double>> GetAverageCallDurationByTimeSlot(string staffId)
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

            DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);
            
            DateTime currentDate = dubaiTime;

            DateTime todayStart = currentDate.Date.AddHours(8);
            DateTime todayEnd = currentDate.Date.AddHours(18);

            Dictionary<string, double> averageCallDurationByTimeSlot = new Dictionary<string, double>
            {
                {"8AM", 0},
                {"9AM", 0},
                {"10AM", 0},
                {"11AM", 0},
                {"12PM", 0},
                {"1PM", 0},
                {"2PM", 0},
                {"3PM", 0},
                {"4PM", 0},
                {"5PM", 0},
                {"6PM", 0}
            };

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var query = $"SELECT calledOn, callEndedOn FROM tblCallInsights WHERE AssignedTo = @staffId AND calledOn >= @todayStart AND callEndedOn <= @todayEnd";

                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@todayStart", todayStart);
                    command.Parameters.AddWithValue("@todayEnd", todayEnd);
                    command.Parameters.AddWithValue("@staffId", staffId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            DateTime calledOn = reader.GetDateTime(reader.GetOrdinal("calledOn"));
                            DateTime callEndedOn = reader.GetDateTime(reader.GetOrdinal("callEndedOn"));

                            // Calculate the call duration in minutes
                            double callDurationMinutes = (callEndedOn - calledOn).TotalMinutes;

                            // Determine the time slot for this call
                            string timeSlot = GetTimeSlotTime(calledOn);

                            // Add the call duration to the corresponding time slot
                            averageCallDurationByTimeSlot[timeSlot] += callDurationMinutes;
                        }
                    }
                }

                await connection.CloseAsync();
            }

            // Round the values to two decimal places
            foreach (var timeSlot in averageCallDurationByTimeSlot.Keys.ToList())
            {
                if (averageCallDurationByTimeSlot[timeSlot] > 0)
                {
                    averageCallDurationByTimeSlot[timeSlot] /= 60;
                    averageCallDurationByTimeSlot[timeSlot] = Math.Round(averageCallDurationByTimeSlot[timeSlot], 2);
                }
            }

            return averageCallDurationByTimeSlot;
        }

        private string GetTimeSlot(DateTime? callTime)
        {
            if (callTime.HasValue)
            {
                if (callTime.Value.Hour >= 8 && callTime.Value.Hour < 9)
                {
                    return "EightAM";
                }
                else if (callTime.Value.Hour >= 9 && callTime.Value.Hour < 10)
                {
                    return "NineAM";
                }
                else if (callTime.Value.Hour >= 10 && callTime.Value.Hour < 11)
                {
                    return "TenAM";
                }
                else if (callTime.Value.Hour >= 11 && callTime.Value.Hour < 12)
                {
                    return "ElevenAM";
                }
                else if (callTime.Value.Hour >= 12 && callTime.Value.Hour < 13)
                {
                    return "TwelvePM";
                }
                else if (callTime.Value.Hour >= 13 && callTime.Value.Hour < 14)
                {
                    return "OnePM";
                }
                else if (callTime.Value.Hour >= 14 && callTime.Value.Hour < 15)
                {
                    return "TwoPM";
                }
                else if (callTime.Value.Hour >= 15 && callTime.Value.Hour < 16)
                {
                    return "ThreePM";
                }
                else if (callTime.Value.Hour >= 16 && callTime.Value.Hour < 17)
                {
                    return "FourPM";
                }
                else if (callTime.Value.Hour >= 17 && callTime.Value.Hour < 18)
                {
                    return "FivePM";
                }
                else
                {
                    return "sixPM";
                }
            }

            return "";
        }
        
        public async Task<Dictionary<string, int>> GetAssignedCallsOnWeekDaysSqlRaw(string staffId)
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

            DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);
            
            DateTime currentDate = dubaiTime;

            DateTime weekStart = currentDate.AddDays(-(int)currentDate.DayOfWeek);
            DateTime weekEnd = weekStart.AddDays(6);

            Dictionary<string, int> leadsByDayOfWeek = new Dictionary<string, int>
            {
                {"Sunday", 0},
                {"Monday", 0},
                {"Tuesday", 0},
                {"Wednesday", 0},
                {"Thursday", 0},
                {"Friday", 0},
                {"Saturday", 0}
            };

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var query = $"SELECT * FROM tblCallInsights WHERE AssignedTo = @staffid AND calledOn IS NOT NULL AND calledOn >= @WeekStart AND calledOn <= @WeekEnd";

                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@WeekStart", weekStart);
                    command.Parameters.AddWithValue("@WeekEnd", weekEnd);
                    command.Parameters.AddWithValue("@staffid", staffId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            DateTime assignon = reader.GetDateTime(reader.GetOrdinal("calledOn"));
                            string dayOfWeek = assignon.DayOfWeek.ToString();
                            leadsByDayOfWeek[dayOfWeek]++;
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return leadsByDayOfWeek;
        }

        public async Task<IEnumerable<dynamic>> GetAllLeadsSqlRaw()
        {
            List<dynamic> paymentSchedules = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var query = "SELECT * FROM tblleads";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dynamic paymentSchedule = new System.Dynamic.ExpandoObject();
                            paymentSchedule.LeadNo = reader.IsDBNull(reader.GetOrdinal("leadno")) ? null : reader.GetString(reader.GetOrdinal("leadno"));
                            paymentSchedule.SourceId = reader.IsDBNull(reader.GetOrdinal("sourceid")) ? null : reader.GetString(reader.GetOrdinal("sourceid"));
                            paymentSchedule.CampaignId = reader.IsDBNull(reader.GetOrdinal("campainid")) ? null : reader.GetString(reader.GetOrdinal("campainid"));
                            paymentSchedule.Name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name"));
                            paymentSchedule.Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone"));
                            paymentSchedule.Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email"));
                            paymentSchedule.OtherNo = reader.IsDBNull(reader.GetOrdinal("otherno")) ? null : reader.GetString(reader.GetOrdinal("otherno"));
                            paymentSchedule.Assigned = reader.IsDBNull(reader.GetOrdinal("assigned")) ? null : reader.GetString(reader.GetOrdinal("assigned"));
                            paymentSchedule.StaffId = reader.IsDBNull(reader.GetOrdinal("staffid")) ? 0 : reader.GetInt32(reader.GetOrdinal("staffid"));
                            paymentSchedule.ReceivedOn = reader.IsDBNull(reader.GetOrdinal("recievedon")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("recievedon"));
                            paymentSchedule.AssignedOn = reader.IsDBNull(reader.GetOrdinal("assignon")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("assignon"));
                            paymentSchedule.LeadStatus = reader.IsDBNull(reader.GetOrdinal("importance")) ? null : reader.GetString(reader.GetOrdinal("importance"));
                            paymentSchedule.Called = reader.IsDBNull(reader.GetOrdinal("called")) ? 0 : reader.GetInt32(reader.GetOrdinal("called"));
                            paymentSchedule.Status = reader.IsDBNull(reader.GetOrdinal("status")) ? 0 : reader.GetInt32(reader.GetOrdinal("status"));
                            paymentSchedule.ContactMethod = reader.IsDBNull(reader.GetOrdinal("contactMethod")) ? 0 : reader.GetInt32(reader.GetOrdinal("contactMethod"));
                            paymentSchedules.Add(paymentSchedule);
                        }
                    }
                }
            }

            return paymentSchedules;
        }

        public async Task<int> GetConversionCount()
        {
            return await _db.Tblleads.Where(x => x.Importance == 7).CountAsync();
        }

        public async Task<int> GetHotLeadCount()
        {
            return await _db.Tblleads.Where(x => x.Importance == 15).CountAsync();
        }

        public async Task<int> GetLeadCount()
        {
            return await _db.Tblleads.CountAsync();
        }

        public async Task<IEnumerable<dynamic>> GetLeadListView()
        {
            List<dynamic> paymentSchedules = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM vLeadList", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dynamic paymentSchedule = new System.Dynamic.ExpandoObject();
                            paymentSchedule.LeadNo = reader.IsDBNull(reader.GetOrdinal("leadno")) ? null : reader.GetString(reader.GetOrdinal("leadno"));
                            paymentSchedule.Source = reader.IsDBNull(reader.GetOrdinal("Source")) ? null : reader.GetString(reader.GetOrdinal("Source"));
                            paymentSchedule.Campaign = reader.IsDBNull(reader.GetOrdinal("campainid")) ? null : reader.GetString(reader.GetOrdinal("campainid"));
                            paymentSchedule.name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name"));
                            paymentSchedule.phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone"));
                            paymentSchedule.email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email"));
                            paymentSchedule.otherno = reader.IsDBNull(reader.GetOrdinal("otherno")) ? null : reader.GetString(reader.GetOrdinal("otherno"));
                            paymentSchedule.staffName = reader.IsDBNull(reader.GetOrdinal("staffname")) ? null : reader.GetString(reader.GetOrdinal("staffname"));
                            paymentSchedule.receivedOn = reader.IsDBNull(reader.GetOrdinal("recievedon")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("recievedon"));
                            paymentSchedule.assignon = reader.IsDBNull(reader.GetOrdinal("assignon")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("assignon"));
                            paymentSchedule.leadstatus = reader.IsDBNull(reader.GetOrdinal("leadstatus")) ? null : reader.GetString(reader.GetOrdinal("leadstatus"));
                            paymentSchedule.called = reader.IsDBNull(reader.GetOrdinal("called")) ? 0 : reader.GetInt32(reader.GetOrdinal("called"));
                            paymentSchedule.contactMethod = reader.IsDBNull(reader.GetOrdinal("ContactMethod")) ? null : reader.GetString(reader.GetOrdinal("ContactMethod"));
                            paymentSchedules.Add(paymentSchedule);
                        }
                    }
                }
            }

            return paymentSchedules;
        }
        
        public async Task<IEnumerable<dynamic>> GetLeadListViewForNewFiltered(int importance)
        {
            List<dynamic> paymentSchedules = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                var leadStatus = await _db.TblLeadStatuses.FirstOrDefaultAsync(x => x.Id == importance);

                using (SqlCommand command = new SqlCommand("SELECT * FROM vLeadList WHERE staffname IS NOT NULL AND leadstatus = '"+ leadStatus.Leadstatus +"' ORDER BY assignon DESC;", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dynamic paymentSchedule = new System.Dynamic.ExpandoObject();

                            paymentSchedule.LeadNo = reader.IsDBNull(reader.GetOrdinal("leadno")) ? null : reader.GetString(reader.GetOrdinal("leadno"));
                            paymentSchedule.Source = reader.IsDBNull(reader.GetOrdinal("Source")) ? null : reader.GetString(reader.GetOrdinal("Source"));
                            paymentSchedule.Campaign = reader.IsDBNull(reader.GetOrdinal("campainid")) ? null : reader.GetString(reader.GetOrdinal("campainid"));
                            paymentSchedule.name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name"));
                            paymentSchedule.phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone"));
                            paymentSchedule.email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email"));
                            paymentSchedule.otherno = reader.IsDBNull(reader.GetOrdinal("otherno")) ? null : reader.GetString(reader.GetOrdinal("otherno"));
                            paymentSchedule.staffName = reader.IsDBNull(reader.GetOrdinal("staffname")) ? null : reader.GetString(reader.GetOrdinal("staffname"));
                            paymentSchedule.receivedOn = reader.IsDBNull(reader.GetOrdinal("recievedon")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("recievedon"));
                            paymentSchedule.assignon = reader.IsDBNull(reader.GetOrdinal("assignon")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("assignon"));
                            paymentSchedule.leadstatus = reader.IsDBNull(reader.GetOrdinal("leadstatus")) ? null : reader.GetString(reader.GetOrdinal("leadstatus"));
                            paymentSchedule.called = reader.IsDBNull(reader.GetOrdinal("called")) ? 0 : reader.GetInt32(reader.GetOrdinal("called"));
                            paymentSchedule.contactMethod = reader.IsDBNull(reader.GetOrdinal("ContactMethod")) ? null : reader.GetString(reader.GetOrdinal("ContactMethod"));
                            paymentSchedule.comment = reader.IsDBNull(reader.GetOrdinal("comments")) ? null : reader.GetString(reader.GetOrdinal("comments"));
                            
                            paymentSchedules.Add(paymentSchedule);
                        }

                    }
                }
            }

            return paymentSchedules;
        }

        public async Task<IEnumerable<dynamic>> GetLeadListViewForNewFilteredByStaff(int staffId)
        {
            List<dynamic> paymentSchedules = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                var staff = await _db.Tblleads.FirstOrDefaultAsync(x => x.Staffid == staffId);

                using (SqlCommand command = new SqlCommand("SELECT * FROM vLeadList WHERE staffname IS NOT NULL AND staffid = '"+ staff.Staffid +"' ORDER BY assignon DESC;", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dynamic paymentSchedule = new System.Dynamic.ExpandoObject();

                            paymentSchedule.LeadNo = reader.IsDBNull(reader.GetOrdinal("leadno")) ? null : reader.GetString(reader.GetOrdinal("leadno"));
                            paymentSchedule.Source = reader.IsDBNull(reader.GetOrdinal("Source")) ? null : reader.GetString(reader.GetOrdinal("Source"));
                            paymentSchedule.Campaign = reader.IsDBNull(reader.GetOrdinal("campainid")) ? null : reader.GetString(reader.GetOrdinal("campainid"));
                            paymentSchedule.name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name"));
                            paymentSchedule.phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone"));
                            paymentSchedule.email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email"));
                            paymentSchedule.otherno = reader.IsDBNull(reader.GetOrdinal("otherno")) ? null : reader.GetString(reader.GetOrdinal("otherno"));
                            paymentSchedule.staffName = reader.IsDBNull(reader.GetOrdinal("staffname")) ? null : reader.GetString(reader.GetOrdinal("staffname"));
                            paymentSchedule.receivedOn = reader.IsDBNull(reader.GetOrdinal("recievedon")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("recievedon"));
                            paymentSchedule.assignon = reader.IsDBNull(reader.GetOrdinal("assignon")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("assignon"));
                            paymentSchedule.leadstatus = reader.IsDBNull(reader.GetOrdinal("leadstatus")) ? null : reader.GetString(reader.GetOrdinal("leadstatus"));
                            paymentSchedule.called = reader.IsDBNull(reader.GetOrdinal("called")) ? 0 : reader.GetInt32(reader.GetOrdinal("called"));
                            paymentSchedule.contactMethod = reader.IsDBNull(reader.GetOrdinal("ContactMethod")) ? null : reader.GetString(reader.GetOrdinal("ContactMethod"));
                            paymentSchedule.comment = reader.IsDBNull(reader.GetOrdinal("comments")) ? null : reader.GetString(reader.GetOrdinal("comments"));
                            
                            paymentSchedules.Add(paymentSchedule);
                        }

                    }
                }
            }

            return paymentSchedules;
        }
        
        public async Task AddNewLeadAsync(Tbllead tbllead)
        {
            await _db.Tblleads.AddAsync(tbllead);
        }

        public int AddNewLead(Tbllead tbllead)
        {
            DAL dAL = new DAL(_configuration);
            SqlParameter[] sQlParameters = new SqlParameter[]
            {
                new SqlParameter("@LeadNo", tbllead.Leadno),
                new SqlParameter("@SourceID", tbllead.Sourceid),
                new SqlParameter("@CampaignID", tbllead.Campainid),
                new SqlParameter("@Name", tbllead.Name),
                new SqlParameter("@Phone", tbllead.Phone),
                new SqlParameter("@Email", tbllead.Email),
                new SqlParameter("@OtherNo", tbllead.Otherno),
                new SqlParameter("@Assigned", tbllead.Assigned),
                new SqlParameter("@StaffID", tbllead.Staffid),
                new SqlParameter("@ReceivedOn", tbllead.Recievedon),
                new SqlParameter("@AssignOn", tbllead.Assignon),
                new SqlParameter("@Importance", tbllead.Importance),
                new SqlParameter("@Called", tbllead.Called),
                new SqlParameter("@Status", tbllead.Status),
                new SqlParameter("@ContactMethod", tbllead.ContactMethod),
                new SqlParameter("@CrossSegmentLead", tbllead.CrossSegmentLead),
                new SqlParameter("@Attending", tbllead.Attending),
                new SqlParameter("@RsvpTypeId", tbllead.RsvpTypeId),
                new SqlParameter("@Comments", tbllead.Comments),
                new SqlParameter("@InterestedIn", tbllead.InterestedIn),
                new SqlParameter("@PlanToDo", tbllead.PlanToDo),
                new SqlParameter("@PlanToDoWhen", tbllead.PlanToDoWhen),
                new SqlParameter("@IsLost", tbllead.IsLost),
                new SqlParameter("@AddedOn", tbllead.AddedOn),
                new SqlParameter("@IsInterested", tbllead.IsInterested),
            };

            int res = dAL.ExecuteNonQueryStoredProcedure("InsertIntoTblLeads", sQlParameters);
            return res;
        }

        public int updateLead(Tbllead tbllead)
        {
            DAL dAL = new DAL(_configuration);
            SqlParameter[] sQlParameters = new SqlParameter[]
            {
                new SqlParameter("@LeadNo", tbllead.Leadno),
                new SqlParameter("@SourceID", tbllead.Sourceid),
                new SqlParameter("@CampaignID", tbllead.Campainid),
                new SqlParameter("@Name", tbllead.Name),
                new SqlParameter("@Phone", tbllead.Phone),
                new SqlParameter("@Email", tbllead.Email),
                new SqlParameter("@OtherNo", tbllead.Otherno),
                new SqlParameter("@Assigned", tbllead.Assigned),
                new SqlParameter("@StaffID", tbllead.Staffid),
                new SqlParameter("@ReceivedOn", tbllead.Recievedon),
                new SqlParameter("@AssignOn", tbllead.Assignon),
                new SqlParameter("@Importance", tbllead.Importance),
                new SqlParameter("@Called", tbllead.Called),
                new SqlParameter("@Status", tbllead.Status),
                new SqlParameter("@ContactMethod", tbllead.ContactMethod),
                new SqlParameter("@CrossSegmentLead", tbllead.CrossSegmentLead),
                new SqlParameter("@Attending", tbllead.Attending),
                new SqlParameter("@RsvpTypeId", tbllead.RsvpTypeId),
                new SqlParameter("@Comments", tbllead.Comments),
                new SqlParameter("@InterestedIn", tbllead.InterestedIn),
                new SqlParameter("@PlanToDo", tbllead.PlanToDo),
                new SqlParameter("@PlanToDoWhen", tbllead.PlanToDoWhen),
                new SqlParameter("@IsLost", tbllead.IsLost),
                new SqlParameter("@AddedOn", tbllead.AddedOn),
                new SqlParameter("@IsInterested", tbllead.IsInterested),
            };

            int res = dAL.ExecuteNonQueryStoredProcedure("UpdateTblLeads", sQlParameters);
            return res;
        }

        public List<VLeads> GetAllDndLeads()
        {
            List<VLeads> branches = new List<VLeads>();

            DAL dAL = new DAL(_configuration);
            SqlParameter[] parameters = new SqlParameter[]
            {
            };
        
            DataTable result = dAL.ExecuteStoredProcedure("GetAllDndLeads", parameters);

            foreach (DataRow item in result.Rows)
            {
                
                VLeads branch = new VLeads
                {
                    LeadNo = item["leadno"] != DBNull.Value ? Convert.ToString(item["leadno"]) : "",
                    Name = item["name"] != DBNull.Value ? Convert.ToString(item["name"]) : "",
                    Phone = item["phone"] != DBNull.Value ? Convert.ToString(item["phone"]) : "",
                    Email = item["email"] != DBNull.Value ? Convert.ToString(item["email"]) : "",
                    OtherNo = item["otherno"] != DBNull.Value ? Convert.ToString(item["otherno"]) : "",
                    ReceivedOn = item["recievedon"] != DBNull.Value ? Convert.ToDateTime(item["recievedon"]) : DateTime.MinValue,
                    AssignOn = item["assignon"] != DBNull.Value ? Convert.ToDateTime(item["assignon"]) : DateTime.MinValue,
                    Called = item["called"] != DBNull.Value ? Convert.ToInt32(item["called"]) : 0,
                    Status = item["status"] != DBNull.Value ? Convert.ToInt32(item["status"]) : 0,
                    Source = item["Source"] != DBNull.Value ? Convert.ToString(item["Source"]) : "",
                    StaffName = item["staffname"] != DBNull.Value ? Convert.ToString(item["staffname"]) : "",
                    ContactMethod = item["ContactMethod"] != DBNull.Value ? Convert.ToString(item["ContactMethod"]) : "",
                    LeadStatus = item["leadstatus"] != DBNull.Value ? Convert.ToString(item["leadstatus"]) : "",
                    CampaignId = item["campainid"] != DBNull.Value ? Convert.ToString(item["campainid"]) : "",
                    Comments = item["comments"] != DBNull.Value ? Convert.ToString(item["comments"]) : "",
                    Importance = item["importance"] != DBNull.Value ? Convert.ToInt32(item["importance"]) : 0
                };

            
                branches.Add(branch);
            }
            
            return branches;
        }

        public async Task<IEnumerable<dynamic>> GetLeadListViewForNew()
        {
            List<dynamic> paymentSchedules = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM vLeadList WHERE staffname IS NOT NULL AND status = 0 AND importance != 18 AND importance != 24 AND importance != 19 ORDER BY recievedon DESC;", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dynamic paymentSchedule = new System.Dynamic.ExpandoObject();

                            paymentSchedule.LeadNo = reader.IsDBNull(reader.GetOrdinal("leadno")) ? null : reader.GetString(reader.GetOrdinal("leadno"));
                            paymentSchedule.Source = reader.IsDBNull(reader.GetOrdinal("Source")) ? null : reader.GetString(reader.GetOrdinal("Source"));
                            paymentSchedule.Campaign = reader.IsDBNull(reader.GetOrdinal("campainid")) ? null : reader.GetString(reader.GetOrdinal("campainid"));
                            paymentSchedule.name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name"));
                            paymentSchedule.phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone"));
                            paymentSchedule.email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email"));
                            paymentSchedule.otherno = reader.IsDBNull(reader.GetOrdinal("otherno")) ? null : reader.GetString(reader.GetOrdinal("otherno"));
                            paymentSchedule.staffName = reader.IsDBNull(reader.GetOrdinal("staffname")) ? null : reader.GetString(reader.GetOrdinal("staffname"));
                            paymentSchedule.receivedOn = reader.IsDBNull(reader.GetOrdinal("recievedon")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("recievedon"));
                            paymentSchedule.assignon = reader.IsDBNull(reader.GetOrdinal("assignon")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("assignon"));
                            paymentSchedule.leadstatus = reader.IsDBNull(reader.GetOrdinal("leadstatus")) ? null : reader.GetString(reader.GetOrdinal("leadstatus"));
                            paymentSchedule.called = reader.IsDBNull(reader.GetOrdinal("called")) ? 0 : reader.GetInt32(reader.GetOrdinal("called"));
                            paymentSchedule.contactMethod = reader.IsDBNull(reader.GetOrdinal("ContactMethod")) ? null : reader.GetString(reader.GetOrdinal("ContactMethod"));
                            paymentSchedule.comment = reader.IsDBNull(reader.GetOrdinal("comments")) ? null : reader.GetString(reader.GetOrdinal("comments"));
                            
                            paymentSchedules.Add(paymentSchedule);
                        }

                    }
                }
            }

            return paymentSchedules;
        }
        
        public async Task<Tbllead> GetLeadsByIdAsync(string id)
        {
            return await _db.Tblleads.FirstOrDefaultAsync(x => x.Leadno == id);
        }

        public async Task<Tbllead> GetLeadsByNameAsync(string name)
        {
            return await _db.Tblleads.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<int> GetOpenLeadCount()
        {
            return await _db.Tblleads.Where(x => x.Importance == 14).CountAsync();
        }

        public async Task<List<dynamic>> GetAllLeadsByStaffIdAndImportance(int staffId, int importance)
        {
            List<dynamic> paymentSchedules = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM vLeadList WHERE staffname IS NOT NULL AND staffid = '"+ staffId +"' AND importance = '"+ importance +"' ORDER BY assignon DESC;", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dynamic paymentSchedule = new System.Dynamic.ExpandoObject();

                            paymentSchedule.LeadNo = reader.IsDBNull(reader.GetOrdinal("leadno")) ? null : reader.GetString(reader.GetOrdinal("leadno"));
                            paymentSchedule.Source = reader.IsDBNull(reader.GetOrdinal("Source")) ? null : reader.GetString(reader.GetOrdinal("Source"));
                            paymentSchedule.Campaign = reader.IsDBNull(reader.GetOrdinal("campainid")) ? null : reader.GetString(reader.GetOrdinal("campainid"));
                            paymentSchedule.name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name"));
                            paymentSchedule.phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString(reader.GetOrdinal("phone"));
                            paymentSchedule.email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString(reader.GetOrdinal("email"));
                            paymentSchedule.otherno = reader.IsDBNull(reader.GetOrdinal("otherno")) ? null : reader.GetString(reader.GetOrdinal("otherno"));
                            paymentSchedule.staffName = reader.IsDBNull(reader.GetOrdinal("staffname")) ? null : reader.GetString(reader.GetOrdinal("staffname"));
                            paymentSchedule.receivedOn = reader.IsDBNull(reader.GetOrdinal("recievedon")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("recievedon"));
                            paymentSchedule.assignon = reader.IsDBNull(reader.GetOrdinal("assignon")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("assignon"));
                            paymentSchedule.leadstatus = reader.IsDBNull(reader.GetOrdinal("leadstatus")) ? null : reader.GetString(reader.GetOrdinal("leadstatus"));
                            paymentSchedule.called = reader.IsDBNull(reader.GetOrdinal("called")) ? 0 : reader.GetInt32(reader.GetOrdinal("called"));
                            paymentSchedule.contactMethod = reader.IsDBNull(reader.GetOrdinal("ContactMethod")) ? null : reader.GetString(reader.GetOrdinal("ContactMethod"));
                            paymentSchedule.comment = reader.IsDBNull(reader.GetOrdinal("comments")) ? null : reader.GetString(reader.GetOrdinal("comments"));
                            
                            paymentSchedules.Add(paymentSchedule);
                        }

                    }
                }
            }

            return paymentSchedules;
        }
    }
}
