using API.Models;
using API.Repos.Dtos.LeadsDtos;
using API.Repos.Dtos;
using API.Repos.Helpers;
using API.Repos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Repos.Dtos.BankBranchDtos;
using System.Net;

namespace API.Controllers
{

    [Route("api/auth")]
    [ApiController]
    public class BankBranchesController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly GlobalDataService _globalDataService;

        public BankBranchesController(CRMContext db, IWebHostEnvironment webHostEnvironment, GlobalDataService globalDataService)
        {
            _response = new ResponseDto();
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _globalDataService = globalDataService;
        }

        [HttpPost("insertBankBranches")]
        public async Task<ResponseDto> Insert(BankBranchDto bankBranchesDto)
        {
            if (bankBranchesDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(bankBranchesDto.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == bankBranchesDto.AuthDto.Hash);

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
                    if (bankBranchesDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingBranch = await _db.Tblbankbranches.FirstOrDefaultAsync(x => x.Brid == bankBranchesDto.Brid);

                    if (existingBranch != null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Invalid Id";
                        return _response;
                    }


                    var newItem = new Tblbankbranch
                    {
                        Brid = bankBranchesDto.Brid,
                        Bankid = bankBranchesDto.Bankid,
                        BranchName = bankBranchesDto.BranchName,
                        Address = bankBranchesDto.Address,
                        Status = bankBranchesDto.Status,
                        Tel = bankBranchesDto.Tel,
                        ContactPerson = bankBranchesDto.ContactPerson,

                    };

                    await _db.Tblbankbranches.AddAsync(newItem);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully created " + newItem.Brid;
                    _response.Result = newItem;
                    return _response;

                }
                catch (Exception ex)
                {
                    _response.Message = "Error while inserting! " + ex.Message;
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


        [HttpPost("updateBankBranches")]
        public async Task<ResponseDto> UpdateLead(BankBranchDto bankBranchesDto)
        {
            if (bankBranchesDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(bankBranchesDto.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == bankBranchesDto.AuthDto.Hash);

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
                    if (bankBranchesDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingBranch = await _db.Tblbankbranches.FirstOrDefaultAsync(x => x.Brid == bankBranchesDto.Brid);

                    if (existingBranch == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Id not found";
                        return _response;
                    }

                    existingBranch.Brid = bankBranchesDto.Brid;
                    existingBranch.Bankid = bankBranchesDto.Bankid;
                    existingBranch.BranchName = bankBranchesDto.BranchName;
                    existingBranch.Address = bankBranchesDto.Address;
                    existingBranch.Status = bankBranchesDto.Status;
                    existingBranch.Tel = bankBranchesDto.Tel;
                    existingBranch.ContactPerson = bankBranchesDto.ContactPerson;

                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully updated BankBranches: " + bankBranchesDto.Brid;
                    _response.Result = bankBranchesDto;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while updating BankBranches! " + ex.Message;
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

        [HttpGet("getBankBranches")]
        public async Task<ActionResult<List<Tblbankbranch>>> GetLeads(string hash)
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

                    List<Tblbankbranch> leads = await _db.Tblbankbranches.ToListAsync();

                    return Ok(leads);
                }
                catch (Exception ex)
                {
                    return BadRequest("Error while retrieving BankBranches: " + ex.Message);
                }
            }
            else
            {
                return Unauthorized("Invalid Hash");
            }
        }

    }
}
