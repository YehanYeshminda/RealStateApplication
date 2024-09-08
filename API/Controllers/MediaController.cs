using API.Models;
using API.Repos.Dtos;
using API.Repos.Dtos.MediaDtos;
using API.Repos.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/media")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly CRMContext _db;
        private readonly ResponseDto _response;
        public MediaController(CRMContext context)
        {
            _db = context;
            _response = new ResponseDto();
        }

        [HttpPost("GetAllMedia")]
        public async Task<ResponseDto> GetAllMedia(AuthDto authDto)
        {
            if (authDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(authDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == authDto.Hash);

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
                    var existingMediaLinks = await _db.TblMedia.ToListAsync();

                    _response.IsSuccess = true;
                    _response.Message = "";
                    _response.Result = existingMediaLinks;

                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while getting media! " + ex.Message;
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

        [HttpPost("GetSingleMedia")]
        public async Task<ResponseDto> GetMediaById([FromQuery]int id ,[FromBody]AuthDto authDto)
        {
            if (authDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(authDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == authDto.Hash);

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
                    var existingMedia = await _db.TblMedia.FirstOrDefaultAsync(x => x.Id == id);

                    if (existingMedia == null)
                    {
                        _response.Message = "Media with this Id does not exist";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    _response.IsSuccess = true;
                    _response.Message = "";
                    _response.Result = existingMedia;

                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while getting single media! " + ex.Message;
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

        [HttpPost("AddNewMedia")]
        public async Task<ResponseDto> AddNewMedia(CreateNewMediaDto createNewMediaDto)
        {
            if (createNewMediaDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(createNewMediaDto.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == createNewMediaDto.AuthDto.Hash);

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
                    var existingMedia = await _db.TblMedia.FirstOrDefaultAsync(x => x.Media == createNewMediaDto.Media);

                    if (existingMedia != null)
                    {
                        _response.Message = "Media with this name already exists";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    var existingCompany = await _db.Tblcompanies.FirstOrDefaultAsync(x => x.Id == createNewMediaDto.Cid);

                    if (existingMedia != null)
                    {
                        _response.Message = "Company with this ID does not exist";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    var newMedia = new TblMedium
                    {
                        Cid = existingCompany.Id,
                        Media = createNewMediaDto.Media,
                        Remark = createNewMediaDto.Remark,
                        Status = createNewMediaDto.Status
                    };

                    await _db.TblMedia.AddAsync(newMedia);
                    await _db.SaveChangesAsync();

                    _response.Message = "";
                    _response.IsSuccess = true;
                    _response.Result = newMedia;

                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while creating new  media! " + ex.Message;
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

        [HttpPost("EditExistingMedia")]
        public async Task<ResponseDto> EditExistingMedia(EditExistingMediaDto editExistingMediaDto)
        {
            if (editExistingMediaDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(editExistingMediaDto.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == editExistingMediaDto.AuthDto.Hash);

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
                    var existingMedia = await _db.TblMedia.FirstOrDefaultAsync(x => x.Id == editExistingMediaDto.Id);

                    if (existingMedia != null)
                    {
                        _response.Message = "media with this ID does not exist";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    existingMedia.Media = editExistingMediaDto.Media;
                    existingMedia.Status = editExistingMediaDto.Status;
                    existingMedia.Remark = editExistingMediaDto.Remark;
                    existingMedia.Cid = editExistingMediaDto.Cid;

                    await _db.SaveChangesAsync();

                    _response.Message = "";
                    _response.IsSuccess = true;
                    _response.Result = existingMedia;

                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while editing existing media! " + ex.Message;
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
