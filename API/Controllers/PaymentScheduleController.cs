using API.Models;
using API.Repos.Dtos;
using API.Repos.Dtos.CampainHDtos;
using API.Repos.Dtos.LeadsDtos;
using API.Repos.Dtos.MediaDtos;
using API.Repos.Dtos.PaymentScheduleDtos;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentScheduleController : ControllerBase
    {
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseDto _response;

        public PaymentScheduleController(CRMContext context, IUnitOfWork unitOfWork)
        {
            _db = context;
            _unitOfWork = unitOfWork;
            _response = new ResponseDto();
        }

        [HttpPost("ComboVExpenseData")]
        public async Task<ResponseDto> GetComboVExpenseData(AuthDto authDto)
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
                var data = await _unitOfWork.paymentScheduleInterface.GetComboDataVExpenses();

                _response.Result = data;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting combo data! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetPaymentScheduleById/{id}")]
        public async Task<ResponseDto> GetPaymentScheduleById([FromRoute]string id, [FromBody]AuthDto authDto)
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
                var existingPaymentSchedule = await _db.TblPymentschedules.FirstOrDefaultAsync(x => x.PaymentScheduleNo == id);

                if (existingPaymentSchedule == null)
                {
                    _response.Message = "Unable to find payment schedule with this Id";
                    _response.IsSuccess = false;
                    return _response;
                }

                _response.IsSuccess = true;
                _response.Message = "";
                _response.Result = existingPaymentSchedule;

                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting media! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetAllPaymentSchedule")]
        public async Task<ResponseDto> GetAllPaymentSchedule(AuthDto authDto)
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
                var existingMediaLinks = await _unitOfWork.paymentScheduleInterface.GetAllViewPaymentScheduleAsync();

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

        [HttpPost("AddPaymentSchedule")]
        public async Task<ResponseDto> AddNewPaymentSchedules(CreatePaymentScheduleDto createPaymentScheduleDto)
        {
            if (createPaymentScheduleDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(createPaymentScheduleDto.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == createPaymentScheduleDto.AuthDto.Hash);

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
                    var existingVendor = await _db.TblSuppliers.FirstOrDefaultAsync(x => x.SupplierId == createPaymentScheduleDto.Venderid);

                    if (existingVendor == null)
                    {
                        _response.Message = "vendor with this id does not exist";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    var newItem = new TblPymentschedule
                    {
                        PaymentScheduleNo = createPaymentScheduleDto.Id,
                        Paidon = createPaymentScheduleDto.Paidon,
                        Reason = createPaymentScheduleDto.Reason,
                        Amount = createPaymentScheduleDto.Amount,
                        Date = createPaymentScheduleDto.Date,
                        Renewevery = createPaymentScheduleDto.Renewevery,
                        Renewstatus = createPaymentScheduleDto.Renewstatus,
                        Rxpaccount = createPaymentScheduleDto.Rxpaccount,
                        Status = createPaymentScheduleDto.Status,
                        Venderid = existingVendor.SupplierId
                    };


                    await _db.TblPymentschedules.AddAsync(newItem);

                    var existingControl = await _db.Tblcontrols.FirstOrDefaultAsync();

                    string numericPart = StaticHelpers.NormalizeLeadNo(Regex.Match(createPaymentScheduleDto.Id, @"\d+").Value);

                    existingControl.PaymentScheduleNo = Convert.ToInt32(numericPart);
                    _db.Tblcontrols.Update(existingControl);

                    await _db.SaveChangesAsync();

                    _response.Message = "Payment schedule has been added successfully";
                    _response.IsSuccess = true;
                    _response.Result = newItem;
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

        [HttpPut("EditPaymentSchedule")]
        public async Task<ResponseDto> EditExistingPaymentSchedules(CreatePaymentScheduleDto createPaymentScheduleDto)
        {
            if (createPaymentScheduleDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(createPaymentScheduleDto.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == createPaymentScheduleDto.AuthDto.Hash);

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

                    var existingPaymentSchedule = await _db.TblPymentschedules.FirstOrDefaultAsync(x => x.PaymentScheduleNo == createPaymentScheduleDto.Id);

                    if (existingPaymentSchedule == null)
                    {
                        _response.Message = "payment schedule with this id does not exist";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    var existingVendor = await _db.TblSuppliers.FirstOrDefaultAsync(x => x.SupplierId == createPaymentScheduleDto.Venderid);

                    if (existingVendor == null)
                    {
                        _response.Message = "vendor with this id does not exist";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    existingPaymentSchedule.Date = createPaymentScheduleDto.Date;
                    existingPaymentSchedule.Venderid = existingVendor.SupplierId;
                    existingPaymentSchedule.Reason = createPaymentScheduleDto.Reason;
                    existingPaymentSchedule.Rxpaccount = createPaymentScheduleDto.Rxpaccount;
                    existingPaymentSchedule.Amount = createPaymentScheduleDto.Amount;
                    existingPaymentSchedule.Paidon = createPaymentScheduleDto.Paidon;
                    existingPaymentSchedule.Renewevery = createPaymentScheduleDto.Renewevery;
                    existingPaymentSchedule.Renewstatus = createPaymentScheduleDto.Renewstatus;
                    existingPaymentSchedule.Status = createPaymentScheduleDto.Status;

                    await _db.SaveChangesAsync();

                    _response.Message = "Payment schedule has been edited successfully";
                    _response.IsSuccess = true;
                    _response.Result = existingPaymentSchedule;
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
    }
}
