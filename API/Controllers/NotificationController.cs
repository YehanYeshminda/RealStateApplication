using API.Models;
using API.Repos.Dtos;
using API.Repos.Dtos.MeetSchedDtos;
using API.Repos.Dtos.NotificationDtos;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using API.Repos.Notification;
using API.Repos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/Notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<MyHub> _hub;
        private readonly IConfiguration _configuration;
        private readonly GlobalDataService _globalDataService;

        public NotificationController(CRMContext db, IUnitOfWork unitOfWork, GlobalDataService globalDataService, IHubContext<MyHub> hub, IConfiguration configuration)
        {
            _response = new ResponseDto();
            _db = db;
            _hub = hub;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _globalDataService = globalDataService;
        }

        [HttpPost("GetAllNotifications")]
        public async Task<ResponseDto> GetAllNotis(GetAllNotificationAll getAllNotificationAll)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(getAllNotificationAll.authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var isAdmin = false;
                Notification notification = new Notification(_configuration);

                var currentStaff = await _unitOfWork.staffInterface.GetStaffByIdAsync(authResponse.Result.Userid);

                if (currentStaff == null)
                {
                    _response.Message = "Unable to find staff with this Id";
                    return _response;
                }
                
                if (currentStaff.Designation == "CEO")
                {
                    isAdmin = true;
                }
                
                var notifications = notification.GetAllNotificationsForUserForAll(isAdmin, currentStaff.Id);
                
                var totalCount = notifications.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / getAllNotificationAll.PageSize);
                var leadsPerPage = notifications.Skip((getAllNotificationAll.Page - 1) * getAllNotificationAll.PageSize).Take(getAllNotificationAll.PageSize).ToList();

                _response.IsSuccess = true;
                _response.Result = new
                {
                    data = leadsPerPage,
                    totalCountPages = totalPages,
                    totalData = totalCount
                };

                _response.Message = "";
                // _response.Result = notifications;
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        [HttpPost("AllNoti")]
        public async Task<ResponseDto> GetAllNotificationsForUser(AuthDto authDto)
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
                Notification notification = new Notification(_configuration);
                
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");
                
                DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);
                
                DateTime currentDate = dubaiTime;
                DateTime startDate = currentDate.Date;
                DateTime endDate = currentDate.Date.AddDays(1);
                
                var allNotificationsForUser = notification.GetAllNotificationsForUser(authResponse.Result.Userid, startDate, endDate);

                _response.Result = allNotificationsForUser;
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        [HttpPost]
        public async Task<ResponseDto> GetNotificationCount([FromBody] AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            var notifications =
                await _unitOfWork.notificationInterface.GetAllNotificationForUser(authResponse.Result.Userid
                    .ToString());

            _response.IsSuccess = true;
            _response.Message = "";
            _response.Result = notifications.Count();

            return _response;
        }

        [HttpPost("GetNotificationForUser")]
        public async Task<ResponseDto> GetAllNotificationsUser(AuthDto authDto)
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
                var getAllNotificationByStaff = await _unitOfWork.notificationInterface.GetAllNotificationForUser(authResponse.Result.Userid.ToString());

                _response.Result = getAllNotificationByStaff;
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while Getting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("AddNewNotification")]
        public async Task<ResponseDto> AddNewNotification([FromBody] AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            var getAllNotificationByStaff = await _unitOfWork.notificationInterface.GetAllNotificationForUser(authResponse.Result.Userid.ToString());
            await _hub.Clients.All.SendAsync("RefreshNotifications", getAllNotificationByStaff.Count());
            //await _hub.Clients.All.SendAsync("RefreshNotificationsValues", getAllNotificationByStaff);
            await _hub.Clients.All.SendAsync("RefreshNotificationsValues", getAllNotificationByStaff);
            return _response;
        }


        [HttpGet("report")]
        public async Task<ActionResult<TblNotification>> ReturnHtmlReport()
        {
            var tableForReport = await _db.TblNotifications.ToListAsync();

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
                        <div class='address'>Near North Villa</div>
                        <div class='telephone'>1234567890</div>
                    </div>
                </div>

                 <div class='report-container'>
                        <table>
                            <tr>
                                <th class='border' colspan=""10"" style=""text-align: center;"">Notification Data</th>
                            </tr>
                            <tr>
                                <th class='border'>Id</th>
                                <th class='border'>Notify</th>
                                <th class='border'>Date</th>
                                <th class='border'>Time</th>
                                <th class='border'>Message</th>
                                <th class='border'>Priority</th>
                                <th class='border'>Forwardto</th>
                                <th class='border'>Addby</th>
                                <th class='border'>Addon</th>
                                <th class='border'>From</th>
                            </tr>";


            foreach (var items in tableForReport)
            {

                string addon = Convert.ToDateTime(items.Addon).ToString("yyyy-MM-dd");
                string date = Convert.ToDateTime(items.Date).ToString("yyyy-MM-dd");
                string from = Convert.ToDateTime(items.From).ToString("yyyy-MM-dd");

                html += $@"
                                <tr>
                                      <td class='border'>{items.Id}</td>
                                      <td class='border'>{items.Notify}</td>
                                      <td class='border'>{date}</td>
                                      <td class='border'>{items.Time}</td>
                                      <td class='border'>{items.Message}</td>
                                      <td class='border'>{items.Priorityid}</td>
                                      <td class='border'>{items.Forwardto}</td>
                                      <td class='border'>{items.Addby}</td>
                                      <td class='border'>{addon}</td>
                                      <td class='border'>{from}</td>
                                </tr>
                                ";
            }

            html += $@"

                </table>
                </div>
                </body>
                </html>";

            var response = new Dtos.HtmlResponseDto { Content = html };

            return Ok(response);
        }



        [HttpGet("cellreport/{id}")]
        public async Task<ActionResult<TblNotification>> ReturnHtmlCellReport(int id)
        {
            var tableForReport = await _db.TblNotifications
                .Where(x => x.Id == id)
                    .ToListAsync();

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
                            table { border-collapse: collapse; margin-left: 90px; } 
                            td, th { padding: 8px; text-align: left; border: 1px solid #ccc;}
                            th { background-color: #f2f2f2; }
      	                    td {width : 300px;}
                            .auto-style5, .auto-style6, .auto-style4 { width: 150px; }
                        </style>
                    </head>
                    <body>
                        <div class='header' style='text-align:left;'>
                            <div class='company-info'>
                                <div class='company-name'>Paramount Real Estate</div>
                                <div class='address'>Near North Villa</div>
                                <div class='telephone'>1234567890</div>
                            </div>
                        </div>
                        <div class='report-container'>
                            <table>";

            foreach (var items in tableForReport)
            {
                string addon = Convert.ToDateTime(items.Addon).ToString("yyyy-MM-dd");
                string date = Convert.ToDateTime(items.Date).ToString("yyyy-MM-dd");
                string from = Convert.ToDateTime(items.From).ToString("yyyy-MM-dd");

                html += $@"
                            <tr>
                                <th colspan=""2"" style=""text-align: center;"">Notification Data</th>
                            </tr>
                                   <tr>
                                        <td>Id</td>
                                        <td>{items.Id}</td>
                                   </tr>
                                   <tr>
                                        <td>Notify</td>
                                        <td>{items.Notify}</td>
                                   </tr>
                                   <tr>
                                        <td>Date</td>
                                        <td>{date}</td>
                                   </tr>
                                   <tr>
                                        <td>Time</td>
                                        <td>{items.Time}</td>
                                   </tr>
                                   <tr>
                                        <td>Message</td>
                                        <td>{items.Message}</td>
                                   </tr>
                                   <tr>
                                        <td>Priority</td>
                                        <td>{items.Priorityid}</td>
                                   </tr>
                                   <tr>
                                        <td>Forward To</td>
                                        <td>{items.Forwardto}</td>
                                   </tr>
                                   <tr>
                                        <td>From</td>
                                        <td>{from}</td>
                                   </tr>
                                   <tr>
                                        <td>Add By</td>
                                        <td>{items.Addby}</td>
                                   </tr>
                                   <tr>
                                        <td>Add On</td>
                                        <td>{addon}</td>
                                   </tr>
                                ";
            }

            html += @"
                        </table>
                     </div>
                  </body>
                </html>";

            var response = new Dtos.HtmlResponseDto { Content = html };

            return Ok(response);
        }

        [HttpPost("getnotification")]
        public async Task<ResponseDto> Getnotification(AuthDto authDto)
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
                var notifications = await _unitOfWork.notificationInterface.GetAllNotificationForUserView(authResponse.Result.Userid.ToString());
                _response.IsSuccess = true;
                _response.Message = "";
                _response.Result = notifications;
                return _response;
            }

            catch (Exception ex)
            {
                _response.Message = "Error while Getting notifications! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("UpdateNotificationCount")]
        public async Task<ResponseDto> UpdateNotificationCount(AuthDto authDto)
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
                var notificationCount = await  _unitOfWork.notificationInterface.UpdateNotiticationCount(authResponse.Result.Userid.ToString());
                _response.Result = notificationCount;
                _response.IsSuccess = true;
                return _response;
            }

            catch (Exception ex)
            {
                _response.Message = "Error while Getting notification count! " + ex.Message;
                return _response;
            }
        }

        [HttpPost("insertnotification")]
        public async Task<ResponseDto> Insert(NotificationsDto notificationDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(notificationDto.AuthDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                if (notificationDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Missing User Data";
                    return _response;
                }

                var existingnotification = await _db.TblNotifications.FirstOrDefaultAsync(x => x.Id == notificationDto.Id);

                if (existingnotification != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid Id";
                    return _response;
                }

                var newUser = new TblNotification
                {
                    Notify = notificationDto.Notify,
                    Date = notificationDto.Date,
                    Time = notificationDto.Time,
                    Message = notificationDto.Message,
                    Priorityid = notificationDto.Priorityid,
                    Forwardto = notificationDto.Forwardto,
                    Snoozeon = DateTime.UtcNow,
                    From = DateTime.UtcNow.Date,
                    Addby = authResponse.Result.Userid,
                    Addon = DateTime.UtcNow.Date,
                };

                await _db.TblNotifications.AddAsync(newUser);
                await _db.SaveChangesAsync();


                var getAllNotificationByStaff = await _unitOfWork.notificationInterface.GetAllNotificationForUser(notificationDto.Forwardto);
                _response.IsSuccess = true;
                _response.Message = "Successfully inserting " + newUser.Id;
                _response.Result = newUser;
                return _response;

            }
            catch (Exception ex)
            {
                _response.Message = "Error while inserting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("InsertProcedureNotification")]
        public async Task<ResponseDto> InsertNotificationProcedure(NewNotificationDto notificationDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(notificationDto.AuthDto);

            if (!authResponse.IsSuccess)
            {
                return new ResponseDto
                {
                    Message = authResponse.Message,
                    IsSuccess = authResponse.IsSuccess,
                    Result = authResponse.Result
                };
            }

            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");
                DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);

                int notify = notificationDto.Notify;
                DateTime date = notificationDto.Date;
                string time = notificationDto.Time;
                string message = notificationDto.Message;
                int priorityid = notificationDto.Priorityid;
                int addby = authResponse.Result.Userid;
                DateTime addon = dubaiTime;
                string forwardto = notificationDto.Notify.ToString();
                DateTime snoozeon = notificationDto.Snoozeon;
                DateTime from = notificationDto.Date;

                var notificationRepository = new Notification(_configuration);
                notificationRepository.InsertNotification(notify, date, time, message, priorityid, addby, addon, forwardto, snoozeon, from);

                return new ResponseDto
                {
                    Result = "Notification inserted successfully",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        [HttpPost("UpdateProcedureNotification")]
        public async Task<ResponseDto> UpdateNotificationProcedure(NewNotificationDto notificationDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(notificationDto.AuthDto);

            if (!authResponse.IsSuccess)
            {
                return new ResponseDto
                {
                    Message = authResponse.Message,
                    IsSuccess = authResponse.IsSuccess,
                    Result = authResponse.Result
                };
            }

            try
            {
                int id = notificationDto.Id; 
                int notify = notificationDto.Notify;
                DateTime date = notificationDto.Date;
                string time = notificationDto.Time;
                string message = notificationDto.Message;
                int priorityid = notificationDto.Priorityid;
                string forwardto = notificationDto.Notify.ToString();
                DateTime snoozeon = notificationDto.Snoozeon;
                DateTime from = notificationDto.Date;

                var notificationRepository = new Notification(_configuration);
                notificationRepository.UpdateNotification(id, notify, date, time, message, priorityid, forwardto, snoozeon, from);

                return new ResponseDto
                {
                    Result = "Notification updated successfully",
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        [HttpPost("DeleteNotification")]
        public async Task<ResponseDto> DeleteNotification([FromBody] AuthDto authDto, [FromQuery] int id)
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

                var existingNotification = await _unitOfWork.notificationInterface.GetNotificationByIdAsync(id);

                if (existingNotification == null)
                {
                    _response.Message = "Unable to find notification with this id";
                    _response.IsSuccess = false;
                    return _response;
                }

                existingNotification.Status = 1;

                _db.TblNotifications.Update(existingNotification);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Successfully deleted notification";
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while inserting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }


        [HttpPost("updatenotification")]
        public async Task<ResponseDto> Updatenotification(NotificationsDto notificationDto)
        {
            if (notificationDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(notificationDto.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == notificationDto.AuthDto.Hash);

            if (_user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid Hash";
                return _response;
            }

            var decryptedDateWithOffset = decodedValues.Date.AddDays(1);
            var currentDate = DateTime.UtcNow.Date;

            if (currentDate < decryptedDateWithOffset.Date)
            {
                try
                {
                    if (notificationDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingnotification = await _db.TblNotifications.FirstOrDefaultAsync(x => x.Id == notificationDto.Id);

                    if (existingnotification == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Id not found";
                        return _response;
                    }

                    existingnotification.Notify = notificationDto.Notify;
                    existingnotification.Date = notificationDto.Date;
                    existingnotification.Time = notificationDto.Time;
                    existingnotification.Message = notificationDto.Message;
                    existingnotification.Priorityid = notificationDto.Priorityid;
                    existingnotification.Forwardto = notificationDto.Forwardto;

                    _db.TblNotifications.Update(existingnotification);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully updated notification: " + existingnotification.Id;
                    _response.Result = existingnotification;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while updating notification! " + ex.Message;
                    _response.IsSuccess = false;
                    return _response;
                }
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid Hash";
                return _response;
            }
        }

    }
}
