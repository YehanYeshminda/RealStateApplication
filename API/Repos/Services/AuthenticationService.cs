using API.Models;
using API.Repos.Dtos;
using API.Repos.Dtos.AuthBase;
using API.Repos.Helpers;
using API.Repos.Interfaces;

namespace API.Repos.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly CRMContext _db;
        private readonly AuthBaseResponseDto _response;

        public AuthenticationService(CRMContext context)
        {
            _db = context;
            _response = new AuthBaseResponseDto();
        }

        public AuthBaseResponseDto ValidateAuthentication(AuthDto authDto)
        {
            if (authDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(authDto.Hash);

            var user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId);

            if (user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid Hash";
                return _response;
            }

            var decryptedDateWithOffset = decodedValues.Date.AddDays(1);
            var currentDate = DateTime.Now;

            

                try
                {
                    _response.Result = user;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while inserting! " + ex.Message;
                    _response.IsSuccess = false;
                    return _response;
                }
        }
    }
}
