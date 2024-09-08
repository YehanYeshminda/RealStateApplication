using API.Models;
using API.Repos;
using API.Repos.Dtos;
using API.Repos.Dtos.MediaDtos;
using API.Repos.Dtos.StaffDtos;
using API.Repos.Dtos.SupplierDtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos.Dtos.VenderToServiceDto;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/vendertoservice")]
    [ApiController]
    public class VenderToServiceController : ControllerBase
    {
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseDto _response;

        public VenderToServiceController(CRMContext context, IUnitOfWork unitOfWork)
        {
            _db = context;
            _unitOfWork = unitOfWork;
            _response = new ResponseDto();
        }

        [HttpGet("report")]
        public async Task<ActionResult<VVendorToServiceList>> ReturnHtmlReport()
        {
            var tableForReport = await _db.VVendorToServiceLists.ToListAsync();

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
                               <th class='border' colspan=""6"" style=""text-align: center;"">Vender to Service Data</th>
                            </tr>
                            <tr>
                                <th class='border'>Id</th>
                                <th class='border'>Venderid</th>
                                <th class='border'>Serviceid</th>
                                <th class='border'>Status</th>
                                <th class='border'>Addby</th>
                                <th class='border'>Addon</th>
                            </tr>";


            foreach (var items in tableForReport)
            {
                string data = Convert.ToDateTime(items.Addon).ToString("yyyy-MM-dd");
                html += $@"
                                <tr>
                                      <td class='border'>{items.Id}</td>
                                      <td class='border'>{items.SupplierName}</td>
                                      <td class='border'>{items.TypeName}</td>
                                      <td class='border'>{items.Status}</td>
                                      <td class='border'>{items.AddBy}</td>
                                      <td class='border'>{data}</td>
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
        public async Task<ActionResult<VVendorToServiceList>> ReturnHtmlCellReport(int id)
        {
            var tableForReport = await _db.VVendorToServiceLists
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

                html += $@"

                                   <tr>
                                       <th colspan=""2"" style=""text-align: center;"">Vender to Service Data</th>
                                   </tr>
                                   <tr>
                                        <td>No</td>
                                        <td>{items.Id}</td>
                                   </tr>
                                   <tr>
                                        <td>Vender</td>
                                        <td>{items.SupplierName}</td>
                                   </tr>
                                   <tr>
                                        <td>Service</td>
                                        <td>{items.TypeName}</td>
                                   </tr>
                                   <tr>
                                        <td>Status</td>
                                        <td>{items.Status}</td>
                                   </tr>
                                   <tr>
                                        <td>Add by</td>
                                        <td>{items.AddBy}</td>
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

        

        [HttpPost("Add")]
        public async Task<ResponseDto> Add(VenderToServiceDto venderToServiceDto)
        {

            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(venderToServiceDto.AuthDto);

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
                Location = AccessLocation.VenderToService.ToString()
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


                var existingVTS = await _db.Tblvendertoservices.FirstOrDefaultAsync(x => x.Id == venderToServiceDto.Id);

                if (existingVTS != null)
                {
                    _response.Message = "Data with this Id already exist";
                    _response.IsSuccess = false;
                    return _response;
                };


                var DataExists = await _db.Tblvendertoservices.FirstOrDefaultAsync(x => x.Venderid == venderToServiceDto.Venderid && x.Serviceid == venderToServiceDto.Serviceid);

                if (DataExists != null)
                {
                    _response.Message = "This vendor and service combo already exists";
                    _response.IsSuccess = false;
                    return _response;
                }

                var newvts = new Tblvendertoservice
                {
                    Venderid = venderToServiceDto.Venderid,
                    Serviceid = venderToServiceDto.Serviceid,
                    Status = venderToServiceDto.Status,
                    Addby = authResponse.Result.Userid,
                    Addon = DateTime.UtcNow.Date
                };


                await _db.Tblvendertoservices.AddAsync(newvts);
                await _db.SaveChangesAsync();

                _response.Message = "";
                _response.IsSuccess = true;
                _response.Result = newvts;

                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while creating new vender to service! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }


        [HttpPost("update")]
        public async Task<ResponseDto> Update(UpdateVenderToServiceDto updateVenderToServiceDto)
        {

            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(updateVenderToServiceDto.AuthDto);

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
                Location = AccessLocation.VenderToService.ToString()
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
                if(updateVenderToServiceDto == null)
    {
                    _response.IsSuccess = false;
                    _response.Message = "Missing User Data";
                    return _response;
                }

                var existingVTS = await _db.Tblvendertoservices.FirstOrDefaultAsync(x => x.Id == updateVenderToServiceDto.Id);

                if (existingVTS == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Id not found";
                    return _response;
                }

                
                var dataExists = await _db.Tblvendertoservices
                    .Where(x => x.Id != updateVenderToServiceDto.Id) 
                    .AnyAsync(x => x.Venderid == updateVenderToServiceDto.Venderid && x.Serviceid == updateVenderToServiceDto.Serviceid);

                if (dataExists)
                {
                    _response.Message = "This vendor and service combo already exists";
                    _response.IsSuccess = false;
                    return _response;
                }

                existingVTS.Venderid = updateVenderToServiceDto.Venderid;
                existingVTS.Serviceid = updateVenderToServiceDto.Serviceid;
                existingVTS.Status = updateVenderToServiceDto.Status;

                //try
                //{
                //    await _db.SaveChangesAsync();

                //    _response.IsSuccess = true;
                //    _response.Message = "Successfully updated data: " + existingVTS.Id;
                //    _response.Result = existingVTS;
                //}

                //if (updateVenderToServiceDto == null)
                //{
                //    _response.IsSuccess = false;
                //    _response.Message = "Missing User Data";
                //    return _response;
                //}

                //var existingVTS = await _db.Tblvendertoservices.FirstOrDefaultAsync(x => x.Id == updateVenderToServiceDto.Id);

                //if (existingVTS == null)
                //{
                //    _response.IsSuccess = false;
                //    _response.Message = "Id not found";
                //    return _response;
                //}


                //existingVTS.Venderid = updateVenderToServiceDto.Venderid;
                //existingVTS.Serviceid = updateVenderToServiceDto.Serviceid;
                //existingVTS.Status = updateVenderToServiceDto.Status;



                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Successfully updated data: " + existingVTS.Id;
                _response.Result = existingVTS;
                return _response;
            }
            catch (Exception ex)
                {
                    _response.Message = "Error while updating data! " + ex.Message;
                    _response.IsSuccess = false;
                    return _response;
                }
        }


        //[HttpPost("GetAllData")]
        //public async Task<ResponseDto> GetAllData(AuthDto authDto)
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
        //        Location = AccessLocation.VenderToService.ToString()
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
        //            var existingVTS = await _unitOfWork.vendorToServiceInterface.GetAllVendorsViewAsync();

        //            _response.Message = "";
        //            _response.IsSuccess = true;
        //            _response.Result = existingVTS;

        //            return _response;
        //        }
        //        catch (Exception ex)
        //        {
        //            _response.Message = "Error while getting Data! " + ex.Message;
        //            _response.IsSuccess = false;
        //            return _response;
        //        }
        //}




        [HttpPost("GetAllData")]
        public async Task<ResponseDto> GetAllData(AuthDto authDto, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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
                Location = AccessLocation.VenderToService.ToString()
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
                var existingVTS = await _unitOfWork.vendorToServiceInterface.GetAllVendorsViewAsync();

                var totalCount = existingVTS.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                var leadsPerPage = existingVTS.Skip((page - 1) * pageSize).Take(pageSize).ToList();


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
