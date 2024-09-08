using API.Models;
using API.Repos.Dtos;
using API.Repos.Dtos.BankDtos;
using API.Repos.Dtos.CommonDto;
using API.Repos.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/bank")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly CRMContext _db;
        private readonly ResponseDto _response;

        public BankController(CRMContext context)
        {
            _db = context;
            _response = new ResponseDto();
        }

        [HttpPost("GetBankNameId")]
        public async Task<ActionResult<List<CommonDto>>> GetBankIds(AuthDto authDto)
        {
            if (authDto.Hash == null)
            {
                return BadRequest("Please provide hash");
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(authDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == authDto.Hash);

            if (_user == null)
            {
                return BadRequest("Invalid Hash");
            }

            var decryptedDateWithOffset = decodedValues.Date.AddDays(1);
            var currentDate = DateTime.UtcNow.Date;

            if (currentDate < decryptedDateWithOffset.Date)
            {
                try
                {
                    var newList = new List<BankInfoDto>();
                    var existingBanks = await _db.Tblbanks.ToListAsync();

                    foreach (var item in existingBanks)
                    {
                        var newItemToAdd = new BankInfoDto
                        {
                            textValue = item.BankName,
                            value = item.BankId
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
            else
            {
                return BadRequest("Invalid Hash");
            }
        }

        [HttpPost("GetAllBanks")]
        public async Task<ResponseDto> GetAllBanksAsync(AuthDto authDto) 
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
                    var existingBanks = await _db.Tblbanks.ToListAsync();

                    _response.IsSuccess = true;
                    _response.Message = "";
                    _response.Result = existingBanks;

                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while getting company! " + ex.Message;
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

        [HttpPost("AddBank")]
        public async Task<ResponseDto> AddNewBank(CreateNewBankDto createNewBankDto)
        {
            if (createNewBankDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(createNewBankDto.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == createNewBankDto.AuthDto.Hash);

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
                    var existingBank = await _db.Tblbanks.FirstOrDefaultAsync(x => x.BankName == createNewBankDto.BankName);

                    if (existingBank != null)
                    {
                        _response.Message = "Bank with this name already exists";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    var newBank = new Tblbank
                    {
                        BankName = createNewBankDto.BankName,
                        Status = createNewBankDto.Status
                    };

                    await _db.Tblbanks.AddAsync(newBank);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Result = newBank;
                    _response.Message = "Successfully created new bank";
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while creating new bank! " + ex.Message;
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

        [HttpPut("EditBank")]
        public async Task<ResponseDto> EditExistingBank(EditExistingBankDto editExistingBankDto)
        {
            if (editExistingBankDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(editExistingBankDto.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == editExistingBankDto.AuthDto.Hash);

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
                    var existingBankWithName = await _db.Tblbanks.FirstOrDefaultAsync(x => x.BankName == editExistingBankDto.BankName);

                    if (existingBankWithName != null)
                    {
                        _response.Message = "Bank with this name already exists";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    var existingBank = await _db.Tblbanks.FirstOrDefaultAsync(x => x.BankId == editExistingBankDto.BankId);

                    if (existingBank != null)
                    {
                        _response.Message = "Bank with this Id does not exist";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    existingBank.BankName = editExistingBankDto.BankName;
                    existingBank.Status = editExistingBankDto.Status;

                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Result = existingBank;
                    _response.Message = "Successfully edited bank";
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while editing new bank! " + ex.Message;
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

        [HttpPost("GetSingleBank")]
        public async Task<ResponseDto> GetSingleBank([FromQuery] string id, [FromBody] AuthDto authDto)
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
                    var existingBankWithId = await _db.Tblbanks.FirstOrDefaultAsync(x => x.BankId == id);

                    if (existingBankWithId != null)
                    {
                        _response.Message = "Bank with this id does exist";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    _response.IsSuccess = true;
                    _response.Result = existingBankWithId;
                    _response.Message = "";
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while editing new bank! " + ex.Message;
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
