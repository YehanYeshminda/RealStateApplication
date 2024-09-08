using API.Models;
using API.Repos.Dtos.CommonDto;
using API.Repos.Dtos.StaffDtos;
using API.Repos.Dtos;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using API.Repos.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Repos.Dtos.AgreementRemiderDtos;
using API.Repos.Dtos.AdvPaymentDtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgreementReminderController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly GlobalDataService _globalDataService;
        private readonly IUnitOfWork _unitOfWork;

        public AgreementReminderController(CRMContext db, IWebHostEnvironment webHostEnvironment, GlobalDataService globalDataService, IUnitOfWork unitOfWork)
        {
            _response = new ResponseDto();
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _globalDataService = globalDataService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("report")]
        public async Task<ActionResult<TblvAgreementReminder>> ReturnHtmlReport()
        {
            var tableForReport = await _db.TblvAgreementReminders.ToListAsync();

            string html = @"
            <html>
            <head>
                <title>Paramount Real Estate</title>
               <style>
                    body { font-family: Tahoma; }
                    .header { display: flex; align-items: center; justify-content: center; padding: 20px; background-color: #f5f5f5; }
                    .logo { width: 100px; height: 100px; }
                    .company-info { display: flex; flex-direction: column; margin-left: 10px; text-align: center; }
                    .company-name { font-size: 24px; font-weight: bold; margin-bottom: 10px; }
                    .address { font-size: 16px; margin-bottom: 10px; }
                    .telephone { font-size: 16px; }
                    .containerheader { text-align: center; }
                    table { width: 100%; border-collapse: collapse; }
                    td, th { padding: 8px; text-align: left; }
                    th { background-color: #f2f2f2; }
                    .auto-style5, .auto-style6, .auto-style4 { width: 150px; }
                    .border { border: 1px solid #ccc; }  
                </style>
            </head>
            <body>
                <div class='header' style='text-align:left;'>

                    <div class='company-info'>
                        <div class='company-name'>Paramount Real Estate</div>
                        <div class='address'>Near North Villa</div>
                        <div class='telephone'>1234567890</div>
                    </div>
                </div>

                 <div class='report-container'>
                        <table>
                            <tr>
                                <th class='border' colspan=""10"" style=""text-align: center;"">Agreement Reminders</th>
                            </tr>
                            <tr>
                                <th class='border'>Id</th>
                                <th class='border'>Date</th>
                                <th class='border'>Customer</th>
                                <th class='border'>Agreement Type</th>
                                <th class='border'>Enddate</th>
                                <th class='border'>Remindon</th>
                                <th class='border'>Remarks</th>
                                <th class='border'>Status</th>
                                <th class='border'>Add by</th>
                                <th class='border'>Add on</th>
                            </tr>";


            foreach (var items in tableForReport)
            {

                string date = Convert.ToDateTime(items.Date).ToString("yyyy-MM-dd");
                string enddate = Convert.ToDateTime(items.Enddate).ToString("yyyy-MM-dd");
                string remindon = Convert.ToDateTime(items.Remindon).ToString("yyyy-MM-dd");
                string addon = Convert.ToDateTime(items.Addon).ToString("yyyy-MM-dd");

                html += $@"
                                <tr>
                                    <td class='border'>{items.Id}</td>
                                    <td class='border'>{date}</td>
                                    <td class='border'>{items.CustName}</td>
                                    <td class='border'>{items.TypeName}</td>
                                    <td class='border'>{enddate}</td>
                                    <td class='border'>{remindon}</td>
                                    <td class='border'>{items.Remarks}</td>
                                    <td class='border'>{items.Status}</td>
                                    <td class='border'>{items.Username}</td>
                                    <td class='border'>{addon}</td>
                                </tr>
                                ";
            }

            html += $@"

                </table>
                </div>
                </body>
                </html>";

            var response = new Dtos.HtmlResponseDto { Content = html };

            return Ok(response);
        }

        [HttpGet("cellreport/{id}")]
        public async Task<ActionResult<TblvAgreementReminder>> ReturnHtmlCellReport(int id)
        {
            var tableForReport = await _db.TblvAgreementReminders
                .Where(x => x.Id == id)
                    .ToListAsync();

            string html = @"
                <html>
                    <head>
                        <title>Paramount Real Estate</title>
                        <style>
                            body { font-family: Tahoma; }
                            .header { display: flex; align-items: center; justify-content: center; padding: 20px; background-color: #f5f5f5; }
                            .logo { width: 100px; height: 100px; }
                            .company-info { display: flex; flex-direction: column; margin-left: 10px; text-align: center; }
                            .company-name { font-size: 24px; font-weight: bold; margin-bottom: 10px; }
                            .address { font-size: 16px; margin-bottom: 10px; }
                            .telephone { font-size: 16px; }
                            .containerheader { text-align: center; }
                            table { border-collapse: collapse; margin-left: 90px; } 
                            td, th { padding: 8px; text-align: left; border: 1px solid #ccc;}
                            th { background-color: #f2f2f2; }
      	                    td {width : 300px;}
                            .auto-style5, .auto-style6, .auto-style4 { width: 150px; }
                        </style>
                    </head>
                    <body>
                        <div class='header' style='text-align:left;'>
                            <div class='company-info'>
                                <div class='company-name'>Paramount Real Estate</div>
                                <div class='address'>Near North Villa</div>
                                <div class='telephone'>1234567890</div>
                            </div>
                        </div>
                        <div class='report-container'>
                            <table>";

            foreach (var items in tableForReport)
            {
                string date = Convert.ToDateTime(items.Date).ToString("yyyy-MM-dd");
                string enddate = Convert.ToDateTime(items.Enddate).ToString("yyyy-MM-dd");
                string remindon = Convert.ToDateTime(items.Remindon).ToString("yyyy-MM-dd");
                string addon = Convert.ToDateTime(items.Addon).ToString("yyyy-MM-dd");

                html += $@"
                            <tr>
                                <th colspan=""2"" style=""text-align: center;"">Agreement Reminders</th>
                            </tr>
                                   <tr>
                                        <td>No</td>
                                        <td>{items.Id}</td>
                                   </tr>
                                   <tr>
                                        <td>Date</td>
                                        <td>{date}</td>
                                   </tr>
                                   <tr>
                                        <td>Custcode</td>
                                        <td>{items.CustName}</td>
                                   </tr>
                                   <tr>
                                        <td>Agreementtype</td>
                                        <td>{items.TypeName}</td>
                                   </tr>
                                   <tr>
                                        <td>Enddate</td>
                                        <td>{enddate}</td>
                                   </tr>
                                   <tr>
                                        <td>Remindon</td>
                                        <td>{remindon}</td>
                                   </tr>
                                   <tr>
                                        <td>Remarks</td>
                                        <td>{items.Remarks}</td>
                                   </tr>
                                   <tr>
                                        <td>Add by</td>
                                        <td>{items.Username}</td>
                                   </tr>
                                   <tr>
                                        <td>Add on</td>
                                        <td>{addon}</td>
                                   </tr>
                                ";
                }

                html += @"
                        </table>
                     </div>
                  </body>
                </html>";

            var response = new Dtos.HtmlResponseDto { Content = html };

            return Ok(response);
        }

        [HttpPost("insertagreementreminder")]
        public async Task<ResponseDto> Insert(AgreementRemindersDto agreementRemindersDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(agreementRemindersDto.AuthDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            var newUserPermission = new SendGetUserPermission
            {
                Event = Event.Add.ToString(),
                Location = AccessLocation.AgreementReminders.ToString()
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
                    if (agreementRemindersDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingreminder = await _db.TblAgreementReminders.FirstOrDefaultAsync(x => x.Id == agreementRemindersDto.Id);
                     
                    if (existingreminder != null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Invalid Id";
                        return _response;
                    }
                    
                    var newUser = new TblAgreementReminder
                    {
                        Date = agreementRemindersDto.Date,
                        Custcode = agreementRemindersDto.Custcode,
                        Agreementtype = agreementRemindersDto.Agreementtype,
                        Enddate = agreementRemindersDto.Enddate,
                        Remindon = agreementRemindersDto.Remindon,
                        Remarks = agreementRemindersDto.Remarks,
                        Status = agreementRemindersDto.Status,
                        Addby = authResponse.Result.Userid,
                        Addon = DateTime.UtcNow.Date,
                    };

                    await _db.TblAgreementReminders.AddAsync(newUser);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully created " + newUser.Agreementtype;
                    _response.Result = newUser;
                    return _response;

                }
                catch (Exception ex)
                {
                    _response.Message = "Error while inserting! " + ex.Message;
                    _response.IsSuccess = false;
                    return _response;
                }
        }


        [HttpPost("updateagreementreminder")]
        public async Task<ResponseDto> Update(AgreementRemindersDto agreementRemindersDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(agreementRemindersDto.AuthDto);
    
            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            var newUserPermission = new SendGetUserPermission
            {
                Event = Event.Edit.ToString(),
                Location = AccessLocation.AgreementReminders.ToString()
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
                    if (agreementRemindersDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingreminder = await _db.TblAgreementReminders.FirstOrDefaultAsync(x => x.Id == agreementRemindersDto.Id);

                    if (existingreminder == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Id not found";
                        return _response;
                    }
                    existingreminder.Date = agreementRemindersDto.Date;
                    existingreminder.Custcode = agreementRemindersDto.Custcode;
                    existingreminder.Agreementtype = agreementRemindersDto.Agreementtype;
                    existingreminder.Enddate = agreementRemindersDto.Enddate;
                    existingreminder.Remindon = agreementRemindersDto.Remindon;
                    existingreminder.Remarks = agreementRemindersDto.Remarks;
                    existingreminder.Status = agreementRemindersDto.Status;

                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully updated staff: " + existingreminder.Id;
                    _response.Result = existingreminder;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while updating staff! " + ex.Message;
                    _response.IsSuccess = false;
                    return _response;
                }
            
        }

        [HttpPost("getagreementreminder")]
        public async Task<ResponseDto> GetReminder(AuthDto authDto)
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
                Location = AccessLocation.AgreementReminders.ToString()
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
                var Meeting = await _unitOfWork.agreementReminderInterface.GetAllAgreementReminderView();
                _response.IsSuccess = true;
                _response.Message = "";
                _response.Result = Meeting;
                return _response;
            }

            catch (Exception ex)
            {
                _response.Message = "Error while Getting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }
    }
}