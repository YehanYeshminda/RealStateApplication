using System.Data;
using API.Models;
using API.Repos;
using API.Repos.Dtos;
using API.Repos.Dtos.CallCenterDtos;
using API.Repos.Dtos.CallInsignt;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using API.Repos.Control;
using API.Repos.LeadStatus;
using API.Repos.Services;
using API.Repos.Notification;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallCenterController : ControllerBase
    {
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ResponseDto _response;

        public CallCenterController(CRMContext context, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _db = context;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _response = new ResponseDto();
        }

        [HttpGet("GetLeadLogByLeadNo")]
        public async Task<ResponseDto> GetAllLeadLogsByLeadNo([FromQuery]string leadNo)
        {
            try
            {
                var existingLeadLogs = await _unitOfWork.leadLogInterface.GetLeadLogByLeadId(leadNo);

                _response.Result = existingLeadLogs;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting all leads by id! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("OnReScheduleCall")]
        public async Task<ResponseDto> OnAssignedCall(UpdateScheduleForLeadDto updateScheduleForLeadDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(updateScheduleForLeadDto.AuthDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var existingLead = await _unitOfWork.leadsInterface.GetLeadsByIdAsync(updateScheduleForLeadDto.LeadNo);

                if (existingLead == null)
                {
                    _response.Message = $"Lead with this Id:{updateScheduleForLeadDto.LeadNo} does not exist";
                    _response.IsSuccess = false;
                    return _response;
                }

                var existingStaff = await _unitOfWork.staffInterface.GetStaffByIdAsync(updateScheduleForLeadDto.AssignedStaff);

                if (existingLead == null)
                {
                    _response.Message = $"staff with this Id:{updateScheduleForLeadDto.AssignedStaff} does not exist";
                    _response.IsSuccess = false;
                    return _response;
                }

                var newLog = new TblLeadlog
                {
                    Addby = authResponse.Result.Userid,
                    Addon = DateTime.Now,
                    Leadid = existingLead.Leadno,
                    Log = $"Lead has been scheduled from {existingLead.Assignon} to {updateScheduleForLeadDto.ScheuledTime} by the user {authResponse.Result.Userid} for the staff {existingStaff.Id} with a remark {updateScheduleForLeadDto.Description}"
                };

                await _db.TblLeadlogs.AddAsync(newLog);
                await _db.SaveChangesAsync();

                var newMeeting = new TblMeeting
                {
                    Addby = authResponse.Result.Userid,
                    Addon = DateTime.Now,
                    Conclusion = "",
                    Custid = 0,
                    Date = DateTime.Now,
                    Meetdate = updateScheduleForLeadDto.OriginalDate,
                    Meettime = updateScheduleForLeadDto.OriginalTime,
                    Name = "Meeting Rechedule",
                    Reason = "Lead Rescheuled and meeting is set",
                    Remarks = updateScheduleForLeadDto.Description,
                    Staffid = updateScheduleForLeadDto.AssignedStaff,
                    Status = 0,
                    Venue = ""
                };

                await _db.TblMeetings.AddAsync(newMeeting);
                await _db.SaveChangesAsync();

                var newMeetingNotification = new TblNotification
                {
                    Addby = authResponse.Result.Userid,
                    Addon = DateTime.Now,
                    Date = newMeeting.Date,
                    Forwardto = newMeeting.Staffid.ToString(),
                    Notify = newMeeting.Staffid,
                    From = updateScheduleForLeadDto.ScheuledTime,
                    Message = newMeeting.Reason,
                    Priorityid = 1,
                    Snoozeon = updateScheduleForLeadDto.ScheuledTime,
                    Time = updateScheduleForLeadDto.OriginalTime
                };

                await _db.TblNotifications.AddAsync(newMeetingNotification);
                await _db.SaveChangesAsync();

                existingLead.Staffid = updateScheduleForLeadDto.AssignedStaff;
                existingLead.Assigned = updateScheduleForLeadDto.AssignedStaff;
                existingLead.Assignon = updateScheduleForLeadDto.ScheuledTime;

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = newLog;
                _response.Message = "Meeting has been succesfully scheduled";
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting all leads by id! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("UpdateLeadLog")]
        public async Task<ResponseDto> UpdateLeadLog(UpdateLeadCallCenterDto updateLeadCallCenterDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(updateLeadCallCenterDto.AuthDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var existingLead = await _unitOfWork.leadsInterface.GetLeadsByIdAsync(updateLeadCallCenterDto.LeadNo);

                if (existingLead == null)
                {
                    _response.Message = $"Lead with this Id:{updateLeadCallCenterDto.LeadNo} does not exist";
                    _response.IsSuccess = false;
                    return _response;
                }
                
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

                DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);

                var newLog = new TblLeadlog
                {
                    Addby = authResponse.Result.Userid,
                    Addon = dubaiTime,
                    Leadid = existingLead.Leadno,
                    Log = $"Ended the call at {dubaiTime} with the lead {existingLead.Leadno} with the name {existingLead.Name} with a remark of {updateLeadCallCenterDto.Remark}"
                };


                await _db.TblLeadlogs.AddAsync(newLog);
                await _db.SaveChangesAsync();

                existingLead.Importance = updateLeadCallCenterDto.LeadStatus;

                await _db.SaveChangesAsync();

                _response.Message = "Lead Status updated!";
                _response.Result = "";
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting all leads by id! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetAllLeadLogByLead")]
        public async Task<ResponseDto> GetAllLeadsByLeadId([FromQuery]string leadId, [FromBody]AuthDto authDto)
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
                var existingLeads = await _unitOfWork.leadLogInterface.GetLeadLogByLeadId(leadId);

                _response.IsSuccess = true;
                _response.Result = existingLeads;
                _response.Message = "";
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting all leads by id! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }
        
        public class MakeCallInsigntRequest
        {
            public AuthDto authDto { get; set; }
            public int PageSize { get; set; }
            public int Page { get; set; }
        }

        [HttpPost("CallListInsignts")]
        public async Task<ResponseDto> GetCallListInsigntData([FromBody] MakeCallInsigntRequest makeCallInsigntRequest)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(makeCallInsigntRequest.authDto);

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
                Location = AccessLocation.Make_Call.ToString()
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
                var existingStaff = await _unitOfWork.staffInterface.GetStaffByIdAsync(authResponse.Result.Userid);

                if (existingStaff == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Staff with this id does not exist";
                    return _response;
                }
                
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

                DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);
                
                DateTime currentDate = dubaiTime;
                DateTime startDate = currentDate.Date;
                DateTime endDate = currentDate.Date.AddDays(1);
                
                var calls = await _unitOfWork.callListInterface.GetCallInsightsByDesignation(startDate.Date, endDate.Date, existingStaff.Designation, authResponse.Result.Userid.ToString());
                
                var totalCount = calls.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / makeCallInsigntRequest.PageSize);
                var leadsPerPage = calls.Skip((makeCallInsigntRequest.Page - 1) * makeCallInsigntRequest.PageSize).Take(makeCallInsigntRequest.PageSize).ToList();

                _response.IsSuccess = true;
                _response.Result = new
                {
                    data = leadsPerPage,
                    totalCountPages = totalPages,
                    totalData = totalCount
                };
                
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting all call insignts! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("ContactListAll")]
        public async Task<ResponseDto> GetContactListFull([FromBody] AuthDto authDto)
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
                Location = AccessLocation.Make_Call.ToString()
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
                var allCallListInsignts = await _unitOfWork.callListInterface.GetAllCallList();

                _response.IsSuccess = true;
                _response.Result = allCallListInsignts;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting all call insignts! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("ContactListAllNew")]
        public async Task<ResponseDto> GetContactListFull([FromBody] AuthDto authDto, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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
                Location = AccessLocation.Make_Call.ToString()
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
                var allCallListInsignts = await _unitOfWork.callListInterface.GetAllCallList();

                var totalCount = allCallListInsignts.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
                var leadsPerPage = allCallListInsignts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                _response.IsSuccess = true;
                _response.Result = new
                {
                    data = leadsPerPage,
                    totalCountPages = totalPages,
                    totalData = totalCount
                };
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting all call insignts! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("CallListInsigntsAllNew")]
        public async Task<ResponseDto> GetCallListInsigntDataFull([FromBody] AuthDto authDto, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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
                Location = AccessLocation.Call_List.ToString()
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
                var allCallListInsignts = await _unitOfWork.callListInterface.GetAllCallList();

                var totalCount = allCallListInsignts.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
                var leadsPerPage = allCallListInsignts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                _response.IsSuccess = true;
                _response.Result = new
                {
                    data = leadsPerPage,
                    totalCountPages = totalPages,
                    totalData = totalCount
                };
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting all call insignts! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }
        
        [HttpPost("AssignCallInsignts")]
        public async Task<ResponseDto> AssignCallInsights([FromBody] AssignCallInsightDto assignCallInsightDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(assignCallInsightDto.AuthDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

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
                Location = AccessLocation.Make_Call.ToString()
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

                var existingStaff = await _unitOfWork.staffInterface.GetStaffByIdAsync(assignCallInsightDto.AssignStaff);

                if (existingStaff == null)
                {
                    _response.Message = "Staff with this id does not exist";
                    _response.IsSuccess = false;
                    return _response;
                }

                _unitOfWork.callListInterface.AssignCallInsight(assignCallInsightDto.CallInsigntIds, existingStaff.Id.ToString());
                // foreach (var item in assignCallInsightDto.CallInsigntIds)
                // {
                //     var existingCallInsignt = await _unitOfWork.callListInterface.ExistingCallInsignt(item);

                //     if (existingCallInsignt == null)
                //     {
                //         _response.Message = "Call insignt with this id does not exist";
                //         _response.IsSuccess = false;
                //         return _response;
                //     }

                //     existingCallInsignt.Status = 1;
                //     existingCallInsignt.AssignedTo = existingStaff.Id.ToString();
                //     existingCallInsignt.AssignedOn = dubaiTime.AddHours(-5);
                // }

                // await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = "";
                _response.Message = "Successfully assigned calls";
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting all call insignts! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("AssignTopCallInsights")]
        public async Task<ResponseDto> AssignTopCallInsights([FromBody] AssignTopCallInsightDto assignTopCallInsightDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(assignTopCallInsightDto.AuthDto);

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
                Location = AccessLocation.Make_Call.ToString()
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
                
                var existingStaff = await _unitOfWork.staffInterface.GetStaffByIdAsync(assignTopCallInsightDto.AssignStaff);

                if (existingStaff == null)
                {
                    return new ResponseDto
                    {
                        Message = "Staff with this id does not exist",
                        IsSuccess = false
                    };
                }

                int numberOfItemsToAssign = assignTopCallInsightDto.NumberOfItemsToAssign;
                
                _unitOfWork.callListInterface.UpdateAndSetCalls(assignTopCallInsightDto.NumberOfItemsToAssign,
                        existingStaff.Id, dubaiTime);

                existingStaff.MonthlyTarget = numberOfItemsToAssign;
                existingStaff.CallsMonthlyTarget = numberOfItemsToAssign;

                await _db.SaveChangesAsync();

                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = $"Successfully assigned {numberOfItemsToAssign} calls"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    Message = "Error while assigning calls: " + ex.Message,
                    IsSuccess = false
                };
            }
        }

        public class LeadInsigntInformation
        {
            public string PhoneNumber { get; set; }
        }

        [HttpPost("GetLeadInsightInformation")]
        public async Task<ResponseDto> GetLeadInsightInformation([FromBody] LeadInsigntInformation leadInsigntInformation)
        {
            try
            {
                var existingLead = await _db.TblCallInsights.FirstOrDefaultAsync(x => x.PhoneNo == leadInsigntInformation.PhoneNumber);

                if (existingLead == null)
                {
                    _response.Message = "Call with this phone number does not exist";
                    _response.IsSuccess = false;
                    return _response;
                }

                _response.Message = "";
                _response.Result = existingLead;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Error. " + ex.Message;
                return _response;
            }
        }

        [HttpPost("AddNewConvertionLead")]
        public async Task<ResponseDto> ConvertToCall(ConvertToCallDto convertToCallDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(convertToCallDto.AuthDto);

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
                CallListService callListService = new CallListService(_db, _configuration);
                LeadStatusNewService leadStatusNewService = new LeadStatusNewService(_configuration);
                LeadsService leadsService = new LeadsService(_db, _configuration);
                ControlService controlService = new ControlService(_configuration);
                LeadLogService leadLogService = new LeadLogService(_db, _configuration);
                
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

                DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);

                var existingCallInsignt = await callListService.ExistingCallInsigntByPhone(convertToCallDto.PhoneNumber);

                if (existingCallInsignt == null)
                {
                    _response.Message = "Call with this number does not exist";
                    _response.IsSuccess = false;
                    return _response;
                }

                var lastLeadValue = GetLastValue("LE", "tblcontrol", "LeadNo");

                if (convertToCallDto.ClientIs == 0)
                {
                    convertToCallDto.ClientIs = 14;
                }

                if (convertToCallDto.NotLookingRadioStatus == "callback")
                {
                    convertToCallDto.ClientIs = 9;
                }
                
                if (convertToCallDto.NotLookingRadioStatus == "notinterested")
                {
                    convertToCallDto.ClientIs = 19;
                }
                
                if (convertToCallDto.NotLookingRadioStatus == "dbd")
                {
                    convertToCallDto.ClientIs = 18;
                }
                
                if (convertToCallDto.NotLookingRadioStatus == "voicemail")
                {
                    convertToCallDto.ClientIs = 17;
                }
                
                if (convertToCallDto.NotLookingRadioStatus == "invalidNo")
                {
                    convertToCallDto.ClientIs = 24;
                }

                var existingLeadStatus = leadStatusNewService.GetLeadStatusById(convertToCallDto.ClientIs ?? 0);

                if (existingLeadStatus == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Unable to find lead status wth this id";
                    return _response;
                }
                
                if (convertToCallDto.IsInterested == 0)
                {
                    if (convertToCallDto.ClientIs == 14)
                    {
                        convertToCallDto.ClientIs = 18;
                    }
                    
                    var newConvertedLead = new Tbllead
                    {
                        Leadno = lastLeadValue,
                        Sourceid = 6,
                        Campainid = "",
                        Name = existingCallInsignt.FirstName + " " + existingCallInsignt.LastName,
                        Phone = convertToCallDto.PhoneNumber,
                        Email = convertToCallDto.Email,
                        Otherno = " ",
                        Assigned = authResponse.Result.Userid,
                        Staffid = authResponse.Result.Userid,
                        Recievedon = dubaiTime,
                        Assignon = dubaiTime,
                        Importance = convertToCallDto.ClientIs ?? 0,
                        Called = 1,
                        Status = 0,
                        ContactMethod = 1,
                        CrossSegmentLead = convertToCallDto.Cross,
                        Attending = "",
                        RsvpTypeId = 0,
                        Comments = convertToCallDto.Comments,
                        InterestedIn = "",
                        PlanToDo = 0,
                        PlanToDoWhen = DateTime.Now.AddDays(-10),
                        IsLost = Convert.ToInt32(convertToCallDto.IsLost),
                        AddedOn = dubaiTime,
                        IsInterested = 0
                    };

                    var existingControl = controlService.GetControlTopOne();

                    // if (convertToCallDto.NotLookingRadioStatus == "callback")
                    // {
                    //     existingCallInsignt.Status = 0;
                    //     existingCallInsignt.AssignedTo = authResponse.Result.Userid.ToString();
                    // }
                    // else
                    // {
                    //     existingCallInsignt.Status = 1;
                    //     existingCallInsignt.AssignedTo = authResponse.Result.Userid.ToString();
                    // }

                    leadsService.AddNewLead(newConvertedLead);
                    callListService.UpdateCall(existingCallInsignt);

                    existingControl.LeadNo++;
                    controlService.UpdateControl(existingControl);
                    
                    var newLog = new TblLeadlog
                    {
                        Addby = authResponse.Result.Userid,
                        Addon = dubaiTime,
                        Leadid = lastLeadValue,
                        Log = "Created uninterested " + newConvertedLead.Leadno + " with the name "+ newConvertedLead.Name + " at " + dubaiTime + " with a status of " + existingLeadStatus.Leadstatus,
                    };

                    leadLogService.AddLog(newLog);

                    _response.Message = "Successfully Converted Lead.";
                    _response.IsSuccess = true;
                    return _response;
                }
                else
                {
                    var newConvertedLead = new Tbllead
                    {
                        Leadno = lastLeadValue,
                        Sourceid = 6,
                        Campainid = "",
                        Name = existingCallInsignt.FirstName + " " + existingCallInsignt.LastName,
                        Phone = convertToCallDto.PhoneNumber,
                        Email = convertToCallDto.Email,
                        Otherno = " ",
                        Assigned = authResponse.Result.Userid,
                        Staffid = authResponse.Result.Userid,
                        Recievedon = dubaiTime,
                        Assignon = dubaiTime,
                        Importance = Convert.ToInt32(convertToCallDto.ClientIs),
                        Called = 1,
                        Status = 0,
                        ContactMethod = 1,
                        CrossSegmentLead = convertToCallDto.Cross,
                        Attending = convertToCallDto.Attending,
                        RsvpTypeId = convertToCallDto.RsvpType,
                        Comments = convertToCallDto.Comments,
                        InterestedIn = convertToCallDto.Project,
                        PlanToDo = convertToCallDto.PlanToDo,
                        PlanToDoWhen = convertToCallDto.When,
                        IsLost = Convert.ToInt32(convertToCallDto.IsLost),
                        AddedOn = dubaiTime,
                        IsInterested = 1
                    };

                    var existingControl = controlService.GetControlTopOne();

                    // if (convertToCallDto.NotLookingRadioStatus == "callback")
                    // {
                    //     existingCallInsignt.Status = 0;
                    //     existingCallInsignt.AssignedTo = authResponse.Result.Userid.ToString();
                    // }
                    // else
                    // {
                    //     existingCallInsignt.Status = 1;
                    //     existingCallInsignt.AssignedTo = authResponse.Result.Userid.ToString();
                    // }

                    leadsService.AddNewLead(newConvertedLead);
                    callListService.UpdateCall(existingCallInsignt);
                    
                    existingControl.LeadNo++;
                    controlService.UpdateControl(existingControl);

                    var newLog = new TblLeadlog
                    {
                        Addby = authResponse.Result.Userid,
                        Addon = dubaiTime,
                        Leadid = lastLeadValue,
                        Log = "Created Lead " + newConvertedLead.Leadno + " with the name "+ newConvertedLead.Name + " at " + dubaiTime + " with a status of " + existingLeadStatus.Leadstatus,
                    };

                    leadLogService.AddLog(newLog);
                    
                    _response.Message = "Successfully Converted Lead.";
                    _response.IsSuccess = true;
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.Message = "Error while converting leads from calls! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [NonAction]
        public string GetLastValue(string prefix, string tableName, string columnName)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.OpenAsync();
                string query = $"SELECT TOP 1 {columnName} FROM {tableName} ORDER BY {columnName} DESC";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            string lastValue = "0";
                            while (reader.Read())
                            {
                                lastValue = reader[columnName].ToString();
                                break;
                            }

                            string numericPart = Regex.Match(lastValue, @"\d+").Value;

                            if (numericPart == "")
                            {
                                lastValue = "0";
                                numericPart = "0";
                            }

                            if (int.TryParse(numericPart, out int lastNumber))
                            {
                                lastNumber++;
                                string formattedValue = prefix + lastNumber.ToString("D2");
                                return formattedValue;
                            }

                            return "";
                        }
                        else
                        {
                            return prefix + "01";
                        }
                    }
                }
            }
        }


        [HttpPost("Notinterested")]
        public async Task<ResponseDto> Notinterested(NotinterestedDto notinteresteddto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(notinteresteddto.AuthDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

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
                Location = AccessLocation.Make_Call.ToString()
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
                if (notinteresteddto.events == "DND")
                {
                    var existing = await _db.TblCallInsights.FirstOrDefaultAsync(x => x.PhoneNo == notinteresteddto.Email);

                    if (existing == null)
                    {
                        _response.Message = "Data is null here";
                        _response.IsSuccess = false;
                        return _response;
                    }
                    existing.Status = 3;

                    await _db.SaveChangesAsync();
                    return _response;
                }

                else if (notinteresteddto.events == "notinterested")
                {
                    var existing = await _db.TblCallInsights.FirstOrDefaultAsync(x => x.PhoneNo == notinteresteddto.Email);
                    if (existing == null)
                    {
                        _response.Message = "Data is null here";
                        _response.IsSuccess = false;
                        return _response;
                    }
                    existing.Status = 4;

                    await _db.SaveChangesAsync();
                    return _response;
                }

                else if (notinteresteddto.events == "notanswering")
                {
                    var existing = await _db.TblCallInsights.FirstOrDefaultAsync(x => x.PhoneNo == notinteresteddto.Email);

                    if (existing == null)
                    {
                        _response.Message = "Data is null here";
                        _response.IsSuccess = false;
                        return _response;
                    }
                    existing.Status = 5;

                    await _db.SaveChangesAsync();
                    return _response;
                }

                return _response;   
            }
            catch (Exception ex)
            {
                _response.Message = "Error";
                _response.IsSuccess = false;
                return _response;
            }
        }

        public class UpdateCallTimeDto
        {
            public AuthDto authDto { get; set; }
            public string Email { get; set; }
        }

        [HttpPost("UpdateCallTime")]
        public async Task<ResponseDto> UpdateCallInsightStatusOfCallTiming(UpdateCallTimeDto updateCallTimeDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(updateCallTimeDto.authDto);

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
                TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time"); // Dubai's time zone ID

                DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);
                
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("UpdateCalledOnForPhoneNo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@NewCalledOn", SqlDbType.DateTime)
                        {
                            Value = dubaiTime
                        });

                        command.Parameters.Add(new SqlParameter("@PhoneNo", SqlDbType.NVarChar, 50)
                        {
                            Value = updateCallTimeDto.Email
                        });

                        command.ExecuteNonQuery();
                    }
                    
                    connection.Close();
                }
                
                _response.IsSuccess = true;
                _response.Message = "Updated call time successully";
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating calls insignt call time! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("UpdateCallEndTime")]
        public async Task<ResponseDto> UpdateCallInsightStatusOfCallEndTiming(UpdateCallTimeDto updateCallTimeDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(updateCallTimeDto.authDto);

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
                TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time"); // Dubai's time zone ID

                DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);
                
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("UpdateCalleEndedForPhoneNo", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@NewCalledOn", SqlDbType.DateTime)
                        {
                            Value = dubaiTime
                        });

                        command.Parameters.Add(new SqlParameter("@PhoneNo", SqlDbType.NVarChar, 50)
                        {
                            Value = updateCallTimeDto.Email
                        });

                        command.ExecuteNonQuery();
                    }
                    
                    connection.Close();
                }
                
                _response.IsSuccess = true;
                _response.Message = "Updated call time successully";
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating calls insignt call time! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPut("UpdateCall")]
        public async Task<ResponseDto> UpdateCall()
        {
            return _response;
        }



        //[HttpPost("GettingCallandleadscount")]
        //public async Task<ResponseDto> GettingCallandleadscounts(AuthDto authDto, DateTime startDate, DateTime endDate)
        //{
        //    var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

        //    if (!authResponse.IsSuccess)
        //    {
        //        _response.Message = authResponse.Message;
        //        _response.IsSuccess = authResponse.IsSuccess;
        //        _response.Result = authResponse.Result;
        //        return _response;
        //    }

        //    try
        //    {
        //        CallandLead staffperformance = new CallandLead(_configuration);

        //        var allstaffperformance = staffperformance.GetCallsAndLeadsCounts(startDate, endDate);

        //        _response.Result = allstaffperformance;
        //        _response.IsSuccess = true;
        //        return _response;
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = ex.Message;
        //        return _response;
        //    }
        //}

        [HttpPost("GettingCallandleadscount")]
        public async Task<ResponseDto> GettingCallandleadscounts(AuthDto authDto, DateTime startDate, DateTime endDate, int staffId)
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
                CallandLead staffperformance = new CallandLead(_configuration);

                if (staffId == 0)
                {
                    var allstaffperformance = staffperformance.GetCallsAndLeadsCounts(startDate, endDate);
                    _response.Result = allstaffperformance;
                }
                else
                {
                    var singlestaffperformance = staffperformance.GetSingleCallsAndLeadsCounts(startDate, endDate, staffId);
                    _response.Result = singlestaffperformance;
                }

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


    }

}
