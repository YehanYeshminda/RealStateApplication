using API.Models;
using API.Repos.Dtos;
using API.Repos.Helpers;
using Microsoft.AspNetCore.Mvc;
using API.Repos.Dtos.LeadAssignDtos;
using Microsoft.EntityFrameworkCore;
using API.Repos.Interfaces;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos;

namespace API.Controllers
{

    [Route("api/leadAssign")]
    [ApiController]
    public class LeadAssignController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public LeadAssignController(CRMContext db, IUnitOfWork unitOfWork)
        {
            _response = new ResponseDto();
            _db = db;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("insertLeadAssign")]
        public async Task<ResponseDto> Insert(LeadAssignDto leadAssignDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(leadAssignDto.AuthDto);

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
                Location = AccessLocation.LeadSegregation.ToString()
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
                if (leadAssignDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Missing User Data";
                    return _response;
                }

                var existingStaff = await _unitOfWork.staffInterface.GetStaffByIdAsync(leadAssignDto.Staffid);

                if (existingStaff == null)
                {
                    _response.IsSuccess = true;
                    _response.Message = "Staff with this id does not exist";
                    return _response;
                }

                foreach (var item in leadAssignDto.Leadid)
                {
                    var existingLead = await _unitOfWork.leadsInterface.GetLeadsByIdAsync(item);

                    if (existingLead == null)
                    {
                        _response.IsSuccess = true;
                        _response.Message = "Lead with Id " + item + "does not exist";
                        return _response;
                    }

                    existingLead.Staffid = existingStaff.Id;
                    existingLead.Assigned = existingStaff.Id;
                    existingLead.Assignon = DateTime.UtcNow;
                    existingLead.Status = 0;

                    var newLeadAssign = new TblleadAssign
                    {
                        Addby = authResponse.Result.Userid,
                        Addon = DateTime.UtcNow,
                        Leadid = item,
                        Staffid = existingStaff.Id,
                        Status = 1,
                    };

                    await _db.TblleadAssigns.AddAsync(newLeadAssign);

                    var newLog = new TblLeadlog
                    {
                        Addby = authResponse.Result.Userid,
                        Addon = DateTime.UtcNow,
                        Leadid = item,
                        Log = "Lead has been Assigned from " + existingLead.Staffid + " with the name " + existingLead.Name + " to staff " + existingStaff.Id + " and name of " + existingStaff.Name + " with a remark of " + leadAssignDto.Remark
                    };

                    await _db.TblLeadlogs.AddAsync(newLog);
                }

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Successfully Assigned Leads";
                _response.Result = "";
                return _response;

            }
            catch (Exception ex)
            {
                _response.Message = "Error while inserting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("removeLeadAssign")]
        public async Task<ResponseDto> RemoveAssignedLeads(LeadAssignDto leadAssignDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(leadAssignDto.AuthDto);

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
                Location = AccessLocation.LeadSegregation.ToString()
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
                if (leadAssignDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Missing User Data";
                    return _response;
                }

                var existingStaff = await _unitOfWork.staffInterface.GetStaffByIdAsync(leadAssignDto.Staffid);

                if (existingStaff == null)
                {
                    _response.IsSuccess = true;
                    _response.Message = "Staff with this id does not exist";
                    return _response;
                }

                foreach (var item in leadAssignDto.Leadid)
                {
                    var existingLeadAssigns = await _unitOfWork.leadAssignInterface.GetLeadAssignsByStaffId(existingStaff.Id, item);

                    if (existingLeadAssigns == null)
                    {
                        _response.Message = "Lead is not assigned in order to remove allocation";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    _db.TblleadAssigns.Remove(existingLeadAssigns);

                    var existingLead = await _unitOfWork.leadsInterface.GetLeadsByIdAsync(item);

                    if (existingLead == null)
                    {
                        _response.IsSuccess = true;
                        _response.Message = "Lead with Id " + item + "does not exist";
                        return _response;
                    }

                    existingLead.Staffid = 0;
                    existingLead.Assigned = 0;
                    existingLead.Assignon = DateTime.UtcNow;
                    existingLead.Status = 0;

                    var newLog = new TblLeadlog
                    {
                        Addby = authResponse.Result.Userid,
                        Addon = DateTime.UtcNow,
                        Leadid = item,
                        Log = "Unallocated lead from " + existingLead.Name + " and reallocated to " + existingStaff.Name
                    };

                    await _db.TblLeadlogs.AddAsync(newLog);
                }

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Successfully Deallocated Leads";
                _response.Result = "";
                return _response;

            }
            catch (Exception ex)
            {
                _response.Message = "Error while inserting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }


        [HttpPost("GetLeadDependingOnStaff")]
        public async Task<ResponseDto> GetLeadsDependingOnStaff([FromQuery]int staffId ,[FromBody] AuthDto authDto)
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
                var newLeadsOfStaff = new List<LeadDependingOnStaffDto>();
                var existingLeadOnStaff = await _unitOfWork.leadAssignInterface.GetLeadAssigningsByStaff(staffId);

                if (existingLeadOnStaff.Count() > 0)
                {
                    foreach (var item in existingLeadOnStaff)
                    {
                        var user = await _unitOfWork.userInterface.GetUserById(Convert.ToInt32(item.Addby));
                        var lead = await _unitOfWork.leadsInterface.GetLeadsByIdAsync(item.Leadid);
                        var staff = await _unitOfWork.staffInterface.GetStaffByIdAsync(item.Staffid);

                        var newItem = new LeadDependingOnStaffDto
                        {
                            Id = item.Id,
                            AddBy = user.Username,
                            AddOn = item.Addon,
                            Lead = lead.Name,
                            Staff = staff.Name,
                            Status = item.Status
                        };

                        newLeadsOfStaff.Add(newItem);
                    }
                }


                _response.IsSuccess = true;
                _response.Result = newLeadsOfStaff;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpGet("getLeadAssign")]
        public async Task<ActionResult<List<TblleadAssign>>> GetLeads(string hash)
        {
            if (hash == null)
            {
                return BadRequest("Please provide hash");
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == hash);

            if (_user == null)
            {
                return Unauthorized("Invalid Hash");
            }

            var decryptedDateWithOffset = decodedValues.Date.AddDays(1);
            var currentDate = DateTime.UtcNow.Date;

            if (currentDate < decryptedDateWithOffset.Date)
            {
                try
                {

                    List<TblleadAssign> leads = await _db.TblleadAssigns.ToListAsync();

                    return Ok(leads);
                }
                catch (Exception ex)
                {
                    return BadRequest("Error while retrieving LeadAssign: " + ex.Message);
                }
            }
            else
            {
                return Unauthorized("Invalid Hash");
            }
        }

    }
}
