using API.Models;
using API.Repos;
using API.Repos.Dtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/performance")]
    [ApiController]
    public class EmployeePerformanceController : ControllerBase
    {
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseDto _response;

        public EmployeePerformanceController(CRMContext context, IUnitOfWork unitOfWork)
        {
            _db = context;
            _unitOfWork = unitOfWork;
            _response = new ResponseDto();
        }

        [HttpPost("allDateBased")]
        public async Task<ResponseDto> SeeAllEmployeePerformance([FromBody] DateFilterDto dateFilter)
        {
            try
            {
                var existingAllEmployees = await _db.Tblstaffs.Where(x => x.Designation == "Sales Person" && x.Status == 0).ToListAsync();
                var allLeads = await _db.Tblleads.ToListAsync();
                var allCallInsights = await _db.TblCallInsights.ToListAsync();

                if (existingAllEmployees.Count == 0)
                {
                    _response.Message = "Employees who are sales persons do not exist";
                    _response.IsSuccess = false;
                    _response.Result = existingAllEmployees;
                    return _response;
                }

                var staffPerformance = new List<StaffPerformance>();

                foreach (var employee in existingAllEmployees)
                {
                    int callsAssignedToEmployee = await _db.TblCallInsights
                        .Where(call => call.AssignedTo == employee.Id.ToString() &&
                                       call.AddOn >= dateFilter.StartDate &&
                                       call.AddOn <= dateFilter.EndDate)
                        .CountAsync();

                    var userImage = await _db.Tblusers.FirstOrDefaultAsync(x => x.Userid == employee.Id);

                    int convertedLeadsCount = 0;
                    int notCalledCount = 0;

                    foreach (var callInsight in allCallInsights)
                    {
                        if (callInsight.AssignedTo == employee.Id.ToString())
                        {
                            if (callInsight.CalledOn != null && callInsight.CallEndedOn != null)
                            {
                                if (allLeads.Any(lead => lead.Phone == callInsight.PhoneNo))
                                {
                                    convertedLeadsCount++;
                                }
                            }
                            else
                            {
                                notCalledCount++;
                            }
                        }
                    }

                    double conversionRatio = convertedLeadsCount / (double)callsAssignedToEmployee;

                    // var performance = new StaffPerformance
                    // {
                    //     FirstName = employee.Firstname,
                    //     LastName = employee.Lastname,
                    //     CallsAssigned = callsAssignedToEmployee,
                    //     ConvertedLeads = convertedLeadsCount,
                    //     NotCalled = notCalledCount,
                    //     Image = userImage.Userimage,
                    // };

                    // if (double.IsFinite(conversionRatio))
                    // {
                    //     performance.ConversionRatio = conversionRatio;
                    // }
                    // else
                    // {
                    //     performance.ConversionRatio = 0;
                    // }
                    //
                    //
                    // staffPerformance.Add(performance);
                }

                _response.Result = staffPerformance;

                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting employee performance! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("reportToday")]
        public async Task<ResponseDto> ReturnHtmlReport([FromBody] AuthDto authDto)
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
                Event = Event.GetAll.ToString(),
                Location = AccessLocation.EmployeePerformance.ToString()
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
                                
                var today = DateTime.Today; // Get the start of today
                var endOfToday = today.AddDays(1); // Get the end of today
                var yesterday = today.AddDays(-1); // Get the start of yesterday
                
                var existingAllEmployees = await _db.Tblstaffs.Where(x => x.Designation == "Sales Person" && x.Status == 0).ToListAsync();
                var allLeads = await _db.Tblleads.Where(x => x.AddedOn >= yesterday && x.AddedOn < endOfToday).ToListAsync();
                var staffPerformance = new List<StaffInteractionReport>();
                var allLeadLogs = await _db.TblLeadlogs.Where(x => x.Addon >= yesterday && x.Addon < endOfToday).ToListAsync();

                foreach (var employee in existingAllEmployees)
                {
                    int callsAssignedToEmployee = await _db.TblCallInsights
                        .Where(call => call.AssignedTo == employee.Id.ToString() && call.AssignedOn >= today && call.CalledOn != null)
                        .CountAsync();

                    var userImage = await _db.Tblusers.FirstOrDefaultAsync(x => x.Userid == employee.Id);
                    
                    int convertedLeadsToday = allLeads
                        .Where(lead => lead.Assigned == employee.Id && lead.IsInterested == 1 && lead.AddedOn >= today)
                        .Count();

                    int convertedLeadsYesterday = allLeads
                        .Where(lead => lead.Assigned == employee.Id && lead.IsInterested == 1 && lead.AddedOn >= yesterday && lead.AddedOn < today)
                        .Count();
                    
                    int notCalledCount = callsAssignedToEmployee - (convertedLeadsToday + convertedLeadsYesterday);

                    int hotLeadCount = allLeads
                        .Where(lead => lead.Importance == 20 && lead.Assigned == employee.Id)
                        .Count();

                    int leadLogCount = 0;
                    foreach (var leads in allLeads)
                    {
                        leadLogCount = allLeadLogs.Where(log => log.Leadid == leads.Leadno).Count();
                    }
                    
                    var performance = new StaffInteractionReport
                    {
                        FullName = employee.Firstname + " " + employee.Lastname,
                        Calls = callsAssignedToEmployee,
                        HotLeads = hotLeadCount,
                        MissedMonths = 0,
                        NoFollowUps = 0,
                        PlannedMeetings = 0,
                        RemarkCount = leadLogCount,
                        WeekFollowUp = 0,
                        YesterdayHotLeads = 0,
                    };

                    staffPerformance.Add(performance);
                }

                string html = @"
            <html>
            <head>
                <title>Paramount Real Estate</title>
               <style>
                    body { font-family: Tahoma; }
                    .header { display: flex; align-items: center; justify-content: center; padding: 20px; background-color: #f5f5f5; }
                    .logo { width: 100px; height: 100px; }
                    .company-info { display: flex; flex-direction: column; margin-left: 10px; text-align: center; }
                    .company-name { font-size: 24px; font-weight: bold; margin-bottom: 10px; }
                    .address { font-size: 16px; margin-bottom: 10px; }
                    .telephone { font-size: 16px; }
                    .containerheader { text-align: center; }
                    table { width: 100%; border-collapse: collapse; }
                    td, th { padding: 8px; text-align: left; }
                    th { background-color: #f2f2f2; }
                    .auto-style5, .auto-style6, .auto-style4 { width: 150px; }
                    .border { border: 1px solid #ccc; }  
                </style>
            </head>
            <body>
                <div class='header' style='text-align:left;'>

                    <div class='company-info'>
                        <div class='company-name'>Paramount Real Estate</div>
                        <div class='address'>1303, Grosvenor Business Tower - Barsha Heights - Dubai</div>
                        <div class='telephone'>04 591 3888</div>
                        <div>Staff interaction report.</div>
                    </div>
                </div>

                 <div class='report-container'>
                        <table>
                                   <tr>
                                       <th class='border' colspan=""9"" style=""text-align: center;"">Staff Data</th>
                                   </tr>
                            <tr>
                                <th class='border'>Employee Name</th>
                                <th class='border'>Calls(A)</th>
                                <th class='border'>F2F(P)</th>
                                <th class='border'>F2F(A)</th>
                                <th class='border'>SV(P)</th>
                                <th class='border'>SV(A)</th>
                                <th class='border'>VC(P)</th>
                                <th class='border'>VC(A)</th>
                                <th class='border'>Hot Update</th>
                                <th class='border'>UNQ-M</th>
                                <th class='border'>1 UTLK</th>
                                <th class='border'>FTTR</th>
                                <th class='border'>Listing Count(Q4/Q5)</th>
                                <th class='border'>Planned</th>
                                <th class='border'>Missed 3 Months</th>
                                <th class='border'>No Follow Up</th>
                                <th class='border'>Weak Follow Up</th>
                                <th class='border'>Yesterday Hot Update</th>
                                <th class='border'>Notes</th>
                            </tr>";


                foreach (var items in staffPerformance)
                {
                    html += $@"
                                <tr>
                                      <td class='border'>{items.FullName}</td>
                                      <td class='border'>{items.Calls}</td>
                                       <td class='border'>0</td>
                                      <td class='border'>0</td>
                                       <td class='border'>0</td>
                                       <td class='border'>0</td>
                                       <td class='border'>0</td>
                                       <td class='border'>0</td>
                                       <td class='border'>0</td>
                                       <td class='border'>0</td>
                                       <td class='border'>0</td>
                                       <td class='border'>0</td>
                                       <td class='border'>0</td>
                                       <td class='border'>0</td>
                                       <td class='border'>0</td>
                                       <td class='border'>0</td>
                                       <td class='border'>0</td>
                                       <td class='border'>0</td>
                                       <td class='border'>0</td>
                                </tr>
                                ";
                }

                html += $@"

                </table>
                </div>
                </body>
                </html>";

                _response.Result = html;

                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting employee performance! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }
        
        [HttpPost("reportWeekly")]
        public async Task<ResponseDto> ReturnHtmlReportWeekly([FromBody] AuthDto authDto)
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
                Event = Event.GetAll.ToString(),
                Location = AccessLocation.EmployeePerformance.ToString()
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
                var today = DateTime.Today;
                var endOfToday = today.AddDays(1);
                var yesterday = today.AddDays(-1);
                var startOfLastWeek = today.AddDays(-7);
                var endOfLastWeek = today;
                
                int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
                var startOfThisWeek = today.AddDays(-daysUntilMonday);
                var endOfThisWeek = startOfThisWeek.AddDays(7);


                var existingAllEmployees = await _db.Tblstaffs.Where(x => x.Designation == "Sales Person" && x.Status == 0).ToListAsync();
                var allLeads = await _db.Tblleads.Where(x => x.AddedOn >= startOfLastWeek && x.AddedOn < endOfToday).ToListAsync();
                var staffPerformance = new List<StaffPerformance>();

                foreach (var employee in existingAllEmployees)
                {
                    int callsAssignedToEmployee = await _db.TblCallInsights
                        .Where(call => call.AssignedTo == employee.Id.ToString() && call.AssignedOn >= startOfThisWeek && call.AssignedOn <= endOfThisWeek && call.CalledOn != null)
                        .CountAsync();

                    var userImage = await _db.Tblusers.FirstOrDefaultAsync(x => x.Userid == employee.Id);

                    int convertedLeadsThisWeek = allLeads
                        .Where(lead => lead.Assigned == employee.Id && lead.IsInterested == 1 && lead.AddedOn >= startOfThisWeek)
                        .Count();

                    int convertedLeadsLastWeek = allLeads
                        .Where(lead => lead.Assigned == employee.Id && lead.IsInterested == 1 && lead.AddedOn >= startOfLastWeek && lead.AddedOn < startOfThisWeek)
                        .Count();
                    
                    // int notCalledCount = callsAssignedToEmployee - (convertedLeadsThisWeek + convertedLeadsLastWeek);

                    var performance = new StaffPerformance
                    {
                        FirstName = employee.Firstname,
                        LastName = employee.Lastname,
                        CallsAssigned = callsAssignedToEmployee,
                        ConvertedLeadsToday = convertedLeadsThisWeek,
                        ConvertedLeadsYesterday = convertedLeadsLastWeek,
                        // NotCalled = notCalledCount,
                        Image = userImage.Userimage,
                    };
                    
                    double conversionRatio = CalculateConversionRatio(convertedLeadsThisWeek, convertedLeadsLastWeek);
                    performance.ConversionRatio = conversionRatio;

                    staffPerformance.Add(performance);
                }

                string html = @"
            <html>
            <head>
                <title>Paramount Real Estate</title>
               <style>
                    body { font-family: Tahoma; }
                    .header { display: flex; align-items: center; justify-content: center; padding: 20px; background-color: #f5f5f5; }
                    .logo { width: 100px; height: 100px; }
                    .company-info { display: flex; flex-direction: column; margin-left: 10px; text-align: center; }
                    .company-name { font-size: 24px; font-weight: bold; margin-bottom: 10px; }
                    .address { font-size: 16px; margin-bottom: 10px; }
                    .telephone { font-size: 16px; }
                    .containerheader { text-align: center; }
                    table { width: 100%; border-collapse: collapse; }
                    td, th { padding: 8px; text-align: left; }
                    th { background-color: #f2f2f2; }
                    .auto-style5, .auto-style6, .auto-style4 { width: 150px; }
                    .border { border: 1px solid #ccc; }  
                </style>
            </head>
            <body>
                <div class='header' style='text-align:left;'>

                    <div class='company-info'>
                        <div class='company-name'>Paramount Real Estate</div>
                        <div class='address'>1303, Grosvenor Business Tower - Barsha Heights - Dubai</div>
                        <div class='telephone'>04 591 3888</div>
                        <div>Employee Performance Weekly</div>
                    </div>
                </div>

                 <div class='report-container'>
                        <table>
                                   <tr>
                                       <th class='border' colspan=""9"" style=""text-align: center;"">Staff Data</th>
                                   </tr>
                            <tr>
                                <th class='border'>Firstname</th>
                                <th class='border'>Lastname</th>
                                <th class='border'>Converted Lead Ratio</th>
                                <th class='border'>Converted Lead Count</th>
                                <th class='border'>Calls Made</th>
                            </tr>";


                foreach (var items in staffPerformance)
                {
                    html += $@"
                                <tr>
                                      <td class='border'>{items.FirstName}</td>
                                      <td class='border'>{items.LastName}</td>
                                      <td class='border'>{items.ConversionRatio}</td>
                                      <td class='border'>{items.ConvertedLeadsToday}</td>
                                      <td class='border'>{items.CallsAssigned}</td>
                                </tr>
                                ";
                }

                html += $@"

                </table>
                </div>
                </body>
                </html>";

                _response.Result = html;

                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting employee performance! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }


        [HttpPost("all")]
        public async Task<ResponseDto> SeeAllEmployeePerformance(AuthDto authDto)
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
                Event = Event.GetAll.ToString(),
                Location = AccessLocation.EmployeePerformance.ToString()
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
                
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

                DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);
                
                var today = dubaiTime;
                var endOfToday = today.AddDays(1);
                var yesterday = today.AddDays(-1);
                
                var existingAllEmployees = await _db.Tblstaffs.Where(x => x.Designation == "Sales Person" && x.Status == 0).ToListAsync();
                var allLeads = await _db.Tblleads.Where(x => x.AddedOn >= yesterday && x.AddedOn < endOfToday).ToListAsync();
                var staffPerformance = new List<StaffPerformance>();

                foreach (var employee in existingAllEmployees)
                {
                    int callsAssignedToEmployee = await _db.TblCallInsights
                        .Where(call => call.AssignedTo == employee.Id.ToString() && call.AssignedOn >= today && call.CalledOn != null)
                        .CountAsync();

                    var userImage = await _db.Tblusers.FirstOrDefaultAsync(x => x.Userid == employee.Id);
                    
                    int convertedLeadsToday = allLeads
                        .Where(lead => lead.Assigned == employee.Id && lead.IsInterested == 1 && lead.AddedOn >= today)
                        .Count();

                    int convertedLeadsYesterday = allLeads
                        .Where(lead => lead.Assigned == employee.Id && lead.IsInterested == 1 && lead.AddedOn >= yesterday && lead.AddedOn < today)
                        .Count();
                    
                    int notCalledCount = callsAssignedToEmployee - (convertedLeadsToday + convertedLeadsYesterday);

                    var performance = new StaffPerformance
                    {
                        FirstName = employee.Firstname,
                        LastName = employee.Lastname,
                        CallsAssigned = callsAssignedToEmployee,
                        ConvertedLeadsToday = convertedLeadsToday,
                        ConvertedLeadsYesterday = convertedLeadsYesterday,
                        NotCalled = notCalledCount,
                        Image = userImage.Userimage,
                    };
                    
                    double conversionRatio = CalculateConversionRatio(convertedLeadsToday, convertedLeadsYesterday);
                    performance.ConversionRatio = conversionRatio;

                    staffPerformance.Add(performance);
                }

                _response.Result = staffPerformance;


                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting employee performance! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }
        
        [HttpPost("allWeekly")]
        public async Task<ResponseDto> SeeAllEmployeePerformanceWeekly(AuthDto authDto)
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
                Event = Event.GetAll.ToString(),
                Location = AccessLocation.EmployeePerformance.ToString()
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
                var today = DateTime.Today;
                var endOfToday = today.AddDays(1);
                var yesterday = today.AddDays(-1);
                var startOfLastWeek = today.AddDays(-7);
                var endOfLastWeek = today;
                
                int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
                var startOfThisWeek = today.AddDays(-daysUntilMonday);
                var endOfThisWeek = startOfThisWeek.AddDays(7);


                var existingAllEmployees = await _db.Tblstaffs.Where(x => x.Designation == "Sales Person" && x.Status == 0).ToListAsync();
                var allLeads = await _db.Tblleads.Where(x => x.AddedOn >= startOfLastWeek && x.AddedOn < endOfToday).ToListAsync();
                var staffPerformance = new List<StaffPerformance>();

                foreach (var employee in existingAllEmployees)
                {
                    int callsAssignedToEmployee = await _db.TblCallInsights
                        .Where(call => call.AssignedTo == employee.Id.ToString() && call.AssignedOn >= startOfThisWeek && call.AssignedOn <= endOfThisWeek && call.CalledOn != null)
                        .CountAsync();

                    var userImage = await _db.Tblusers.FirstOrDefaultAsync(x => x.Userid == employee.Id);

                    int convertedLeadsThisWeek = allLeads
                        .Where(lead => lead.Assigned == employee.Id && lead.IsInterested == 1 && lead.AddedOn >= startOfThisWeek)
                        .Count();

                    int convertedLeadsLastWeek = allLeads
                        .Where(lead => lead.Assigned == employee.Id && lead.IsInterested == 1 && lead.AddedOn >= startOfLastWeek && lead.AddedOn < startOfThisWeek)
                        .Count();
                    
                    // int notCalledCount = callsAssignedToEmployee - (convertedLeadsThisWeek + convertedLeadsLastWeek);

                    var performance = new StaffPerformance
                    {
                        FirstName = employee.Firstname,
                        LastName = employee.Lastname,
                        CallsAssigned = callsAssignedToEmployee,
                        ConvertedLeadsToday = convertedLeadsThisWeek,
                        ConvertedLeadsYesterday = convertedLeadsLastWeek,
                        // NotCalled = notCalledCount,
                        Image = userImage.Userimage,
                    };
                    
                    double conversionRatio = CalculateConversionRatio(convertedLeadsThisWeek, convertedLeadsLastWeek);
                    performance.ConversionRatio = conversionRatio;

                    staffPerformance.Add(performance);
                }

                _response.Result = staffPerformance;

                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting employee performance! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }
        
        [HttpPost("allMonthly")]
        public async Task<ResponseDto> SeeAllEmployeePerformanceMonthly(AuthDto authDto)
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
                Event = Event.GetAll.ToString(),
                Location = AccessLocation.EmployeePerformance.ToString()
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
                // Calculate date range for the last month and this month starting from the first day of this month
                var today = DateTime.Today; // Get the start of today
                var endOfToday = today.AddDays(1); // Get the end of today

                // Calculate the start of this month (first day of this month)
                var startOfThisMonth = new DateTime(today.Year, today.Month, 1);
                var endOfThisMonth = startOfThisMonth.AddMonths(1);

                // Calculate the start of last month
                var startOfLastMonth = startOfThisMonth.AddMonths(-1);
                var endOfLastMonth = startOfThisMonth;

                var existingAllEmployees = await _db.Tblstaffs.Where(x => x.Designation == "Sales Person" && x.Status == 0).ToListAsync();
                var allLeads = await _db.Tblleads.Where(x => x.AddedOn >= startOfLastMonth && x.AddedOn < endOfToday).ToListAsync();
                var staffPerformance = new List<StaffPerformance>();

                foreach (var employee in existingAllEmployees)
                {
                    int callsAssignedToEmployee = await _db.TblCallInsights
                        .Where(call => call.AssignedTo == employee.Id.ToString() && call.CalledOn != null && call.AssignedOn >= startOfLastMonth && call.AssignedOn <= endOfToday)
                        .CountAsync();

                    var userImage = await _db.Tblusers.FirstOrDefaultAsync(x => x.Userid == employee.Id);

                    int convertedLeadsThisMonth = allLeads
                        .Where(lead => lead.Assigned == employee.Id && lead.IsInterested == 1 && lead.AddedOn >= startOfThisMonth)
                        .Count();

                    int convertedLeadsLastMonth = allLeads
                        .Where(lead => lead.Assigned == employee.Id && lead.IsInterested == 1 && lead.AddedOn >= startOfLastMonth && lead.AddedOn < startOfThisMonth)
                        .Count();

                    int notCalledCount = callsAssignedToEmployee - (convertedLeadsThisMonth + convertedLeadsLastMonth);

                    var performance = new StaffPerformance
                    {
                        FirstName = employee.Firstname,
                        LastName = employee.Lastname,
                        CallsAssigned = callsAssignedToEmployee,
                        ConvertedLeadsToday = convertedLeadsThisMonth,
                        ConvertedLeadsYesterday = convertedLeadsLastMonth,
                        NotCalled = notCalledCount,
                        Image = userImage.Userimage,
                    };

                    double conversionRatio = CalculateConversionRatio(convertedLeadsThisMonth, convertedLeadsLastMonth);
                    performance.ConversionRatio = conversionRatio;

                    staffPerformance.Add(performance);
                }

                _response.Result = staffPerformance;

                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting employee performance! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

                
        private double CalculateConversionRatio(int convertedToday, int convertedYesterday)
        {
            if (convertedYesterday == 0)
            {
                if (convertedToday == 0)
                {
                    return 0.00;
                }
                else
                {
                    return 100.00;
                }
            }

            double ratio = ((double)convertedToday / convertedYesterday) * 100.00;

            return Math.Round(ratio, 2);
        }
        
        public class StaffInteractionReport
        {
            public string FullName { get; set; }
            public int Calls { get; set; }
            public int HotLeads { get; set; }
            public int PlannedMeetings { get; set; }
            public int MissedMonths { get; set; }
            public int NoFollowUps { get; set; }
            public int WeekFollowUp { get; set; }
            public int YesterdayHotLeads { get; set; }
            public int RemarkCount { get; set; }
        }

        public class ExcelFormat
        {
            public string LeadId { get; set; }
            public string ClientName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Source { get; set; }
            public DateTime AssignedDate { get; set; }
            public string LeadStatus { get; set; }
            public string CalledOnDate { get; set; }
            public string Activity { get; set; }
            public DateTime NextInteractionDate { get; set; }
            public string NextActivity { get; set; }
            public string Comment { get; set; }
        }

        public class StaffPerformance
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int CallsAssigned { get; set; }
            public int ConvertedLeadsToday { get; set; }
            public int ConvertedLeadsYesterday { get; set; }
            public int NotCalled { get; set; }
            public string Image { get; set; }
            public double ConversionRatio { get; set; }
        }

        public class DateFilterDto
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public AuthDto authDto { get; set; }
        }
    }
}
