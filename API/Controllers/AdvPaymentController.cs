using API.Models;
using API.Repos;
using API.Repos.Dtos;
using API.Repos.Dtos.AdvPaymentDtos;
using API.Repos.Dtos.LeadsDtos;
using API.Repos.Dtos.PropAssignDtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos.Interfaces;
using API.Repos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class AdvPaymentController : ControllerBase
        {
            private readonly ResponseDto _response;
            private readonly CRMContext _db;
            private readonly IUnitOfWork _unitOfWork;
            private readonly GlobalDataService _globalDataService;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly IConfiguration _configuration;

            public AdvPaymentController(IConfiguration configuration, CRMContext db, IUnitOfWork unitOfWork, GlobalDataService globalDataService, IWebHostEnvironment webHostEnvironment)
            {
                _response = new ResponseDto();
                _db = db;
                _unitOfWork = unitOfWork;
                _globalDataService = globalDataService;
                _webHostEnvironment = webHostEnvironment;
                _configuration = configuration;
            }

        [HttpGet("report")]
        public async Task<ActionResult<TblvAdvPayment>> ReturnHtmlReport()
        {
            var tableForReport = await _db.TblvAdvPayments.ToListAsync();

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
                                <th class='border' colspan=""12"" style=""text-align: center;"">Advance Payment</th>
                            </tr>
                            <tr>
                                <th class='border'>Id</th>
                                <th class='border'>Date</th>
                                <th class='border'>Salesby</th>
                                <th class='border'>Customer</th>
                                <th class='border'>Address</th>
                                <th class='border'>Chequepaid</th>
                                <th class='border'>Chequeno</th>
                                <th class='border'>Cashpaid</th>
                                <th class='border'>Cardpaid</th>
                                <th class='border'>Cardbank</th>
                                <th class='border'>Paymentfor</th>
                                <th class='border'>Description</th>
                            </tr>";


            foreach (var items in tableForReport)
            {
                string date = Convert.ToDateTime(items.Date).ToString("yyyy-MM-dd");

                html += $@"
                   <tr>
                        <td class='border'>{items.Id}</td>
                        <td class='border'>{date}</td>
                        <td class='border'>{items.TypeName}</td>
                        <td class='border'>{items.CustName}</td>
                        <td class='border'>{items.Address}</td>
                        <td class='border'>{items.Chequepaid}</td>
                        <td class='border'>{items.Chequeno}</td>
                        <td class='border'>{items.Cashpaid}</td>
                        <td class='border'>{items.Cardpaid}</td>
                        <td class='border'>{items.BankCode}</td>
                        <td class='border'>{items.Propertname}</td>
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
        public async Task<ActionResult<TblvAdvPayment>> ReturnHtmlCellReport(int id)
        {
            var tableForReport = await _db.TblvAdvPayments
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

                                html += $@"
                                    <tr>
                                        <th colspan=""2"" style=""text-align: center;"">Advance Payment</th>
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
                                        <td>Salesby</td>
                                        <td>{items.TypeName}</td>
                                   </tr>
                                   <tr>
                                        <td>Customer</td>
                                        <td>{items.CustName}</td>
                                   </tr>
                                   <tr>
                                        <td>Address</td>
                                        <td>{items.Address}</td>
                                   </tr>
                                   <tr>
                                        <td>Chequepaid</td>
                                        <td>{items.Chequepaid}</td>
                                   </tr>
                                   <tr>
                                        <td>Chequeno</td>
                                        <td>{items.Chequeno}</td>
                                   </tr>
                                   <tr>
                                        <td>Cashpaid</td>
                                        <td>{items.Cashpaid}</td>
                                   </tr>
                                   <tr>
                                        <td>Cardpaid</td>
                                        <td>{items.Cardpaid}</td>
                                   </tr>
                                   <tr>
                                        <td>Cardbank</td>
                                        <td>{items.BankCode}</td>
                                   </tr>
                                   <tr>
                                        <td>Paymentfor</td>
                                        <td>{items.Propertname}</td>
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


        [HttpPost("getadvpayment")]
        public async Task<ResponseDto> Getadvpayment(AuthDto authDto)
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
                Location = AccessLocation.AdvancePayment.ToString()
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
                var Meeting = await _unitOfWork.advPaymentInterface.GetViewAllAdv();
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

        [HttpPost("insertadvpayment")]
        public async Task<ResponseDto> Insert(AdvPaymentDto advpaymentDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(advpaymentDto.AuthDto);

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
                Location = AccessLocation.AdvancePayment.ToString()
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
                if (advpaymentDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Missing User Data";
                    return _response;
                }

                var existingadvpayment = await _db.TblAdvPayments.FirstOrDefaultAsync(x => x.Id == advpaymentDto.Id);

                if (existingadvpayment != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid Id";
                    return _response;
                }

                var newUser = new TblAdvPayment
                {
                    Date = advpaymentDto.Date,
                    Salesby = advpaymentDto.Salesby,
                    Customer = advpaymentDto.Customer,
                    Address = advpaymentDto.Address,
                    Chequepaid = advpaymentDto.Chequepaid,
                    Chequeno = advpaymentDto.Chequeno,
                    Cashpaid = advpaymentDto.Cashpaid,
                    Cardpaid = advpaymentDto.Cardpaid,
                    Cardbank = advpaymentDto.Cardbank,
                    Paymentfor = advpaymentDto.Paymentfor,
                    Description = advpaymentDto.Description,
                    Addon = DateTime.UtcNow,
                    Addby = authResponse.Result.Userid,
                };



                await _db.TblAdvPayments.AddAsync(newUser);
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

        [HttpPost("updateadvpayment")]
        public async Task<ResponseDto> Updateadvpayment(AdvPaymentDto advpaymentDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(advpaymentDto.AuthDto);

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
                Location = AccessLocation.AdvancePayment.ToString()
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
                if (advpaymentDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Missing User Data";
                    return _response;
                }

                var existingadvpayment = await _db.TblAdvPayments.FirstOrDefaultAsync(x => x.Id == advpaymentDto.Id);

                if (existingadvpayment == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Id not found";
                    return _response;
                }

                existingadvpayment.Date = advpaymentDto.Date;
                existingadvpayment.Salesby = advpaymentDto.Salesby;
                existingadvpayment.Customer = advpaymentDto.Customer;
                existingadvpayment.Address = advpaymentDto.Address;
                existingadvpayment.Chequepaid = advpaymentDto.Chequepaid;
                existingadvpayment.Chequeno = advpaymentDto.Chequeno;
                existingadvpayment.Cashpaid = advpaymentDto.Cashpaid;
                existingadvpayment.Cardpaid = advpaymentDto.Cardpaid;
                existingadvpayment.Cardbank = advpaymentDto.Cardbank;
                existingadvpayment.Paymentfor = advpaymentDto.Paymentfor;
                existingadvpayment.Description = advpaymentDto.Description;

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Successfully updated advpayment: " + existingadvpayment.Id;
                _response.Result = existingadvpayment;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating advpayment! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }
    }
}
