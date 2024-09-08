using API.Models;
using API.Repos.Dtos;
using API.Repos.Helpers;
using API.Repos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Repos.Dtos.StaffDtos;
using API.Repos.Interfaces;
using API.Repos.Dtos.CustomerDtos;
using API.Repos.Dtos.AdvPaymentDtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos;

namespace API.Controllers
{

    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(CRMContext db, IWebHostEnvironment webHostEnvironment, GlobalDataService globalDataService, IUnitOfWork unitOfWork)
        {
            _response = new ResponseDto();
            _db = db;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("report")]
        public async Task<ActionResult<Tblcustomer>> ReturnHtmlReport()
        {
            var tableForReport = await _db.Tblcustomers.ToListAsync();

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
                                <th class='border' colspan=""11"" style=""text-align: center;"">Customer Data</th>
                            </tr>
                            <tr>
                                <th class='border'>Cust Id</th>
                                <th class='border'>Cust Name</th>
                                <th class='border'>Cust City</th>
                                <th class='border'>Cust Mobile</th>
                                <th class='border'>Email</th>
                                <th class='border'>Credit days</th>
                                <th class='border'>Credit Allow</th>
                                <th class='border'>Credit Limit</th>
                                <th class='border'>Credit Period</th>
                                <th class='border'>TotRetCheque</th>
                                <th class='border'>Contact Person</th>
                            </tr>";


            foreach (var items in tableForReport)
            {

                html += $@"
                                <tr>
                                      <td class='border'>{items.CustId}</td>
                                      <td class='border'>{items.CustName}</td>
                                      <td class='border'>{items.CustCity}</td>
                                      <td class='border'>{items.CustMobile}</td>
                                      <td class='border'>{items.Email}</td>
                                      <td class='border'>{items.CreditDays}</td>
                                      <td class='border'>{items.CreditAllow}</td>
                                      <td class='border'>{items.CreditLimit}</td>
                                      <td class='border'>{items.CreditPeriod}</td>
                                      <td class='border'>{items.TotRetCheque}</td>
                                      <td class='border'>{items.ContPerson}</td>
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
        public async Task<ActionResult<Tblcustomer>> ReturnHtmlCellReport(int id)
        {
            var tableForReport = await _db.Tblcustomers
                .Where(x => x.CustId == id)
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

                html += $@"
        
                            <tr>
                                <th colspan=""2"" style=""text-align: center;"">Customer Data</th>
                            </tr>
                                   <tr>
                                        <td>Customer No</td>
                                        <td>{items.CustId}</td>
                                   </tr>
                                   <tr>
                                        <td>Name</td>
                                        <td>{items.CustName}</td>
                                   </tr>
                                   <tr>
                                        <td>City</td>
                                        <td>{items.CustCity}</td>
                                   </tr>
                                   <tr>
                                        <td>Mobile</td>
                                        <td>{items.CustMobile}</td>
                                   </tr>
                                   <tr>
                                        <td>Email</td>
                                        <td>{items.Email}</td>
                                   </tr>
                                   <tr>
                                        <td>Credit Days</td>
                                        <td>{items.CreditDays}</td>
                                   </tr>
                                   <tr>
                                        <td>Credit Allow</td>
                                        <td>{items.CreditAllow}</td>
                                   </tr>
                                   <tr>
                                        <td>Credit Limit</td>
                                        <td>{items.CreditLimit}</td>
                                   </tr>
                                   <tr>
                                        <td>Credit Period</td>
                                        <td>{items.CreditPeriod}</td>
                                   </tr>
                                   <tr>
                                        <td>TotReqCheque</td>
                                        <td>{items.TotRetCheque}</td>
                                   </tr>
                                   <tr>
                                        <td>Contact Person</td>
                                        <td>{items.ContPerson}</td>
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


        [HttpPost("insertcustomer")]
        public async Task<ResponseDto> Insert(CustomerDto customerDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(customerDto.AuthDto);

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
                Location = AccessLocation.Customer.ToString()
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
                    if (customerDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingcustomer = await _db.Tblcustomers.FirstOrDefaultAsync(x => x.CustId == customerDto.CustId);

                    if (existingcustomer != null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Invalid Id";
                        return _response;
                    }

                    var newUser = new Tblcustomer
                    {
                        CustName = customerDto.CustName,
                        CustAddress = customerDto.CustAddress,
                        CustCity = customerDto.CustCity,
                        CustMobile = customerDto.CustMobile,
                        CustPhone = customerDto.CustPhone,
                        Email = customerDto.Email,
                        ContPerson = customerDto.ContPerson,
                        CreditAllow = customerDto.CreditAllow,
                        CreditLimit = customerDto.CreditLimit,
                        CreditDays = customerDto.CreditDays,
                        Status = customerDto.Status,
                        CreditPeriod = customerDto.CreditPeriod,
                        Remarks = customerDto.Remarks,
                        Points = customerDto.Points,
                        CardNo = customerDto.CardNo,
                        VatNo = customerDto.VatNo,
                        TotRetCheque = customerDto.TotRetCheque
                    };

                    await _db.Tblcustomers.AddAsync(newUser);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully created " + newUser.CustName;
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


        [HttpPost("updatecustomer")]
        public async Task<ResponseDto> Updatecustomer(CustomerDto customerDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(customerDto.AuthDto);

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
                Location = AccessLocation.Customer.ToString()
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
                    if (customerDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingcustomer = await _db.Tblcustomers.FirstOrDefaultAsync(x => x.CustId == customerDto.CustId);

                    if (existingcustomer == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Id not found";
                        return _response;
                    }

                    existingcustomer.CustName = customerDto.CustName;
                    existingcustomer.CustAddress = customerDto.CustAddress;
                    existingcustomer.CustCity = customerDto.CustCity;
                    existingcustomer.CustMobile = customerDto.CustMobile;
                    existingcustomer.CustPhone = customerDto.CustPhone;
                    existingcustomer.Email = customerDto.Email;
                    existingcustomer.ContPerson = customerDto.ContPerson;
                    existingcustomer.CreditAllow = customerDto.CreditAllow;
                    existingcustomer.CreditLimit = customerDto.CreditLimit;
                    existingcustomer.CreditDays = customerDto.CreditDays;
                    existingcustomer.Status = customerDto.Status;
                    existingcustomer.CreditPeriod = customerDto.CreditPeriod;
                    existingcustomer.Remarks = customerDto.Remarks;
                    existingcustomer.Points = customerDto.Points;
                    existingcustomer.CardNo = customerDto.CardNo;
                    existingcustomer.VatNo = customerDto.VatNo;
                    existingcustomer.TotRetCheque = customerDto.TotRetCheque;

                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully updated customer: " + existingcustomer.CustId;
                    _response.Result = existingcustomer;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while updating customer! " + ex.Message;
                    _response.IsSuccess = false;
                    return _response;
                }
        }

        [HttpPost("getcustomer")]
        public async Task<ResponseDto> GetAllcustomer(AuthDto authDto)
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
                Location = AccessLocation.Customer.ToString()
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
                var customer = await _unitOfWork.customerInterface.GetAllCustomerAsync();
                _response.IsSuccess = true;
                _response.Message = "";
                _response.Result = customer;
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

