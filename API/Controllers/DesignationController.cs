using API.Models;
using API.Repos;
using API.Repos.Dtos;
using API.Repos.Dtos.Designation;
using API.Repos.Dtos.StaffDtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/designation")]
    [ApiController]
    public class DesignationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CRMContext _db;
        private readonly ResponseDto _response;
            
        public DesignationController(IUnitOfWork unitOfWork, CRMContext context)
        {
            _unitOfWork = unitOfWork;
            _db = context;
            _response = new ResponseDto();
        }

        [HttpPost("Get")]
        public async Task<ResponseDto> GetAllDesignations([FromBody] AuthDto authDto, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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
                Location = AccessLocation.Designation.ToString()
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
                var existingDesinations = await _db.TblDesignationtypes
                .Where(d => d.Status == 0)
                .ToListAsync();


                var totalCount = existingDesinations.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
                var leadsPerPage = existingDesinations.Skip((page - 1) * pageSize).Take(pageSize).ToList();

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
                _response.IsSuccess = false;
                _response.Message = "Error while getting designations. " + ex.Message;
                return _response;
            }
        }

        [HttpPost("Add")]
        public async Task<ResponseDto> AddNewDesignation(AddNewDesignationDto addNewDesignationDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(addNewDesignationDto.authDto);

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
                Location = AccessLocation.Designation.ToString()
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
                var existingDesignation = await _db.TblDesignationtypes.FirstOrDefaultAsync(x => x.TypeName == addNewDesignationDto.Name);

                if (existingDesignation != null)
                {
                    _response.Message = "Designation with this name already exists";
                    _response.IsSuccess = false;
                    return _response;
                }

                var newDesignation = new TblDesignationtype
                {
                    Cid = 1,
                    TypeName = addNewDesignationDto.Name,
                    Remark = addNewDesignationDto.Remark,
                    Status = 0,
                };

                await _db.TblDesignationtypes.AddAsync(newDesignation);
                await _db.SaveChangesAsync();

                _response.Message = "Successfully saved designation";
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while saving designation. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("DeleteDesignation")]
        public async Task<ResponseDto> DeleteDesignation([FromBody] AuthDto authDto, [FromQuery] int id)
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

                var existingdata = await _db.TblDesignationtypes.FirstOrDefaultAsync(x => x.TypeId == id);

                if (existingdata == null)
                {
                    _response.Message = "Unable to find designation with this id";
                    _response.IsSuccess = false;
                    return _response;
                }

                existingdata.Status = 1;
                _db.TblDesignationtypes.Update(existingdata);

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Successfully deleted designation";
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
