using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using API.Models;
using API.Repos;
using API.Repos.Dtos;
using API.Repos.Dtos.LeadsDtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadLogController : ControllerBase
    {
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseDto _response;

        public LeadLogController(CRMContext context, IUnitOfWork unitOfWork)
        {
            _db = context;
            _unitOfWork = unitOfWork;
            _response = new ResponseDto();
        }
        
        public class GetLeadLogResponseDto
        {
            public AuthDto authDto { get; set; }
            public string LeadNo { get; set; }
        }

        public class InsertLeadLogDto
        {
            public AuthDto authDto { get; set; }
            public string LeadNo { get; set; }
            public string Log { get; set; }
        }

        [HttpPost("Get")]
        public async Task<ResponseDto> GetLeadLogResponse(GetLeadLogResponseDto getLeadLogResponseDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(getLeadLogResponseDto.authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }


            try
            {
                var existingLeadLog = await _db.Tblleadlogviews.Where(x => x.Leadid == getLeadLogResponseDto.LeadNo).OrderByDescending(x => x.Addon).ToListAsync();

                _response.Result = existingLeadLog;
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting lead log data. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }


        [HttpPost("GetLeadLogs")]
        public async Task<ResponseDto> GetLeadLog(GetLeadLogResponseDto getLeadLogResponseDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(getLeadLogResponseDto.authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var existingLeadLog = await _db.Tblleadlogviews
                    .Where(x => x.Leadid == getLeadLogResponseDto.LeadNo)
                    .OrderByDescending(x => x.Addon)
                    .Select(x => x.Log)
                    .ToListAsync();

                string[] logArray = existingLeadLog.ToArray();

                _response.Result = logArray;
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting lead log data. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }


        [HttpPost("insert")]
        public async Task<ResponseDto> Insert(InsertLeadLogDto leadlogDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(leadlogDto.authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                if (leadlogDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Missing User Data";
                    return _response;
                }

                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

                DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);

                var newLeadLog = new TblLeadlog
                {
                    Addby = authResponse.Result.Userid,
                    Addon = dubaiTime,
                    Leadid = leadlogDto.LeadNo,
                   //. Log = "Manual Log" +" " + leadlogDto.Log + " " + "Created at: " + " " + dubaiTime ,
                    Log = dubaiTime + " " + "\n: " + leadlogDto.Log
                };

                await _db.TblLeadlogs.AddAsync(newLeadLog);

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Successfully created " + leadlogDto.LeadNo;
                _response.Result = newLeadLog;
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
