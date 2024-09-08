using API.Models;
using API.Repos.Dtos.PropertyRegiterDtos;
using API.Repos.Dtos;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using API.Repos.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Repos.Dtos.LeadsDtos;
using API.Repos.Dtos.PropertyDevDtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyDevelopmentController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public PropertyDevelopmentController(CRMContext db, IUnitOfWork unitOfWork)
        {
            _response = new ResponseDto();
            _db = db;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("report")]
        public async Task<ActionResult<Tblpropdev>> ReturnHtmlReport()
        {
            var tableForReport = await _db.Tblpropdevs.ToListAsync();

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
                                <th class='border' colspan=""13"" style=""text-align: center;"">Property Development Data</th>
                            </tr>
                            <tr>
                                <th class='border'>Id</th>
                                <th class='border'>Date</th>
                                <th class='border'>Property Name</th>
                                <th class='border'>Vender</th>
                                <th class='border'>Expense Account</th>
                                <th class='border'>Description</th>
                                <th class='border'>Amount</th>
                                <th class='border'>Cash Paid</th>
                                <th class='border'>Bank Transfer</th>
                                <th class='border'>Bank Id</th>
                                <th class='border'>Cheque Paid</th>
                                <th class='border'>Cheque Id</th>
                                <th class='border'>Approved By</th>
                            </tr>";


            foreach (var items in tableForReport)
            {
                string date = Convert.ToDateTime(items.Date).ToString("yyyy-MM-dd");

                html += $@"
                   <tr>
                        <td class='border'>{items.Id}</td>
                        <td class='border'>{date}</td>
                        <td class='border'>{items.Propname}</td>
                        <td class='border'>{items.Vender}</td>
                        <td class='border'>{items.Expenseaccount}</td>
                        <td class='border'>{items.Description}</td>
                        <td class='border'>{items.Amount}</td>
                        <td class='border'>{items.Cashpaid}</td>
                        <td class='border'>{items.Banktransfer}</td>
                        <td class='border'>{items.Bankid}</td>
                        <td class='border'>{items.Chequepaid}</td>
                        <td class='border'>{items.Chequeid}</td>
                        <td class='border'>{items.Approvedby}</td>
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
        public async Task<ActionResult<VPropertyDevelopmentList>> ReturnHtmlCellReport(string id)
        {
            var tableForReport = await _db.VPropertyDevelopmentLists
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
                                       <th colspan=""2"" style=""text-align: center;"">Property Development Data</th>
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
                                        <td>Property Name</td>
                                        <td>{items.PropertyName}</td>
                                   </tr>
                                   <tr>
                                        <td>Vender</td>
                                        <td>{items.SupplierName}</td>
                                   </tr>
                                   <tr>
                                        <td>Expense Account</td>
                                        <td>{items.Expenseaccount}</td>
                                   </tr>
                                   <tr>
                                        <td>Description</td>
                                        <td>{items.Description}</td>
                                   </tr>
                                   <tr>
                                        <td>Amount</td>
                                        <td>{items.Amount}</td>
                                   </tr>
                                   <tr>
                                        <td>Cash Paid</td>
                                        <td>{items.Cashpaid}</td>
                                   </tr>
                                   <tr>
                                        <td>Bank Transfer</td>
                                        <td>{items.Banktransfer}</td>
                                   </tr>
                                   <tr>
                                        <td>Bank</td>
                                        <td>{items.BankCode}</td>
                                   </tr>
                                   <tr>
                                        <td>Cheque Paid</td>
                                        <td>{items.Chequepaid}</td>
                                   </tr>
                                   <tr>
                                        <td>Approved By</td>
                                        <td>{items.ApprovedBy}</td>
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

      
        [HttpPost("getpropdev")]
        public async Task<ResponseDto> Getpropdev(AuthDto authDto)
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
                Location = AccessLocation.PropertyDevelopment.ToString()
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
                var Meeting = await _unitOfWork.propDevInterface.GetViewAllPropDevAll();
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

        [HttpPost("insertpropdev")]
        public async Task<ResponseDto> Insert(PropDevDto propdevDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(propdevDto.AuthDto);

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
                Location = AccessLocation.PropertyDevelopment.ToString()
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
                    if (propdevDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingpropdev = await _db.Tblpropdevs.FirstOrDefaultAsync(x => x.Id == propdevDto.Id);

                    if (existingpropdev != null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Invalid Id";
                        return _response;
                    }

                    var newUser = new Tblpropdev
                    {
                        Id = propdevDto.Id,
                        Date = propdevDto.Date,
                        Propname = propdevDto.Propname,
                        Vender = propdevDto.Vender,
                        Propertyno = propdevDto.Propertyno,
                        Expenseaccount = propdevDto.Expenseaccount,
                        Description = propdevDto.Description,
                        Amount = propdevDto.Amount,
                        Cashpaid = propdevDto.Cashpaid,
                        Banktransfer = propdevDto.Banktransfer,
                        Bankid = propdevDto.Bankid,
                        Chequepaid = propdevDto.Chequepaid,
                        Chequeid = propdevDto.Chequeid,
                        Approvedby = propdevDto.Approvedby,
                        Addon = DateTime.UtcNow,
                        Addby = authResponse.Result.Userid,
                    };

                    

                    await _db.Tblpropdevs.AddAsync(newUser);
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

        [HttpPost("updatepropdev")]
        public async Task<ResponseDto> Updatepropdev(PropDevDto propdevDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(propdevDto.AuthDto);

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
                Location = AccessLocation.PropertyDevelopment.ToString()
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
                    if (propdevDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingpropdev = await _db.Tblpropdevs.FirstOrDefaultAsync(x => x.Id == propdevDto.Id);

                    if (existingpropdev == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Id not found";
                        return _response;
                    }

                    existingpropdev.Id = propdevDto.Id;
                    existingpropdev.Date = propdevDto.Date;
                    existingpropdev.Propname = propdevDto.Propname;
                    existingpropdev.Vender = propdevDto.Vender;
                    existingpropdev.Propertyno = propdevDto.Propertyno;
                    existingpropdev.Expenseaccount = propdevDto.Expenseaccount;
                    existingpropdev.Description = propdevDto.Description;
                    existingpropdev.Amount = propdevDto.Amount;
                    existingpropdev.Cashpaid = propdevDto.Cashpaid;
                    existingpropdev.Banktransfer = propdevDto.Banktransfer;
                    existingpropdev.Bankid = propdevDto.Bankid;
                    existingpropdev.Chequepaid = propdevDto.Chequepaid;
                    existingpropdev.Chequeid = propdevDto.Chequeid;
                    existingpropdev.Approvedby = propdevDto.Approvedby;

                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully updated propdev: " + existingpropdev.Id;
                    _response.Result = existingpropdev;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while updating propdev! " + ex.Message;
                    _response.IsSuccess = false;
                    return _response;
                }
        }
    }
}
