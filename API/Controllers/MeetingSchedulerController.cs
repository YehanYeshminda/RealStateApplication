using API.Models;
using API.Repos.Dtos;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using API.Repos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Repos.Dtos.MeetSchedDtos;
using API.Repos.Dtos.CommonDto;
using API.Repos.Dtos.AdvPaymentDtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos;

namespace API.Controllers
{
    [Route("api/meetingscheduler")]
    [ApiController]
    public class MeetingSchedulerController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly GlobalDataService _globalDataService;

        public MeetingSchedulerController(CRMContext db, IUnitOfWork unitOfWork, GlobalDataService globalDataService)
        {
            _response = new ResponseDto();
            _db = db;
            _unitOfWork = unitOfWork;
            _globalDataService = globalDataService;
        }

        [HttpGet("report")]
        public async Task<ActionResult<TblVmeeting>> ReturnHtmlReport()
        {
            var tableForReport = await _db.TblVmeetings.ToListAsync();

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
                                <th class='border' colspan=""14"" style=""text-align: center;"">Meeting Schedule Data</th>
                            </tr>
                            <tr>
                                <th class='border'>Id</th>
                                <th class='border'>Name</th>
                                <th class='border'>Date</th>
                                <th class='border'>Staff</th>
                                <th class='border'>Reason</th>
                                <th class='border'>Customer</th>
                                <th class='border'>Meeting Date</th>
                                <th class='border'>Meeting Time</th>
                                <th class='border'>Venue</th>
                                <th class='border'>Remarks</th>
                                <th class='border'>Status</th>
                                <th class='border'>Conclusion</th>
                                <th class='border'>Addby</th>
                                <th class='border'>Addon</th>
                            </tr>";


            foreach (var items in tableForReport)
            {

                string addon = Convert.ToDateTime(items.Addon).ToString("yyyy-MM-dd");
                string meetdate = Convert.ToDateTime(items.Meetdate).ToString("yyyy-MM-dd");
                string date = Convert.ToDateTime(items.Date).ToString("yyyy-MM-dd");

                html += $@"
                                <tr>
                                      <td class='border'>{items.Id}</td>
                                      <td class='border'>{items.Name}</td>
                                      <td class='border'>{date}</td>
                                      <td class='border'>{items.Expr1}</td>
                                      <td class='border'>{items.Reason}</td>
                                      <td class='border'>{items.CustName}</td>
                                      <td class='border'>{meetdate}</td>
                                      <td class='border'>{items.Meettime}</td>
                                      <td class='border'>{items.Venue}</td>
                                      <td class='border'>{items.Remarks}</td>
                                      <td class='border'>{items.Status}</td>
                                      <td class='border'>{items.Conclusion}</td>
                                      <td class='border'>{items.Addby}</td>
                                      <td class='border'>{addon}</td>
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
        public async Task<ActionResult<TblVmeeting>> ReturnHtmlCellReport(int id)
        {
            var tableForReport = await _db.TblVmeetings
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
                string meetdate = Convert.ToDateTime(items.Meetdate).ToString("yyyy-MM-dd");
                string date = Convert.ToDateTime(items.Date).ToString("yyyy-MM-dd");

                html += $@"
                            <tr>
                                <th colspan=""2"" style=""text-align: center;"">Meeting Schedule Data</th>
                            </tr>
                                   <tr>
                                        <td>Id</td>
                                        <td>{items.Id}</td>
                                   </tr>
                                   <tr>
                                        <td>Name</td>
                                        <td>{items.Name}</td>
                                   </tr>
                                   <tr>
                                        <td>Date</td>
                                        <td>{date}</td>
                                   </tr>
                                   <tr>
                                        <td>Staff</td>
                                        <td>{items.Expr1}</td>
                                   </tr>
                                   <tr>
                                        <td>Reason</td>
                                        <td>{items.Reason}</td>
                                   </tr>
                                   <tr>
                                        <td>Customer</td>
                                        <td>{items.CustName}</td>
                                   </tr>
                                   <tr>
                                        <td>Meeting Date</td>
                                        <td>{meetdate}</td>
                                   </tr>
                                   <tr>
                                        <td>Meeting Time</td>
                                        <td>{items.Meettime}</td>
                                   </tr>
                                   <tr>
                                        <td>Venue</td>
                                        <td>{items.Venue}</td>
                                   </tr>
                                   <tr>
                                        <td>Remarks</td>
                                        <td>{items.Remarks}</td>
                                   </tr>
                                   <tr>
                                        <td>Status</td>
                                        <td>{items.Status}</td>
                                   </tr>
                                   <tr>
                                        <td>Conclusion</td>
                                        <td>{items.Conclusion}</td>
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


        [HttpPost("GetMeetingInfoForCalender")]
        public async Task<ResponseDto> GetMeetingInforForCalender([FromBody] AuthDto authDto)
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
                var arrayToAdd = new List<MeetingForCalenderDto>();
                var existingMeetings = await _unitOfWork.iMeetingInterface.GetAllMeetingByUserId(authResponse.Result.Userid);

                foreach (var item in existingMeetings)
                {
                    var dateWithTime = item.Meetdate.Date + TimeSpan.Parse(item.Meettime);

                    var newItem = new MeetingForCalenderDto
                    {
                        Id = item.Id.ToString(),
                        StartDate = dateWithTime.ToString("M/d/yyyy HH:mm:ss"),
                        Title = item.Name
                    };


                    arrayToAdd.Add(newItem);
                }

                _response.Result = arrayToAdd;
                _response.Message = "";
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while inserting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }


        }

        [HttpPost("insertMeeting")]
        public async Task<ResponseDto> Insert(MeetSchedDto meetingDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(meetingDto.AuthDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            var newUserPermission = new SendGetUserPermission
            {
                Event = Event.Add.ToString(),
                Location = AccessLocation.MeetSchedule.ToString()
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
                if (meetingDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Missing User Data";
                    return _response;
                }

                var existingMeeting = await _db.TblMeetings.FirstOrDefaultAsync(x => x.Id == meetingDto.Id);

                if (existingMeeting != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Meeing with this id already exists";
                    return _response;
                }

                var newMeeting = new TblMeeting
                {
                    Date = meetingDto.Date,
                    Name = meetingDto.Name,
                    Staffid = meetingDto.Staffid,
                    Reason = meetingDto.Reason,
                    Custid = meetingDto.Custid,
                    Meetdate = meetingDto.Meetdate,
                    Meettime = meetingDto.Meettime,
                    Venue = meetingDto.Venue,
                    Remarks = meetingDto.Remarks,
                    Addby = authResponse.Result.Userid,
                    Addon = DateTime.UtcNow.Date,
                    Status = meetingDto.Status,
                    Conclusion = meetingDto.Conclusion,
                };
                await _db.TblMeetings.AddAsync(newMeeting);
                await _db.SaveChangesAsync();

                foreach (var item in meetingDto.staffIds)
                {
                    var existingStaff = await _unitOfWork.staffInterface.GetStaffByIdAsync(item);

                    if (existingStaff == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Staff with " + item + " does not exist";
                        return _response;
                    }

                    var newSuppoerStaffForMeeting = new TblSupportStaff
                    {
                        Addby = authResponse.Result.Userid,
                        Addon = DateTime.UtcNow,
                        Meetid = newMeeting.Id,
                        Staffid = item,
                    };

                    await _db.TblSupportStaffs.AddAsync(newSuppoerStaffForMeeting);
                }

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Successfully schedule meeting with the Id " + newMeeting.Id;
                _response.Result = newMeeting;
                return _response;

            }
            catch (Exception ex)
            {
                _response.Message = "Error while inserting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }


        [HttpPost("updateMeeting")]
        public async Task<ResponseDto> UpdateMeeting(MeetSchedDto meetingDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(meetingDto.AuthDto);

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
                Location = AccessLocation.MeetSchedule.ToString()
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
                    if (meetingDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingMeeting = await _db.TblMeetings.FirstOrDefaultAsync(x => x.Id == meetingDto.Id);

                    if (existingMeeting == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Id not found";
                        return _response;
                    }

                    existingMeeting.Date = meetingDto.Date;
                    existingMeeting.Name = meetingDto.Name;
                    existingMeeting.Staffid = meetingDto.Staffid;
                    existingMeeting.Reason = meetingDto.Reason;
                    existingMeeting.Custid = meetingDto.Custid;
                    existingMeeting.Meetdate = meetingDto.Meetdate;
                    existingMeeting.Meettime = meetingDto.Meettime;
                    existingMeeting.Venue = meetingDto.Venue;
                    existingMeeting.Remarks = meetingDto.Remarks;
                    existingMeeting.Status = meetingDto.Status;
                    existingMeeting.Conclusion = meetingDto.Conclusion;


                    var existingSupportStaff = await _db.TblSupportStaffs
                        .Where(x => x.Meetid == existingMeeting.Id)
                        .ToListAsync();

                    _db.TblSupportStaffs.RemoveRange(existingSupportStaff);

                    foreach (var item in meetingDto.staffIds)
                    {
                        var existingStaff = await _unitOfWork.staffInterface.GetStaffByIdAsync(item);

                        if (existingStaff == null)
                        {
                            _response.IsSuccess = false;
                            _response.Message = "Staff with " + item + " does not exist";
                            return _response;
                        }

                        var newSuppoerStaffForMeeting = new TblSupportStaff
                        {
                            Addby = authResponse.Result.Userid,
                            Addon = DateTime.UtcNow,
                            Meetid = existingMeeting.Id, 
                            Staffid = item,
                        };

                        await _db.TblSupportStaffs.AddAsync(newSuppoerStaffForMeeting);
                    }


                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully updated Meeting: " + existingMeeting.Id;
                    _response.Result = existingMeeting;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while updating Meeting! " + ex.Message;
                    _response.IsSuccess = false;
                    return _response;
                }
        }

        [HttpPost("getMeeting")]
        public async Task<ResponseDto> GetAllMeeting(AuthDto authDto)
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
                Location = AccessLocation.MeetSchedule.ToString()
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
                var Meeting = await _unitOfWork.iMeetingInterface.GetAllVMeeting();
                _response.IsSuccess = true;
                _response.Message = "";
                _response.Result = Meeting;
                return _response;

            }
            catch (Exception ex)
            {
                _response.Message = "Error while Getting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }


        [HttpPost("GetStaffNameId")]
        public async Task<ActionResult<List<CommonDto>>> GetStaffNameId(AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                return BadRequest(authResponse.Message);
            }

            try
            {
                var newList = new List<CommonDto>();
                var existingBanks = await _db.Tblstaffs.Where(x => x.Status == 0).ToListAsync();

                foreach (var item in existingBanks)
                {
                    var newItemToAdd = new CommonDto
                    {
                        textValue = item.Name,
                        value = item.Id
                    };

                    newList.Add(newItemToAdd);
                }

                return Ok(newList);
            }
            catch (Exception ex)
            {
                return BadRequest("Error while getting data! " + ex.Message);
            }
        }

        [HttpPost("GetStaffNameIdAll")]
        public async Task<ActionResult<List<CommonDto>>> GetStaffNameIdAll(AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                return BadRequest(authResponse.Message);
            }

            try
            {
                var newList = new List<CommonDto>();
                var existingBanks = await _db.Tblstaffs.ToListAsync();

                foreach (var item in existingBanks)
                {
                    var newItemToAdd = new CommonDto
                    {
                        textValue = item.Name,
                        value = item.Id
                    };
                    newList.Add(newItemToAdd);
                }

                return Ok(newList);
            }

            catch (Exception ex)
            {
                return BadRequest("Error! " + ex.Message);
            }
        }

    }
}
