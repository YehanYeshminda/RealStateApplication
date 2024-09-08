using System.Drawing.Printing;
using API.Models;
using API.Repos;
using API.Repos.Dtos;
using API.Repos.Dtos.LeadForwardDto;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace API.Controllers
{
    [Route("api/leadforward")]
    [ApiController]
    public class LeadForwardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CRMContext _db;
        private readonly ResponseDto _response;

        public LeadForwardController(IUnitOfWork unitOfWork, CRMContext context)
        {
            _unitOfWork = unitOfWork;
            _db = context;
            _response = new ResponseDto();
        }

        [HttpPost("GetAllLeadForwards")]
        public async Task<ResponseDto> GetAllLeadForwards(AuthDto authDto)
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
                Location = AccessLocation.LeadForward.ToString()
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
                var leads = await _unitOfWork.leadForwardInterface.GetLeadForwardListView();
                _response.IsSuccess = true;
                _response.Message = "";
                _response.Result = leads;
                return _response;

            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetAllLeadForwardsNew")]
        public async Task<ResponseDto> GetAllLeadForwardsNew([FromBody]AuthDto authDto, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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
                Location = AccessLocation.LeadForward.ToString()
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
                var leads = await _unitOfWork.leadForwardInterface.GetLeadForwardListView();

                var totalCount = leads.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
                var leadsPerPage = leads.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                _response.IsSuccess = true;
                _response.Message = "";
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
                _response.Message = "Error while getting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetLeadWithLeadForward")]
        public async Task<ResponseDto> GetLeadForwardByLeadNo([FromQuery] string leadNo, [FromBody] AuthDto authDto)
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
                var existingLogData = await _unitOfWork.leadForwardInterface.GetLeadForwardWithLog(leadNo);

                if (existingLogData == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "lead with this Id does not exist";
                    return _response;
                }

                _response.Message = "";
                _response.Result = existingLogData;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("AddNewLeadForward")]
        public async Task<ResponseDto> AddNewLeadForward(CreateNewLeadForwardDto createNewLeadForwardDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(createNewLeadForwardDto.AuthDto);

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
                Location = AccessLocation.LeadForward.ToString()
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
                var existingLead = await _unitOfWork.leadsInterface.GetLeadsByIdAsync(createNewLeadForwardDto.Leadid);

                if (existingLead == null)
                {
                    _response.Message = "Lead with this id does not exist";
                    _response.IsSuccess = false;
                    return _response;
                }

                var existingStaff = await _unitOfWork.staffInterface.GetStaffByIdAsync(createNewLeadForwardDto.Forwardstaffid);

                if (existingStaff == null)
                {
                    _response.Message = "Staff with this id does not exist";
                    _response.IsSuccess = false;
                    return _response;
                }

                existingLead.Staffid = existingStaff.Id;
                existingLead.Assigned = existingStaff.Id;

                var newItem = new TblLeadforward
                {
                    Addby = authResponse.Result.Userid,
                    Addon = DateTime.UtcNow,
                    Date = createNewLeadForwardDto.Date,
                    Forwardfromid = existingLead.Staffid.ToString(),
                    Forwardstaffid = existingStaff.Id.ToString(),
                    Leadid = existingLead.Leadno,
                    Reason = createNewLeadForwardDto.Reason
                };

                var newLog = new TblLeadlog
                {
                    Addby = authResponse.Result.Userid,
                    Addon = DateTime.UtcNow,
                    Leadid = existingLead.Leadno,
                    Log = "Forwaded Lead from " + existingLead.Assigned + " to " + existingStaff.Name + " with a remark " + createNewLeadForwardDto.Reason,
                };

                await _db.TblLeadforwards.AddAsync(newItem);
                await _db.TblLeadlogs.AddAsync(newLog);

                await _db.SaveChangesAsync();

                _response.Message = "Lead forwarded Successfully";
                _response.IsSuccess = true;
                _response.Result = newItem;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("EditExistingLeadForward")]
        public async Task<ResponseDto> EditExistingLeadForward(EditExistingLeadForwardDto editExistingLeadForwardDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(editExistingLeadForwardDto.AuthDto);

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
                Location = AccessLocation.LeadForward.ToString()
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
                var existingLeadForward = await _unitOfWork.leadForwardInterface.GetLeadForwardsById(editExistingLeadForwardDto.Id);

                if (existingLeadForward == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "lead forward with this Id does not exist";
                    return _response;
                }

                existingLeadForward.Forwardfromid = existingLeadForward.Forwardfromid;
                existingLeadForward.Forwardstaffid = editExistingLeadForwardDto.Forwardstaffid.ToString();
                existingLeadForward.Leadid = editExistingLeadForwardDto.Leadid;
                existingLeadForward.Date = editExistingLeadForwardDto.Date;
                existingLeadForward.Reason = editExistingLeadForwardDto.Reason;

                var newLog = new TblLeadlog
                {
                    Addby = authResponse.Result.Userid,
                    Addon = DateTime.UtcNow,
                    Leadid = existingLeadForward.Leadid,
                    Log = "Forwaded Lead Edited on " + DateTime.UtcNow + " from the user " + authResponse.Result.Userid,
                };

                await _db.TblLeadlogs.AddAsync(newLog);

                await _db.SaveChangesAsync();

                _response.Message = "Lead Forwarded Edited Successfully";
                _response.IsSuccess = true;
                _response.Result = newLog;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }
    }
}
