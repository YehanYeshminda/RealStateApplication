using API.Models;
using API.Repos;
using API.Repos.Dtos;
using API.Repos.Dtos.CommonDto;
using API.Repos.Dtos.LeadsDtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public LeadsController(CRMContext db, IUnitOfWork unitOfWork)
        {
            _response = new ResponseDto();
            _db = db;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetLeadStatusResults")]
        public async Task<ResponseDto> GetOpenLeadCount()
        {
            try
            {
                var openLeads = await _unitOfWork.leadsInterface.GetOpenLeadCount();
                var hotLeads = await _unitOfWork.leadsInterface.GetHotLeadCount();
                var convesions = await _unitOfWork.leadsInterface.GetConversionCount();


                var leadsdResult = new
                {
                    openLeadCount = openLeads,
                    hotLeadCount = hotLeads,
                    conversions = convesions
                };

                _response.Result = leadsdResult;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting leads! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetAllLeads")]
        public async Task<ResponseDto> GetAllLeads([FromBody] AuthDto authDto, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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
                Location = AccessLocation.Lead.ToString()
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
                var leads = await _unitOfWork.leadsInterface.GetLeadListViewForNew();

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
                _response.Message = "Error while inserting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetAllLeadsByStaffId")]
        public async Task<ResponseDto> GetAllLeadsByStaffId([FromBody] AuthDto authDto, [FromQuery] int staffId)
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
                Location = AccessLocation.Lead.ToString()
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
                var leads = await _unitOfWork.leadsInterface.GetLeadListViewForNewFilteredByStaff(staffId);

                _response.IsSuccess = true;
                _response.Message = "";
                _response.Result = leads;
                
                return _response;

            }
            catch (Exception ex)
            {
                _response.Message = "Error while inserting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        public class DeleteLeadsDto
        {
            public AuthDto authDto { get; set; }
            public string LeadNo { get; set; }
        }

        [HttpPost("DeleteLeads")]
        public async Task<ResponseDto> DeleteLeadFromLeads(DeleteLeadsDto deleteLeadsDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(deleteLeadsDto.authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var existingEmployee = await _db.Tblstaffs.FirstOrDefaultAsync(x => x.Id == authResponse.Result.Userid);

                if (existingEmployee == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Unable to find employee with this id";
                    return _response;
                }

                if (existingEmployee.Designation != "CEO")
                {
                    _response.Message = "Insufficient permission to delete leads";
                    _response.IsSuccess = false;
                    return _response;
                }
                
                var existingLead = await _db.Tblleads.FirstOrDefaultAsync(x => x.Leadno == deleteLeadsDto.LeadNo);

                if (existingLead == null)
                {
                    _response.Message = "Unable to find lead to delete";
                    _response.IsSuccess = false;
                    return _response;
                }
                
                existingLead.Status = 1;
                _db.Tblleads.Update(existingLead);
                await _db.SaveChangesAsync();

                _response.Message = "Successfully removed lead";
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while deleting lead! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpGet("GetAllLeadsByStatusImportance")]
        public async Task<ResponseDto> GetAllLeadsByStatusAndName([FromQuery] int staffId, int importance, int pageSize, int page) 
        {
            try
            {
                var leads = await _unitOfWork.leadsInterface.GetAllLeadsByStaffIdAndImportance(staffId, importance);

                var totalCount = leads.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
                var leadsPerPage = leads.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                _response.Message = "";
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
                _response.Message = "Error while getting leads for filter! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("UpdateLeadstatus")]
        public async Task<ResponseDto> UpdateLeadstatus(UpdateLeadStatusDto updateLeadStatusDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(updateLeadStatusDto.AuthDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }
            
            try
            {
                var existingLead = await _unitOfWork.leadsInterface.GetLeadsByIdAsync(updateLeadStatusDto.leadNo);

                if (existingLead == null)
                {
                    _response.Message = "Unable to find lead with this id";
                    _response.IsSuccess = false;
                    return _response;
                }
                
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

                DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);

                var beforeStatus = existingLead.Importance;
                var leadStatusBefore = await _db.TblLeadStatuses.FirstOrDefaultAsync(x => x.Id == beforeStatus);

                if (leadStatusBefore == null)
                {
                    _response.Message = "Unable to find status";
                    _response.IsSuccess = false;
                    return _response;
                }

                existingLead.Importance = updateLeadStatusDto.Status;
                
                var leadStatusAfter = await _db.TblLeadStatuses.FirstOrDefaultAsync(x => x.Id == existingLead.Importance);
                
                if (leadStatusAfter == null)
                {
                    _response.Message = "Unable to find status";
                    _response.IsSuccess = false;
                    return _response;
                }

                _db.Tblleads.Update(existingLead);

                var newLog = new TblLeadlog
                {
                    Leadid = existingLead.Leadno,
                    Log =   "Changed the status from " + leadStatusBefore.Leadstatus + " at " + dubaiTime + " to " + leadStatusAfter.Leadstatus + " at " + dubaiTime,
                    Addon = dubaiTime,
                    Addby = authResponse.Result.Userid,
                };

                await _db.TblLeadlogs.AddAsync(newLog);

                _response.Message = "Updated Lead Status";
                await _db.SaveChangesAsync();

                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating status! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetLeadsById")]
        public async Task<ResponseDto> GetLeadById([FromBody]AuthDto authDto, [FromQuery] string leadNo)
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
                var leads = await _unitOfWork.leadsInterface.GetLeadsByIdAsync(leadNo);

                if (leads == null)
                {
                    _response.Message = "Lead with this id is not found";
                    _response.IsSuccess = false;
                    return _response;
                }

                _response.IsSuccess = true;
                _response.Message = "";
                _response.Result = leads;
                return _response;

            }
            catch (Exception ex)
            {
                _response.Message = "Error while inserting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetLeadNameConMethod")]
        public async Task<ResponseDto> GetLeadNameWithContactMethod([FromQuery] string leadNo, AuthDto authDto)
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
                var lead = await _unitOfWork.leadsInterface.GetLeadsByIdAsync(leadNo);

                if (lead == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "lead with this name does not exist";
                    return _response;
                }

                var contactMethod = await _unitOfWork.contactMethodInterface.GetContactMethod(lead.ContactMethod ?? 0);

                if (contactMethod == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "contact method with does not exist";
                    return _response;
                }

                var newItem = new ReturnLeadDto
                {
                    ContactMethod = contactMethod.ContactMethod,
                    LeadName = lead.Name,
                    Lead = lead,
                };

                _response.IsSuccess = true;
                _response.Result = newItem;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting lead name! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("insert")]
        public async Task<ResponseDto> Insert(leadsdto leadDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(leadDto.AuthDto);

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
                Location = AccessLocation.Lead.ToString()
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
                if (leadDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Missing User Data";
                    return _response;
                }

                var existingLead = await _unitOfWork.leadsInterface.GetLeadsByNameAsync(leadDto.Name);

                if (existingLead != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "lead with this name already exists";
                    return _response;
                }

                var existingSource = await _unitOfWork.sourceInterface.GetSourcesByIdAsync(leadDto.Sourceid);

                if (existingSource == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "source with this id does not exist";
                    return _response;
                }

                var existingStatus = await _unitOfWork.leadStatusInterface.GetLeadStatusByName("Open");

                if (existingStatus == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Default status does not exist";
                    return _response;
                }
                
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

                DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);

                var newUser = new Tbllead
                {
                    Leadno = leadDto.Leadno,
                    Sourceid = existingSource.Id,
                    Name = leadDto.Name,
                    Phone = leadDto.Phone,
                    Email = leadDto.Email,
                    Otherno = leadDto.Otherno,
                    Recievedon = dubaiTime,
                    Assignon = dubaiTime,
                    Status = leadDto.Status,
                    Staffid = authResponse.Result.Userid,
                    Importance = existingStatus.Id,
                    AddedOn = dubaiTime
                };
                
                
                var newLeadAssign = new TblleadAssign();

                if (leadDto.AssignedTo != null) 
                {
                    newLeadAssign.Addby = authResponse.Result.Userid;
                    newLeadAssign.Addon = dubaiTime;
                    newLeadAssign.Leadid = leadDto.Leadno;
                }


                if (leadDto.AssignedTo != null)
                {
                    var existingStaff = await _unitOfWork.staffInterface.GetStaffByIdAsync(leadDto.AssignedTo ?? 0);

                    if (existingStaff == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "staff with this id does not exist";
                        return _response;
                    }

                    newUser.Assigned = existingStaff.Id;
                    newLeadAssign.Staffid = existingStaff.Id;
                    newUser.Status = 1;
                }

                if (leadDto.LeadStatus != null) 
                {
                    var existingLeadStatus = await _unitOfWork.leadStatusInterface.GetLeadStatusBYId(leadDto.LeadStatus ?? 0);

                    if (existingLeadStatus == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Lead status with this id does not exist";
                        return _response;
                    }

                    newUser.Importance = existingLeadStatus.Id;
                }

                var existingPreferedContactMethod = await _unitOfWork.preferedContactMethodInterface.GetContactMethodById(leadDto.ContactMethod);

                if (existingPreferedContactMethod == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Contact Method with this id does not exist";
                    return _response;
                }

                newUser.ContactMethod = existingPreferedContactMethod.Id;

                if (leadDto.Campainid != null)
                {
                    var existingCampaign = await _unitOfWork.campaignInterface.GetCompaignByIdAsync(leadDto.Campainid);

                    if (existingCampaign == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "campaign with this id does not exist";
                        return _response;
                    }

                    newUser.Campainid = existingCampaign.No;
                }
                else
                {
                    newUser.Campainid = "0";
                }

                var existingControl = await _db.Tblcontrols.FirstOrDefaultAsync();

                string numericPart = StaticHelpers.NormalizeLeadNo(Regex.Match(leadDto.Leadno, @"\d+").Value);

                await _db.Tblleads.AddAsync(newUser);

                if (leadDto.AssignedTo != null)
                {
                    await _db.TblleadAssigns.AddAsync(newLeadAssign);
                }

                existingControl.LeadNo = Convert.ToInt32(numericPart);
                _db.Tblcontrols.Update(existingControl);

                var newLeadLog = new TblLeadlog
                {
                    Addby = authResponse.Result.Userid,
                    Addon = dubaiTime,
                    Leadid = newUser.Leadno,
                    Log = "Assigned on : " + dubaiTime,
                };

                await _db.TblLeadlogs.AddAsync(newLeadLog);

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Successfully created " + newUser.Leadno;
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


        [HttpPost("update")]
        public async Task<ResponseDto> UpdateLead(leadsdto leadDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(leadDto.AuthDto);

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
                Location = AccessLocation.Lead.ToString()
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
                if (leadDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Missing User Data";
                    return _response;
                }

                var existingLead = await _unitOfWork.leadsInterface.GetLeadsByIdAsync(leadDto.Leadno);

                if (existingLead == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Lead not found";
                    return _response;
                }


                existingLead.Sourceid = leadDto.Sourceid;
                existingLead.Campainid = leadDto.Campainid;
                existingLead.Name = leadDto.Name;
                existingLead.Phone = leadDto.Phone;
                existingLead.Email = leadDto.Email;
                existingLead.Otherno = leadDto.Otherno;
                existingLead.Status = leadDto.Status;
                existingLead.Assigned = leadDto.AssignedTo ?? 0;
                existingLead.Importance = leadDto.LeadStatus ?? 0;
                existingLead.ContactMethod = leadDto.ContactMethod;

                if (existingLead.Comments != "" || existingLead.Comments != null)
                {
                    existingLead.Comments = leadDto.Comment;
                }
                
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

                DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);
                
                var newLog = new TblLeadlog
                {
                    Addby = authResponse.Result.Userid,
                    Addon = dubaiTime,
                    Leadid = existingLead.Leadno,
                    Log = "Updated Lead " + existingLead.Leadno + " at " + dubaiTime,
                };

                await _db.TblLeadlogs.AddAsync(newLog);
                await _db.SaveChangesAsync();
                
                _response.IsSuccess = true;
                _response.Message = "Successfully updated lead: " + existingLead.Name;
                _response.Result = existingLead;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating lead! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetLeadForwardById")]
        public async Task<ResponseDto> GetResponseById([FromRoute] string id, [FromBody] AuthDto authDto)
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
                var existingLead = await _unitOfWork.leadsInterface.GetLeadsByIdAsync(id);

                if (existingLead == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Lead with this Id does not exist";
                    return _response;
                }

                _response.IsSuccess = true;
                _response.Result = existingLead;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting lead! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        public class LineChartData
        {
            public List<string> Labels { get; set; } = new List<string>();
            public List<int> Datasets { get; set; } = new List<int>();
        }
        
        public class FilterLeadByStatusDto
        {
            public int LeadStatus { get; set; }
            public AuthDto authDto { get; set; }
            public int pageSize { get; set; }
            public int page { get; set; }
        }

        [HttpPost("FilterLeadByStatus")]
        public async Task<ResponseDto> FilterLeadByStatus(FilterLeadByStatusDto filterLeadByStatusDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(filterLeadByStatusDto.authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var leads = await _unitOfWork.leadsInterface.GetLeadListViewForNewFiltered(filterLeadByStatusDto.LeadStatus);
                var totalCount = leads.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / filterLeadByStatusDto.pageSize);
                var leadsPerPage = leads.Skip((filterLeadByStatusDto.page - 1) * filterLeadByStatusDto.pageSize).Take(filterLeadByStatusDto.pageSize).ToList();

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
                _response.Message = "Error while getting leads data! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetLeadsChartData")]
        public async Task<ResponseDto> GetLeadChartData(AuthDto authDto)
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
                var leadData = _db.Tblleads
                    .Where(lead => lead.Recievedon != null)
                    .GroupBy(lead => lead.Recievedon.Month)
                    .OrderBy(group => group.Key)
                    .Select(group => new
                    {
                        Month = DateTimeFormatInfo.CurrentInfo.GetMonthName(group.Key),
                        TotalLeads = group.Count()
                    })
                    .ToList();

                var labels = new List<string>();
                var data = new List<int>();

                foreach (var item in leadData)
                {
                    labels.Add(item.Month);
                    data.Add(item.TotalLeads);
                }

                var lineChartData = new LineChartData();
                lineChartData.Labels = labels;
                lineChartData.Datasets = data;

                _response.Result = lineChartData;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting chart data! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetLeadInfoForDialog")]
        public async Task<ResponseDto> GetLeadInfoWithAllInfo([FromBody] AuthDto authDto, [FromQuery] string leadNo)
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
                var existingLead = await _unitOfWork.leadsInterface.GetLeadsByIdAsync(leadNo);

                if (existingLead == null)
                {
                    _response.Message = "Lead with this id does not exist! ";
                    _response.IsSuccess = false;
                    return _response;
                }

                existingLead.Called = 1;

                var existingSource = await _unitOfWork.sourceInterface.GetSourcesByIdAsync(existingLead.Sourceid);

                if (existingSource == null)
                {
                    _response.Message = "Source does not exist! ";
                    _response.IsSuccess = false;
                    return _response;
                }

                var itemsToReturn = new
                {
                    Source = existingSource.Source,
                    LeadName = existingLead.Name,
                    ReceivedOn = existingLead.Recievedon,
                    leadStatus = existingLead.Importance
                };
                
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

                DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);

                var newTblLog = new TblLeadlog
                {
                    Addby = authResponse.Result.Userid,
                    Addon = dubaiTime,
                    Leadid = existingLead.Leadno,
                    Log = "Called Lead at " + dubaiTime + " with the leadId of " + existingLead.Leadno + " by staff " + authResponse.Result.Userid
                };

                await _db.TblLeadlogs.AddAsync(newTblLog);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Result = itemsToReturn;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting lead Info data! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetLeadNameId")]
        public async Task<ActionResult<List<CommonDto>>> GetLeadNameAndId(AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                return BadRequest(authResponse.Message);
            }

            try
            {
                var newList = new List<BankInfoDto>();
                var existingBanks = await _db.Tblleads.Where(x => x.Status == 0).ToListAsync();

                foreach (var item in existingBanks)
                {
                    var newItemToAdd = new BankInfoDto
                    {
                        textValue = item.Name,
                        value = item.Leadno
                    };

                    newList.Add(newItemToAdd);
                }

                return Ok(newList);
            }
            catch (Exception ex)
            {
                return BadRequest("Error while getting banks! " + ex.Message);
            }
        }

        [HttpPost("GetLeadNameIdAll")]
        public async Task<ActionResult<List<CommonDto>>> GetLeadNameAndIdAll(AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                return BadRequest(authResponse.Message);
            }

            try
            {
                var existingLeadForward = _unitOfWork.leadForwardInterface.GetBankInfoByLeadId();
                return Ok(existingLeadForward);
            }
            catch (Exception ex)
            {
                return BadRequest("Error while getting banks! " + ex.Message);
            }
        }


    }
}
