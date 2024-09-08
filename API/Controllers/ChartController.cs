using API.Models;
using API.Repos;
using API.Repos.Dtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public readonly ResponseDto _response;

        public ChartController(CRMContext context, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _db = context;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _response = new ResponseDto();
        }

        [HttpPost("GetCallAvailableStatisticData")]
        public async Task<ResponseDto> GetCallAvailableStatisticData(AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var currentStaff = await _db.Tblstaffs.FirstOrDefaultAsync(x => x.Id == authResponse.Result.Userid);

                if (currentStaff == null)
                {
                    _response.Message = "Error while trying to find staff";
                    _response.IsSuccess = false;
                    return _response;
                }

                if (currentStaff.Designation == "CEO")
                {
                    var count = _unitOfWork.chartInterface.GetTotalCallsLeftAdmin();

                    _response.IsSuccess = true;
                    _response.Message = "Count retrieved successfully.";
                    _response.Result = new
                    {
                        ConvertedLeadsCount = count,
                        ConversionPercentage = 2
                    };

                    return _response;
                }
                else
                {
                    var count = _unitOfWork.chartInterface.GetTotalCallsLeft(currentStaff.Id);

                    _response.IsSuccess = true;
                    _response.Message = "Count retrieved successfully.";
                    _response.Result = new
                    {
                        ConvertedLeadsCount = count,
                        ConversionPercentage = 2
                    };

                    return _response;
                }

            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating lead! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetLeadLostData")]
        public async Task<ResponseDto> GetLostLeadData(AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var currentStaff = await _db.Tblstaffs.FirstOrDefaultAsync(x => x.Id == authResponse.Result.Userid);

                if (currentStaff == null)
                {
                    _response.IsSuccess = false;
                    return _response;
                }

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    var existingLeadStatus = await _db.TblLeadStatuses.FirstOrDefaultAsync(x => x.Leadstatus == "Lost Lead");

                    if (existingLeadStatus == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Lead status with importance 8 does not exist";
                        return _response;
                    }

                    string query;

                    if (currentStaff.Designation == "CEO")
                    {
                        query = $"select count(*) from tblleads where importance = {existingLeadStatus.Id};";
                    }
                    else
                    {
                        query = $"select count(*) from tblleads where importance = {existingLeadStatus.Id} AND assigned = {currentStaff.Id};";
                    }

                    SqlCommand command = new SqlCommand(query, connection);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection))
                    {
                        while (await reader.ReadAsync())
                        {
                            var convertedLeadsCount = reader.GetInt32(0);

                            if (currentStaff.Designation == "CEO")
                            {
                                _response.Message = "Successfully retrieved all lost lead data.";
                                _response.Result = new
                                {
                                    convertedLeadsCount = reader.GetInt32(0),
                                    ConversionPercentage = 100,
                                };
                                return _response;
                            }
                            else
                            {
                                DateTime utcNow = DateTime.UtcNow;
                                TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

                                DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);
                                
                                var currentDate = dubaiTime.Date;
                                var totalAssignedLeadsCount = await _db.Tblleads
                                    .Where(x => x.Recievedon.Date == currentDate && x.Staffid == authResponse.Result.Userid)
                                    .CountAsync();

                                double conversionPercentage = totalAssignedLeadsCount != 0
                                    ? (double)convertedLeadsCount / totalAssignedLeadsCount * 100
                                    : 0;

                                _response.Message = "Successfully retrieved lead data for the current staff.";
                                _response.Result = new
                                {
                                    ConvertedLeadsCount = convertedLeadsCount,
                                    ConversionPercentage = Math.Round(conversionPercentage, 2).ToString("0.00")
                                };

                            }

                            _response.IsSuccess = true;

                            return _response;
                        }
                    }

                    _response.Message = "Error occurred while processing command";
                    _response.IsSuccess = false;
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating lead! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetLeadStatisticData")]
        public async Task<ResponseDto> GetStatisticData(AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var currentStaff = await _db.Tblstaffs.FirstOrDefaultAsync(x => x.Id == authResponse.Result.Userid);

                if (currentStaff == null)
                {
                    _response.IsSuccess = false;
                    return _response;
                }
                            
                if (currentStaff.Designation == "CEO")
                {
                    var monthlyCount = _unitOfWork.chartInterface.GeControltMonthlyCallsTargetAccordingToStaff(currentStaff.Id);
                    var leadCount = _unitOfWork.chartInterface.GetLeadsConversionsTodayAdmin();

                    int targetLeadsPerDay = monthlyCount;
                    int convertedLeadsCount = leadCount;

                    double conversionPercentage = targetLeadsPerDay != 0
                    ? (double)convertedLeadsCount / targetLeadsPerDay * 100
                    : 0;
                                
                    double roundedConversionPercentage = Math.Round(conversionPercentage, 2);

                    _response.Message = "Successfully retrieved lead data.";
                    _response.IsSuccess = true;
                    _response.Result = new
                    {
                        ConvertedLeadsCount = convertedLeadsCount,
                        ConversionPercentage = roundedConversionPercentage
                    };

                    return _response;
                }
                else
                {
                    
                    var monthlyCount = _unitOfWork.chartInterface.GeControltMonthlyCallsTargetAccordingToStaff(currentStaff.Id);
                    var leadCount = _unitOfWork.chartInterface.GetLeadsConversionsTodayUser(currentStaff.Id);

                    int targetLeadsPerDay = monthlyCount;
                    int convertedLeadsCount = leadCount;

                    double conversionPercentage = targetLeadsPerDay != 0
                    ? (double)convertedLeadsCount / targetLeadsPerDay * 100
                    : 0;
                                
                    double roundedConversionPercentage = Math.Round(conversionPercentage, 2);

                    _response.Message = "Successfully retrieved lead data.";
                    _response.IsSuccess = true;
                    _response.Result = new
                    {
                        ConvertedLeadsCount = convertedLeadsCount,
                        ConversionPercentage = roundedConversionPercentage
                    };

                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating lead! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }


        [HttpPost("GetCallsStatisticData")]
        public async Task<ResponseDto> GetCallStatisticData(AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var currentStaff = await _db.Tblstaffs.FirstOrDefaultAsync(x => x.Id == authResponse.Result.Userid);

                if (currentStaff == null)
                {
                    _response.IsSuccess = false;
                    return _response;
                }

                if (currentStaff.Designation == "CEO")
                {

                    var dialedCalled = _unitOfWork.chartInterface.GetCallsLeftUserAdmin();
                    var targetLeads = _unitOfWork.chartInterface.GetControlCallsMonthlyTargetAccordingToStaff(currentStaff.Id);

                    int targetLeadsPerDay = targetLeads;
                    int convertedLeadsCount = dialedCalled;
                    double conversionPercentage = targetLeadsPerDay != 0
                    ? (double)convertedLeadsCount / targetLeadsPerDay * 100
                    : 0;

                    _response.Message = "Successfully retrieved call data.";
                    _response.IsSuccess = true;
                    _response.Result = new
                    {
                        ConvertedLeadsCount = convertedLeadsCount,
                        ConversionPercentage = conversionPercentage
                    };

                    return _response;
                }
                else
                {
                    var dialedCalled = _unitOfWork.chartInterface.GetCallsLeftUser(currentStaff.Id);
                    var targetLeads = _unitOfWork.chartInterface.GetControlCallsMonthlyTargetAccordingToStaff(currentStaff.Id);

                    int targetLeadsPerDay = targetLeads;
                    int convertedLeadsCount = dialedCalled;
                    double conversionPercentage = targetLeadsPerDay != 0
                    ? (double)convertedLeadsCount / targetLeadsPerDay * 100
                    : 0;

                    _response.Message = "Successfully retrieved call data.";
                    _response.IsSuccess = true;
                    _response.Result = new
                    {
                        ConvertedLeadsCount = convertedLeadsCount,
                        ConversionPercentage = conversionPercentage
                    };

                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating lead! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("UpdateLeadDailyCount")]
        public async Task<ResponseDto> UpdateLeadDailyCount([FromQuery]int count, [FromBody] AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    var query = $"UPDATE tblcontrol SET lead_stats = @count";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@count", count);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        await connection.CloseAsync();
                        _response.Message = "Control updated successfully.";
                        _response.IsSuccess = true;
                        return _response;
                    }

                    await connection.CloseAsync();

                    _response.Message = "Error while updating lead";
                    _response.IsSuccess = false;
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating control table. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("UpdateCallDailyCount")]
        public async Task<ResponseDto> UpdateCallDailyCount([FromQuery] int count, [FromBody] AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    var query = $"UPDATE tblcontrol SET call_stats = @count";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@count", count);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        await connection.CloseAsync();
                        _response.Message = "Control updated successfully.";
                        _response.IsSuccess = true;
                        return _response;
                    }

                    await connection.CloseAsync();

                    _response.Message = "Error while updating call";
                    _response.IsSuccess = false;
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating control table. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPut("UpdateCountDaily")]
        public async Task<ResponseDto> UpdateCountDaily([FromBody] AuthDto authDto, [FromQuery] int count, [FromQuery] string type, [FromQuery] int staffId)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            var newUserPermission = new SendGetUserPermission
            {
                Event = Event.Edit.ToString(),
                Location = AccessLocation.ChangeDailyAmount.ToString()
            };

            var hasPermission = await _unitOfWork.userpermissionInterface.GetUserPermission(newUserPermission, authResponse.Result.Userid.ToString());

            if (!hasPermission.HasPermission)
            {
                _response.Message = "Access Denied";
                _response.IsSuccess = false;
                _response.Result = "";
                return _response;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    var query = "";

                    if (type == "Lead")
                    {
                        query += "update tblstaffs set tblstaffs.monthlyTarget = @amount WHERE id = @staffId";
                    }
                    else if (type == "Calls")
                    {
                        query += "update tblstaffs set tblstaffs.callsMonthlyTarget = @amount WHERE id = @staffId";
                    }

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@amount", count);
                    command.Parameters.AddWithValue("@staffId", staffId);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        await connection.CloseAsync();
                        _response.Message = "Control updated successfully.";
                        _response.IsSuccess = true;
                        return _response;
                    }

                    await connection.CloseAsync();

                    _response.Message = "Error while updating call";
                    _response.IsSuccess = false;
                    return _response;

                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Error while updating! " + ex.Message; 
                return _response;
            }
        }

        [HttpPost("GetConversionsForTimeSlots")]
        public async Task<ResponseDto> GetBarChartData(AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {

                var existingStaff = await _db.Tblstaffs.FirstOrDefaultAsync(x => x.Id == authResponse.Result.Userid);

                if (existingStaff == null)
                {
                    _response.Message = "Unable to find staff with if";
                    _response.IsSuccess = false;
                    return _response;
                }
                
                var leads = await _unitOfWork.leadsInterface.GetCallDurationCategoriesByTimeSlotAndImportanceWithDND(authResponse.Result.Userid.ToString(), existingStaff.Designation);

                _response.Result = leads;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting pie chart data. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("CallsMadeAccordingToYesterday")]
        public async Task<ResponseDto> CallsMadeAccordingToYesterday(AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

                DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);
                
                DateTime yesterday = dubaiTime.Date.AddDays(-1);
                DateTime today = dubaiTime.Date;

                var assignedCallsYesterday = await _db.TblCallInsights
                    .Where(call => call.AssignedOn != null && call.AssignedOn.Value.Date == yesterday && call.AssignedTo == authResponse.Result.Userid.ToString() && call.CalledOn != null)
                    .ToListAsync();

                var currentDayCalls = await _db.TblCallInsights
                    .Where(call => call.CalledOn != null && call.CalledOn.Value.Date == today && call.AssignedTo == authResponse.Result.Userid.ToString())
                    .ToListAsync();

                int yesterdayCount = assignedCallsYesterday.Count;
                int todayCount = currentDayCalls.Count;

                double percentage = 0.0;

                if (yesterdayCount > 0)
                {
                    percentage = ((double)todayCount / yesterdayCount) * 100;
                }

                _response.Result = new
                {
                    todaysCalls = todayCount,
                    yesterCalls = yesterdayCount,
                    percentageChange = percentage
                };

                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting pie chart data. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }
        
        [HttpPost("GetConversionsForTimes")]
        public async Task<ResponseDto> GetBarChartDataTime(AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var leads = await _unitOfWork.leadsInterface.GetAverageCallDurationByDayOfWeek(authResponse.Result.Userid.ToString());

                _response.Result = leads;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting pie chart data. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetAssignedConversionsForDates")]
        public async Task<ResponseDto> GetBarChartPieData(AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var leads = await _unitOfWork.leadsInterface.GetAssignedCallsOnWeekDaysSqlRaw(authResponse.Result.Userid.ToString());
                _response.Result = leads;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting pie chart data. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetPieChartData")]
        public async Task<ResponseDto> GetPieChartData(AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var currentStaff = await _db.Tblstaffs.FirstOrDefaultAsync(x => x.Id == authResponse.Result.Userid);

                if (currentStaff == null)
                {
                    _response.Message = "Staff with this id does not exist";
                    _response.IsSuccess = false;
                    return _response;
                }

                if (currentStaff.Designation == "CEO")
                {
                    var assignedCallsQuery = _unitOfWork.chartInterface.GetTotalCallsLeftAdmin();
                    var assignedButCalledQuery = _unitOfWork.chartInterface.GetCallsLeftUserAdmin();
                    var conversionsQuery = _unitOfWork.chartInterface.GetLeadsConversionsTodayAdmin();

                    _response.Message = "";
                    _response.Result = new
                    {
                        AssignedCallsButCalled = assignedButCalledQuery,
                        CallsToMake = assignedCallsQuery,
                        Conversions = conversionsQuery,
                    };

                    return _response;
                }
                else
                {
                    var assignedCallsQuery = _unitOfWork.chartInterface.GetTotalCallsLeft(currentStaff.Id);
                    var assignedButCalledQuery = _unitOfWork.chartInterface.GetCallsLeftUser(currentStaff.Id);
                    var conversion = _unitOfWork.chartInterface.GetLeadsConversionsTodayUser(currentStaff.Id);

                    _response.Message = "";
                    _response.Result = new
                    {
                        AssignedCallsButCalled = assignedButCalledQuery,
                        CallsToMake = assignedCallsQuery,
                        Conversions = conversion,
                    };

                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting pie chart data. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }
    
    }
}
