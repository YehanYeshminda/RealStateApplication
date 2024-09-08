using API.Models;
using API.Repos.Dtos;
using API.Repos.Dtos.BankDtos;
using API.Repos.Dtos.SourceDto;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        private readonly ISourceInterface _sourceInterface;
        private readonly CRMContext _db;
        private readonly ResponseDto _response;

        public SourceController(ISourceInterface sourceInterface, CRMContext context)
        {
            _sourceInterface = sourceInterface;
            _db = context;
            _response = new ResponseDto();
        }

        [HttpPost("AddNewSource")]
        public async Task<ResponseDto> AddNewSource(CreateSourceDto createSourceDto)
        {
            if (createSourceDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(createSourceDto.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == createSourceDto.AuthDto.Hash);

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
                    var newSource = await _sourceInterface.CreateSourceAsync(createSourceDto);

                    if (!newSource.IsSuccess)
                    {
                        _response.IsSuccess = false;
                        _response.Message = newSource.Message;
                        return _response;
                    }

                    return newSource;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while creating new source! " + ex.Message;
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

        [HttpPut("EditExistingSource")]
        public async Task<ResponseDto> EditExistingSource(CreateSourceDto createSourceDto)
        {
            if (createSourceDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(createSourceDto.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == createSourceDto.AuthDto.Hash);

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
                    var newSource = await _sourceInterface.EditSourceAsync(createSourceDto);

                    if (!newSource.IsSuccess)
                    {
                        _response.IsSuccess = false;
                        _response.Message = newSource.Message;
                        return _response;
                    }

                    return newSource;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while editing existing source! " + ex.Message;
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
