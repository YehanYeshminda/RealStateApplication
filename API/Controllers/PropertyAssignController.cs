using API.Models;
using API.Repos.Dtos.PropertyDevDtos;
using API.Repos.Dtos;
using API.Repos.Interfaces;
using API.Repos.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Repos.Dtos.PropAssignDtos;
using API.Repos.Dtos.CommonDto;
using API.Repos.Helpers;
using Microsoft.Data.SqlClient;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyAssignController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly GlobalDataService _globalDataService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public PropertyAssignController(IConfiguration configuration, CRMContext db, IUnitOfWork unitOfWork, GlobalDataService globalDataService, IWebHostEnvironment webHostEnvironment)
        {
            _response = new ResponseDto();
            _db = db;
            _unitOfWork = unitOfWork;
            _globalDataService = globalDataService;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        [HttpGet("report")]
        public async Task<ActionResult<TblvPropAssign>> ReturnHtmlReport()
        {
            var tableForReport = await _db.TblvPropAssigns.ToListAsync();

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
                                <th class='border' colspan=""7"" style=""text-align: center;"">Property Assign Data</th>
                            </tr>
                            <tr>
                                <th class='border'> Id </th>
                                <th class='border'> Date </th>
                                <th class='border'> Salesperson </th>
                                <th class='border'> Customer </th>
                                <th class='border'> Validtill </th>
                                <th class='border'> Advnotno </th>
                                <th class='border'> Description </th>
                            </tr>";


            foreach (var items in tableForReport)
            {
                string date = Convert.ToDateTime(items.Date).ToString("yyyy-MM-dd");
                string validtill = Convert.ToDateTime(items.Validtill).ToString("yyyy-MM-dd");

                html += $@"
                   <tr>
                        <td class='border'>{items.Id}</td>
                        <td class='border'>{date}</td>
                        <td class='border'>{items.TypeName}</td>
                        <td class='border'>{items.CustName}</td>
                        <td class='border'>{validtill}</td>
                        <td class='border'>{items.Advnotno}</td>
                        <td class='border'>{items.Description}</td>
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
        public async Task<ActionResult<TblvPropAssign>> ReturnHtmlCellReport(int id)
        {
            var tableForReport = await _db.TblvPropAssigns
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
                string validtill = Convert.ToDateTime(items.Validtill).ToString("yyyy-MM-dd");

                html += $@"
                                   <tr>
                                       <th colspan=""2"" style=""text-align: center;"">Property Assign Data</th>
                                   </tr>
                                   <tr>
                                        <td>Id</td>
                                        <td>{items.Id}</td>
                                   </tr>
                                   <tr>
                                        <td>Date</td>
                                        <td>{date}</td>
                                   </tr>
                                   <tr>
                                        <td>Sales Person</td>
                                        <td>{items.TypeName}</td>
                                   </tr>
                                   <tr>
                                        <td>Customer</td>
                                        <td>{items.CustName}</td>
                                   </tr>
                                   <tr>
                                        <td>Valid Till</td>
                                        <td>{validtill}</td>
                                   </tr>
                                   <tr>
                                        <td>Advance Note No</td>
                                        <td>{items.Advnotno}</td>
                                   </tr>
                                   <tr>
                                        <td>Description</td>
                                        <td>{items.Description}</td>
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


        [HttpPost("getpropassign")]
        public async Task<ResponseDto> Getpropassign(AuthDto authDto)
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
                Location = AccessLocation.PropertyAssign.ToString()
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
                var Meeting = await _unitOfWork.propAssignInterface.GetAllPropAssignView();
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

        [HttpPost("insertpropassign")]
        public async Task<ResponseDto> Insert(ProAssignDto propassignDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(propassignDto.AuthDto);

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
                Location = AccessLocation.PropertyAssign.ToString()
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
                if (propassignDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Missing User Data";
                    return _response;
                }

                var existingpropassign = await _db.Tblpropassigns.FirstOrDefaultAsync(x => x.Id == propassignDto.Id);

                if (existingpropassign != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid Id";
                    return _response;
                }

                var newUser = new Tblpropassign
                {
                    Date = propassignDto.Date,
                    Salesperson = propassignDto.Salesperson,
                    Customerid = propassignDto.Customerid,
                    Validtill = propassignDto.Validtill,
                    Advnotno = propassignDto.Advnotno,
                    Description = propassignDto.Description,
                    Addon = DateTime.UtcNow,
                    Addby = authResponse.Result.Userid,
                };



                await _db.Tblpropassigns.AddAsync(newUser);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Successfully created " + newUser.Id;
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

        [HttpPost("updatepropassign")]
        public async Task<ResponseDto> Updatepropassign(ProAssignDto propassignDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(propassignDto.AuthDto);

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
                Location = AccessLocation.PropertyAssign.ToString()
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
                if (propassignDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Missing User Data";
                    return _response;
                }

                var existingpropassign = await _db.Tblpropassigns.FirstOrDefaultAsync(x => x.Id == propassignDto.Id);

                if (existingpropassign == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Id not found";
                    return _response;
                }

                existingpropassign.Date = propassignDto.Date;
                existingpropassign.Salesperson = propassignDto.Salesperson;
                existingpropassign.Customerid = propassignDto.Customerid;
                existingpropassign.Validtill = propassignDto.Validtill;
                existingpropassign.Advnotno = propassignDto.Advnotno;
                existingpropassign.Description = propassignDto.Description;

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Successfully updated propassign: " + existingpropassign.Id;
                _response.Result = existingpropassign;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating propassign! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

    }
}
