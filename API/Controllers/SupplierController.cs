using API.Models;
using API.Repos;
using API.Repos.Dtos;
using API.Repos.Dtos.SupplierDtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/supplier")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseDto _response;

        public SupplierController(CRMContext context, IUnitOfWork unitOfWork)
        {
            _db = context;
            _unitOfWork = unitOfWork;
            _response = new ResponseDto();
        }

        [HttpGet("report")]
        public async Task<ActionResult<TblSupplier>> ReturnHtmlReport()
        {
            var tableForReport = await _db.TblSuppliers.ToListAsync();

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
                                       <th class='border' colspan=""8"" style=""text-align: center;"">Vendor Data</th>
                                   </tr>
                            <tr>
                                <th class='border'>Supplier Id</th>
                                <th class='border'>Supplier Name</th>
                                <th class='border'>Email</th>
                                <th class='border'>Address</th>
                                <th class='border'>Credit Period</th>
                                <th class='border'>Mobile</th>
                                <th class='border'>Fax</th>
                                <th class='border'>Vat No</th>
                            </tr>";


            foreach (var items in tableForReport)
            {


                html += $@"
                                <tr>
                                      <td class='border'>{items.SupplierId}</td>
                                      <td class='border'>{items.SupplierName}</td>
                                      <td class='border'>{items.Email}</td>
                                      <td class='border'>{items.Address}</td>
                                      <td class='border'>{items.CreditPeriod}</td>
                                      <td class='border'>{items.Mobile}</td>
                                      <td class='border'>{items.Fax}</td>
                                      <td class='border'>{items.VatNo}</td>
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

        [HttpGet("cellreport/{suppID}")]
        public async Task<ActionResult<TblSupplier>> ReturnHtmlCellReport(int suppID)
        {
            var tableForReport = await _db.TblSuppliers
                .Where(x => x.SupplierId == suppID)
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
                                           <th colspan=""2"" style=""text-align: center;"">Vender Data</th>
                                       </tr>
                                       <tr>
                                            <td>Supplier Id</td>
                                            <td>{items.SupplierId}</td>
                                       </tr>
                                       <tr>
                                            <td>Name</td>
                                            <td>{items.SupplierName}</td>
                                       </tr>
                                       <tr>
                                            <td>Email</td>
                                            <td>{items.Email}</td>
                                       </tr>
                                       <tr>
                                            <td>Address</td>
                                            <td>{items.Address}</td>
                                       </tr>
                                       <tr>
                                            <td>Credit Period</td>
                                            <td>{items.CreditPeriod}</td>
                                       </tr>
                                       <tr>
                                            <td>Mobile</td>
                                            <td>{items.Mobile}</td>
                                       </tr>
                                       <tr>
                                            <td>Fax</td>
                                            <td>{items.Fax}</td>
                                       </tr>
                                       <tr>
                                            <td>Vat</td>
                                            <td>{items.VatNo}</td>
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

        [HttpPost("AddNewSupplier")]
        public async Task<ResponseDto> AddNewSupplier(CreateNewSupplierDto createNewSupplierDto)
        {

            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(createNewSupplierDto.AuthDto);

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
                Location = AccessLocation.VenderRegister.ToString()
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
                    var existingCompany = await _db.Tblcompanies.FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(createNewSupplierDto.Cid));

                    if (existingCompany == null)
                    {
                        _response.Message = "Company with this id does not exist";
                        _response.IsSuccess = false;
                        return _response;
                    };

                    var existingStaff = await _db.Tblstaffs.FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(createNewSupplierDto.Staff));

                    if (existingStaff == null)
                    {
                        _response.Message = "staff with this id does not exist";
                        _response.IsSuccess = false;
                        return _response;
                    };

                    var existingSupplierWithName = await _db.TblSuppliers.FirstOrDefaultAsync(x => x.SupplierName == createNewSupplierDto.SupplierName);

                    if (existingSupplierWithName != null)
                    {
                        _response.Message = "Supplier with this name already exist";
                        _response.IsSuccess = false;
                        return _response;
                    };

                    var newSupplier = new TblSupplier
                    {
                        Address = createNewSupplierDto.Address,
                        Cid = existingCompany.Id.ToString(),
                        CreditPeriod = createNewSupplierDto.CreditPeriod,
                        Email = createNewSupplierDto.Email,
                        Mobile = createNewSupplierDto.Mobile,
                        Fax = createNewSupplierDto.Fax,
                        Phone = createNewSupplierDto.Phone,
                        Status = createNewSupplierDto.Status,
                        Staff = existingStaff.Id,
                        SupplierName = createNewSupplierDto.SupplierName,
                        VatNo = createNewSupplierDto.VatNo,
                    };

                    await _db.TblSuppliers.AddAsync(newSupplier);
                    await _db.SaveChangesAsync();

                    _response.Message = "";
                    _response.IsSuccess = true;
                    _response.Result = newSupplier;

                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while creating new  supplier! " + ex.Message;
                    _response.IsSuccess = false;
                    return _response;
                }
        }


        [HttpPost("updatesupplier")]
        public async Task<ResponseDto> Updatesupplier(updateSupplierDto updatesupplier)
        {

            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(updatesupplier.AuthDto);

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
                Location = AccessLocation.VenderRegister.ToString()
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
                    if (updatesupplier == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingsupplier = await _unitOfWork.supplierInterface.GetSupplierByIdAsync(updatesupplier.SupplierId);

                    if (existingsupplier == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Id not found";
                        return _response;
                    }

                    existingsupplier.Address = updatesupplier.Address;
                    existingsupplier.CreditPeriod = updatesupplier.CreditPeriod;
                    existingsupplier.Email = updatesupplier.Email;
                    existingsupplier.Mobile = updatesupplier.Mobile;
                    existingsupplier.Fax = updatesupplier.Fax;
                    existingsupplier.Phone = updatesupplier.Phone;
                    existingsupplier.Status = updatesupplier.Status;
                    //existingsupplier.Staff = updatesupplier.Staff;
                    existingsupplier.SupplierName = updatesupplier.SupplierName;
                    existingsupplier.VatNo = updatesupplier.VatNo;

                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully updated supplier: " + existingsupplier.SupplierId;
                    _response.Result = existingsupplier;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while updating supplier! " + ex.Message;
                    _response.IsSuccess = false;
                    return _response;
                }
        }


        //[HttpPost("GetAllSuppliers")]
        //public async Task<ResponseDto> GetAllSuppliers(AuthDto authDto)
        //{
        //    var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

        //    if (!authResponse.IsSuccess)
        //    {
        //        _response.Message = authResponse.Message;
        //        _response.IsSuccess = authResponse.IsSuccess;
        //        _response.Result = authResponse.Result;
        //        return _response;
        //    }

        //    var newUserPermission = new SendGetUserPermission
        //    {
        //        Event = Event.GetAll.ToString(),
        //        Location = AccessLocation.VenderRegister.ToString()
        //    };

        //    var hasPermission = await _unitOfWork.userpermissionInterface.GetUserPermission(newUserPermission, authResponse.Result.Userid.ToString());

        //    if (!hasPermission.HasPermission)
        //    {
        //        _response.Message = "Access Denied";
        //        _response.IsSuccess = false;
        //        _response.Result = "";
        //        return _response;
        //    }
        //    try
        //        {
        //            var existingSuppliers = await _unitOfWork.supplierInterface.GetAllSuppliersAsync();

        //            _response.Message = "";
        //            _response.IsSuccess = true;
        //            _response.Result = existingSuppliers;

        //            return _response;
        //        }
        //        catch (Exception ex)
        //        {
        //            _response.Message = "Error while getting suppliers! " + ex.Message;
        //            _response.IsSuccess = false;
        //            return _response;
        //        }
        //}



        [HttpPost("GetAllSuppliers")]
        public async Task<ResponseDto> GetAllSuppliers(AuthDto authDto, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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
                 Location = AccessLocation.VenderRegister.ToString()
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
                var existingSuppliers = await _unitOfWork.supplierInterface.GetAllVSuppliers();

                var totalCount = existingSuppliers.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                var leadsPerPage = existingSuppliers.Skip((page - 1) * pageSize).Take(pageSize).ToList();


                _response.IsSuccess = true;
                _response.Message = "";
                _response.Result = _response.Result = new
                {
                    data = leadsPerPage,
                    totalCountPages = totalPages,
                    totalData = totalCount
                }; ;
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
