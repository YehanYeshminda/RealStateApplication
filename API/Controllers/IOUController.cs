using API.Models;
using API.Repos.Dtos.CommonDto;
using API.Repos.Dtos.StaffDtos;
using API.Repos.Dtos;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using API.Repos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Repos.Dtos.IOUDtos;
using Newtonsoft.Json.Linq;
using System.Xml;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos;
using API.Repos.Dtos.AdvPaymentDtos;

namespace API.Controllers
{

    [Route("api/IOU")]
    [ApiController]
    public class IOUController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly GlobalDataService _globalDataService;

        public IOUController(CRMContext db, IUnitOfWork unitOfWork, GlobalDataService globalDataService)
        {
            _response = new ResponseDto();
            _db = db;
            _unitOfWork = unitOfWork;
            _globalDataService = globalDataService;
        }

        [HttpGet("report")]
        public async Task<ActionResult<TblIou>> ReturnHtmlReport()
        {
            var tableForReport = await _db.TblIous.ToListAsync();

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
                                <th class='border' colspan=""11"" style=""text-align: center;"">IOU Data</th>
                            </tr>
                            <tr>
                                <th class='border'>Id</th>
                                <th class='border'>Branchid</th>
                                <th class='border'>Date</th>
                                <th class='border'>Issueto</th>
                                <th class='border'>Reason</th>
                                <th class='border'>Returned</th>
                                <th class='border'>Returnon</th>
                                <th class='border'>Approvedby</th>
                                <th class='border'>Value</th>
                                <th class='border'>Addby</th>
                                <th class='border'>Addon</th>
                            </tr>";


            foreach (var items in tableForReport)
            {

                string addon = Convert.ToDateTime(items.Addon).ToString("yyyy-MM-dd");
                string returnon = Convert.ToDateTime(items.Returnon).ToString("yyyy-MM-dd");
                string date = Convert.ToDateTime(items.Date).ToString("yyyy-MM-dd");

                html += $@"
                                <tr>
                                      <td class='border'>{items.Id}</td>
                                      <td class='border'>{items.Branchid}</td>
                                      <td class='border'>{date}</td>
                                      <td class='border'>{items.Issueto}</td>
                                      <td class='border'>{items.Reason}</td>
                                      <td class='border'>{items.Returned}</td>
                                      <td class='border'>{returnon}</td>
                                      <td class='border'>{items.Approvedby}</td>
                                      <td class='border'>{items.Value}</td>
                                      <td class='border'>{items.Addby}</td>
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
        public async Task<ActionResult<TblvIou>> ReturnHtmlCellReport(int id)
        {
            var tableForReport = await _db.TblvIous
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
                string addon = Convert.ToDateTime(items.Addon).ToString("yyyy-MM-dd");
                string returnon = Convert.ToDateTime(items.Returnon).ToString("yyyy-MM-dd");
                string date = Convert.ToDateTime(items.Date).ToString("yyyy-MM-dd");

                html += $@"
                            <tr>
                                <th colspan=""2"" style=""text-align: center;"">IOU Data</th>
                            </tr>
                                   <tr>
                                        <td>No</td>
                                        <td>{items.Id}</td>
                                   </tr>
                                   <tr>
                                        <td>Branch</td>
                                        <td>{items.BranchName}</td>
                                   </tr>
                                   <tr>
                                        <td>Date</td>
                                        <td>{date}</td>
                                   </tr>
                                   <tr>
                                        <td>Issued To</td>
                                        <td>{items.TypeName}</td>
                                   </tr>
                                   <tr>
                                        <td>Reason</td>
                                        <td>{items.Reason}</td>
                                   </tr>
                                   <tr>
                                        <td>Returned</td>
                                        <td>{items.Returned}</td>
                                   </tr>
                                   <tr>
                                        <td>Returned On</td>
                                        <td>{returnon}</td>
                                   </tr>
                                   <tr>
                                        <td>Approvedby</td>
                                        <td>{items.Username}</td>
                                   </tr>
                                   <tr>
                                        <td>Value</td>
                                        <td>{items.Value}</td>
                                   </tr>
                                   <tr>
                                        <td>Add On</td>
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

        [HttpPost("insertIou")]
        public async Task<ResponseDto> Insert(IouDto iouDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(iouDto.AuthDto);

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
                Location = AccessLocation.IOU.ToString()
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
                    if (iouDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingIou = await _db.TblIous.FirstOrDefaultAsync(x => x.Id == iouDto.Id);

                    if (existingIou != null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Invalid Id";
                        return _response;
                    }

                    var newUser = new TblIou
                    {
                        Branchid = _globalDataService.BrId ?? 0,
                        Date = iouDto.Date,
                        Issueto = iouDto.Issueto,
                        Reason = iouDto.Reason,
                        Returnon = iouDto.Returnon,
                        Approvedby = iouDto.Approvedby,
                        Value = iouDto.Value,
                        Returned = iouDto.Returned,
                        Addby = authResponse.Result.Userid,
                        Addon = DateTime.UtcNow.Date,
                    };

                    await _db.TblIous.AddAsync(newUser);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully inserting " + newUser.Id;
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


        [HttpPost("updateIou")]
        public async Task<ResponseDto> UpdateIou(IouDto iouDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(iouDto.AuthDto);

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
                Location = AccessLocation.IOU.ToString()
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
                    if (iouDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingIou = await _db.TblIous.FirstOrDefaultAsync(x => x.Id == iouDto.Id);

                    if (existingIou == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Id not found";
                        return _response;
                    }

                    existingIou.Branchid = _globalDataService.BrId ?? 0;
                    existingIou.Date = iouDto.Date;
                    existingIou.Issueto = iouDto.Issueto;
                    existingIou.Reason = iouDto.Reason;
                    existingIou.Returnon = iouDto.Returnon;
                    existingIou.Approvedby = iouDto.Approvedby;
                    existingIou.Value = iouDto.Value;
                    existingIou.Returned = iouDto.Returned;

                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully updated Iou: " + existingIou.Id;
                    _response.Result = existingIou;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while updating Iou! " + ex.Message;
                    _response.IsSuccess = false;
                    return _response;
                }
        }

        [HttpPost("getIou")]
        public async Task<ResponseDto> GetAlliou(AuthDto authDto)
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
                Location = AccessLocation.IOU.ToString()
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
                var iou = await _unitOfWork.iOUInterface.GetAllViou();
                _response.IsSuccess = true;
                _response.Message = "";
                _response.Result = iou;
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
