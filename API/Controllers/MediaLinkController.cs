using API.Models;
using API.Repos.Dtos;
using API.Repos.Dtos.MediaDtos;
using API.Repos.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaLinkController : ControllerBase
    {
        private readonly CRMContext _db;
        private readonly ResponseDto _response;

        public MediaLinkController(CRMContext cRMContext)
        {
            _db = cRMContext;
            _response = new ResponseDto();
        }


        [HttpPost("GetAllMediaLink")]
        public async Task<ResponseDto> GetAllMediaLinks(AuthDto authDto)
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
                    var existingMediaLinks = await _db.TblMediaLinks.ToListAsync();

                    _response.IsSuccess = true;
                    _response.Message = "";
                    _response.Result = existingMediaLinks;

                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while getting media links! " + ex.Message;
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

        //[HttpPost("AddMediaLink")]
        //public async Task<ResponseDto> AddNewMediaLinks(CreateNewMediaLinkDto createNewMediaLinkDto)
        //{
        //    if (createNewMediaLinkDto.AuthDto.Hash == null)
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = "Please provide hash";
        //        return _response;
        //    }

        //    HelperAuth decodedValues = AuthValidator.DecodeValue(createNewMediaLinkDto.AuthDto.Hash);

        //    var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == createNewMediaLinkDto.AuthDto.Hash);

        //    if (_user == null)
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = "Invalid Hash";
        //        return _response;
        //    }

        //    var decryptedDateWithOffset = decodedValues.Date.AddDays(1);
        //    var currentDate = DateTime.UtcNow.Date;

        //    if (currentDate < decryptedDateWithOffset.Date)
        //    {
        //        try
        //        {
        //            var existingMedia = await _db.TblMedia.FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(createNewMediaLinkDto.Mediaid));

        //            if (existingMedia == null)
        //            {
        //                _response.Message = "Media with this id does not exist";
        //                _response.IsSuccess = false;
        //                return _response;
        //            }

        //            var existingCompaign = await _db.TblCampainHs.FirstOrDefaultAsync(x => x.No == createNewMediaLinkDto.Campainno);

        //            if (existingCompaign == null)
        //            {
        //                _response.Message = "Compaign with this id does not exist";
        //                _response.IsSuccess = false;
        //                return _response;
        //            }

        //            var newItem = new TblMediaLink
        //            {
        //                Campainno = createNewMediaLinkDto.Campainno,
        //                Medialink = createNewMediaLinkDto.Mediaid,
        //            };

        //            await _db.TblMediaLinks.AddAsync(newItem);
        //            await _db.SaveChangesAsync();

        //            _response.IsSuccess = true;
        //            _response.Message = "";
        //            _response.Result = newItem;
        //            return _response;
        //        }
        //        catch (Exception ex)
        //        {
        //            _response.Message = "Error while creating new media links! " + ex.Message;
        //            _response.IsSuccess = false;
        //            return _response;
        //        }
        //    }
        //    else
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = "Invalid Hash";
        //        return _response;
        //    }
        //}

        //[HttpPut("EditMediaLink")]
        //public async Task<ResponseDto> EditExistingMediaLinks(EditExistingMediaLinkDto editExistingMediaLinkDto)
        //{
        //    if (editExistingMediaLinkDto.AuthDto.Hash == null)
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = "Please provide hash";
        //        return _response;
        //    }

        //    HelperAuth decodedValues = AuthValidator.DecodeValue(editExistingMediaLinkDto.AuthDto.Hash);

        //    var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == editExistingMediaLinkDto.AuthDto.Hash);

        //    if (_user == null)
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = "Invalid Hash";
        //        return _response;
        //    }

        //    var decryptedDateWithOffset = decodedValues.Date.AddDays(1);
        //    var currentDate = DateTime.UtcNow.Date;

        //    if (currentDate < decryptedDateWithOffset.Date)
        //    {
        //        try
        //        {
        //            var existingMediaLinks = await _db.TblMediaLinks.FirstOrDefaultAsync(x => x.Id == editExistingMediaLinkDto.Id);

        //            if (existingMediaLinks == null)
        //            {
        //                _response.IsSuccess = false;
        //                _response.Message = "Unable to find media link with this Id";
        //                return _response;
        //            }

        //            var existingMediaId = await _db.TblMedia.FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(editExistingMediaLinkDto.Mediaid));

        //            if (existingMediaId == null)
        //            {
        //                _response.IsSuccess = false;
        //                _response.Message = "Unable to find media with this Id";
        //                return _response;
        //            }

        //            var existingCompaignId = await _db.TblCampainHs.FirstOrDefaultAsync(x => x.No == editExistingMediaLinkDto.Campainno);

        //            if (existingCompaignId == null)
        //            {
        //                _response.IsSuccess = false;
        //                _response.Message = "Unable to find campaign with this Id";
        //                return _response;
        //            }

        //            existingMediaLinks.Mediaid = editExistingMediaLinkDto.Mediaid;
        //            existingMediaLinks.Campainno = editExistingMediaLinkDto.Campainno;

        //            await _db.SaveChangesAsync();

        //            _response.IsSuccess = true;
        //            _response.Message = "Successfully edited media link";
        //            _response.Result = existingMediaLinks;
        //            return _response;
        //        }
        //        catch (Exception ex)
        //        {
        //            _response.Message = "Error while getting media links! " + ex.Message;
        //            _response.IsSuccess = false;
        //            return _response;
        //        }
        //    }
        //    else
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = "Invalid Hash";
        //        return _response;
        //    }
        //}

        //[HttpPost("GetSingleMediaLink")]
        //public async Task<ResponseDto> GetSingleMediaLinks([FromQuery] int id, [FromBody] AuthDto authDto)
        //{
        //    if (authDto.Hash == null)
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = "Please provide hash";
        //        return _response;
        //    }

        //    HelperAuth decodedValues = AuthValidator.DecodeValue(authDto.Hash);

        //    var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == authDto.Hash);

        //    if (_user == null)
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = "Invalid Hash";
        //        return _response;
        //    }

        //    var decryptedDateWithOffset = decodedValues.Date.AddDays(1);
        //    var currentDate = DateTime.UtcNow.Date;

        //    if (currentDate < decryptedDateWithOffset.Date)
        //    {
        //        try
        //        {
        //            var existingMediaLinks = await _db.TblMediaLinks.FirstOrDefaultAsync(x => x.Id == id);

        //            if (existingMediaLinks == null)
        //            {
        //                _response.IsSuccess = false;
        //                _response.Message = "Unable to find media link with this Id";
        //                return _response;
        //            }

        //            _response.IsSuccess = true;
        //            _response.Message = "";
        //            _response.Result = existingMediaLinks;
        //            return _response;
        //        }
        //        catch (Exception ex)
        //        {
        //            _response.Message = "Error while getting media link! " + ex.Message;
        //            _response.IsSuccess = false;
        //            return _response;
        //        }
        //    }
        //    else
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = "Invalid Hash";
        //        return _response;
        //    }
        //}
    }
}
