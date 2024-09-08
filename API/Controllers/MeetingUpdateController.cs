using API.Models;
using API.Repos.Dtos;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Repos.Dtos.MeetSchedDtos;

namespace API.Controllers
{
    [Route("api/MeetingUpdate")]
    [ApiController]
    public class MeetingUpdateController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public MeetingUpdateController(CRMContext db, IUnitOfWork unitOfWork)
        {
            _response = new ResponseDto();
            _db = db;
            _unitOfWork = unitOfWork;
        }


        [HttpPost("updateMeeting")]
        public async Task<ResponseDto> UpdateMeeting(UpdateConclusionDto meetingDto)
        {
            if (meetingDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(meetingDto.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == meetingDto.AuthDto.Hash);

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
                    if (meetingDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingMeeting = await _db.TblMeetings.FirstOrDefaultAsync(x => x.Id == meetingDto.Id);

                    if (existingMeeting == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Id not found";
                        return _response;
                    }

                    existingMeeting.Name = meetingDto.Name;
                    existingMeeting.Conclusion = meetingDto.Conclusion;


                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully updated conclusion: " + existingMeeting.Id;
                    _response.Result = existingMeeting;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while updating Meeting! " + ex.Message;
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


        [HttpPost("rescheduleMeeting")]
        public async Task<ResponseDto> RescheduleMeeting(ReSchedDto remeetingDto)
        {
            if (remeetingDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(remeetingDto.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == remeetingDto.AuthDto.Hash);

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
                    if (remeetingDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingMeeting = await _db.TblMeetings.FirstOrDefaultAsync(x => x.Id == remeetingDto.Id);

                    if (existingMeeting == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Id not found";
                        return _response;
                    }

                    existingMeeting.Meetdate = remeetingDto.Meetdate;
                    existingMeeting.Meettime = remeetingDto.Meettime;
                    existingMeeting.Venue = remeetingDto.Venue;


                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully updated conclusion: " + existingMeeting.Id;
                    _response.Result = existingMeeting;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while updating Meeting! " + ex.Message;
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
