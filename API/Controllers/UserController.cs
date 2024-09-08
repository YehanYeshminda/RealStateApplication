using API.Repos;
using API.Repos.Dtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseDto _response;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _response = new ResponseDto();
        }

        [HttpPost("GetAllUserNameId")]
        public async Task<ResponseDto> GetAllUserNameId(AuthDto authDto)
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
                var allUserNameIds = await _unitOfWork.userInterface.GetAllUsersNameAndId();

                _response.Result = allUserNameIds;
                _response.Message = "";
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting name and id! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetAllUserComboPermission")]
        public async Task<ResponseDto> GetAllUserPermission(AuthDto authDto, string userId)
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
                Location = AccessLocation.UserPermission.ToString()
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
                //var allUserNameIds = await _unitOfWork.userpermissionInterface.GetAllUserPermissionById(userId);
                var all = await _unitOfWork.userpermissionInterface.GetDesignationById(userId);

                _response.Result = all; 
                _response.Message = "";
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting name and id! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }
    }
}
