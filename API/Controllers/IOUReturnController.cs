using API.Models;
using API.Repos.Dtos;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using API.Repos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Repos.Dtos.IOUDtos;
using API.Repos.Dtos.IOURtnDtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos;

namespace API.Controllers
{

    [Route("api/IOUrtn")]
    [ApiController]
    public class IOUReturnController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly GlobalDataService _globalDataService;

        public IOUReturnController(CRMContext db, IUnitOfWork unitOfWork, GlobalDataService globalDataService)
        {
            _response = new ResponseDto();
            _db = db;
            _unitOfWork = unitOfWork;
            _globalDataService = globalDataService;
        }

        [HttpGet("report")]
        public async Task<ActionResult<TblIourtn>> ReturnHtmlReport()
        {
            var tableForReport = await _db.TblIourtns.ToListAsync();

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
                                <th class='border' colspan=""6"" style=""text-align: center;"">IOU Return Data</th>
                            </tr>
                            <tr>
                                <th class='border'>Rtnid</th>
                                <th class='border'>Iouid</th>
                                <th class='border'>Retnon</th>
                                <th class='border'>Brid</th>
                                <th class='border'>Addby</th>
                                <th class='border'>Addon</th>
                            </tr>";


            foreach (var items in tableForReport)
            {

                string addon = Convert.ToDateTime(items.Addon).ToString("yyyy-MM-dd");
                string retnon = Convert.ToDateTime(items.Retnon).ToString("yyyy-MM-dd");

                html += $@"
                                <tr>
                                      <td class='border'>{items.Rtnid}</td>
                                      <td class='border'>{items.Iouid}</td>
                                      <td class='border'>{retnon}</td>
                                      <td class='border'>{items.Brid}</td>
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
        public async Task<ActionResult<TblvIourtn>> ReturnHtmlCellReport(int id)
        {
            var tableForReport = await _db.TblvIourtns
                .Where(x => x.Rtnid == id)
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
                string retnon = Convert.ToDateTime(items.Retnon).ToString("yyyy-MM-dd");

                html += $@"
                            <tr>
                                <th colspan=""2"" style=""text-align: center;"">IOU Return Data</th>
                            </tr>
                                   <tr>
                                        <td>Return No</td>
                                        <td>{items.Rtnid}</td>
                                   </tr>
                                   <tr>
                                        <td>IOU No</td>
                                        <td>{items.Iouid}</td>
                                   </tr>
                                   <tr>
                                        <td>Return On</td>
                                        <td>{items.Retnon}</td>
                                   </tr>
                                   <tr>
                                        <td>Branch</td>
                                        <td>{items.BranchName}</td>
                                   </tr>
                                   <tr>
                                        <td>Description</td>
                                        <td>{items.Desc}</td>
                                   </tr>
                                   <tr>
                                        <td>Add By</td>
                                        <td>{items.Username}</td>
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

     
        [HttpPost("insertIOUrtn")]
        public async Task<ResponseDto> Insert(IOUrtnDto iourtnDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(iourtnDto.AuthDto);

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
                Location = AccessLocation.IOUReturn.ToString()
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
                    if (iourtnDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingIOUrtn = await _db.TblIourtns.FirstOrDefaultAsync(x => x.Rtnid == iourtnDto.Rtnid);

                    if (existingIOUrtn != null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Invalid Id";
                        return _response;
                    }

                    var newUser = new TblIourtn
                    {
                        Iouid = iourtnDto.Iouid,
                        Retnon = iourtnDto.Retnon,
                        Brid = _globalDataService.BrId ?? 0,
                        Desc =iourtnDto.Desc,
                        Addby = authResponse.Result.Userid,
                        Addon = DateTime.UtcNow.Date,
                    };

                    await _db.TblIourtns.AddAsync(newUser);
                    await _db.SaveChangesAsync();


                    var existingIOU = await _db.TblIous.FirstOrDefaultAsync(x => x.Id == iourtnDto.Iouid);

                    if (existingIOU == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Not exist Id";
                        return _response;
                    }

                    existingIOU.Returned = 1;
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully inserting " + newUser.Rtnid;
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


        [HttpPost("updateIOUrtn")]
        public async Task<ResponseDto> UpdateIOUrtn(IOUrtnDto iourtnDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(iourtnDto.AuthDto);

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
                Location = AccessLocation.IOUReturn.ToString()
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
                    if (iourtnDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingIOUrtn = await _db.TblIourtns.FirstOrDefaultAsync(x => x.Rtnid == iourtnDto.Rtnid);

                    if (existingIOUrtn == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Id not found";
                        return _response;
                    }

                    existingIOUrtn.Iouid = iourtnDto.Iouid;
                    existingIOUrtn.Retnon = iourtnDto.Retnon;
                    existingIOUrtn.Brid = _globalDataService.BrId ?? 0;
                    existingIOUrtn.Desc = iourtnDto.Desc;

                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully updated IOUrtn: " + existingIOUrtn.Rtnid;
                    _response.Result = existingIOUrtn;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while updating IOUrtn! " + ex.Message;
                    _response.IsSuccess = false;
                    return _response;
                }
        }

        [HttpPost("getIOUrtn")]
        public async Task<ResponseDto> GetAllIOUrtn(AuthDto authDto)
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
                Location = AccessLocation.IOUReturn.ToString()
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
                var IOUrtn = await _unitOfWork.iIOUrtnInterface.GetAllVIOUrtn();
                _response.IsSuccess = true;
                _response.Message = "";
                _response.Result = IOUrtn;
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
