using API.Models;
using API.Repos.Dtos;
using API.Repos.Helpers;
using Microsoft.AspNetCore.Mvc;
using API.Repos.Dtos.CampainHDtos;
using API.Repos.Interfaces;
using API.Repos.Dtos.CommonDto;
using Microsoft.EntityFrameworkCore;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos;

namespace API.Controllers
{

    [Route("api/CampainH")]
    [ApiController]
    public class CampainHController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public CampainHController(CRMContext db, IUnitOfWork unitOfWork)
        {
            _response = new ResponseDto();
            _db = db;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("report")]
        public async Task<ActionResult<TblCampainH>> ReturnHtmlReport()
        {
            var tableForReport = await _db.TblCampainHs.ToListAsync();

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
                                <th class='border' colspan=""12"" style=""text-align: center;"">Campaign Data</th>
                            </tr>
                            <tr>
                                <th class='border'>No</th>
                                <th class='border'>Name</th>
                                <th class='border'>Date</th>
                                <th class='border'>Description</th>
                                <th class='border'>Totalcost</th>
                                <th class='border'>Remarks</th>
                                <th class='border'>Dateto</th>
                                <th class='border'>DateFrom</th>
                                <th class='border'>Status</th>
                                <th class='border'>Remarks</th>
                                <th class='border'>Addby</th>
                                <th class='border'>Addon</th>
                            </tr>";


            foreach (var items in tableForReport)
            {

                string addon = Convert.ToDateTime(items.Addon).ToString("yyyy-MM-dd");
                string dateto= Convert.ToDateTime(items.Dateto).ToString("yyyy-MM-dd");
                string date = Convert.ToDateTime(items.Date).ToString("yyyy-MM-dd");
                string dateFrom = Convert.ToDateTime(items.Datefrom).ToString("yyyy-MM-dd");

                html += $@"
                                <tr>
                                      <td class='border'>{items.No}</td>
                                      <td class='border'>{items.Name}</td>
                                      <td class='border'>{date}</td>
                                      <td class='border'>{items.Description}</td>
                                      <td class='border'>{items.Totalcost}</td>
                                      <td class='border'>{items.Remarks}</td>
                                      <td class='border'>{dateto}</td>
                                      <td class='border'>{dateFrom}</td>
                                      <td class='border'>{items.Status}</td>
                                      <td class='border'>{items.Remarks}</td>
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
        public async Task<ActionResult<TblVcampaign>> ReturnHtmlCellReport(string id)
        {
            var tableForReport = await _db.TblVcampaigns
                .Where(x => x.No == id)
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
                string dateto = Convert.ToDateTime(items.Dateto).ToString("yyyy-MM-dd");
                string date = Convert.ToDateTime(items.Date).ToString("yyyy-MM-dd");
                string dateFrom = Convert.ToDateTime(items.Datefrom).ToString("yyyy-MM-dd");

                html += $@"
                            <tr>
                                <th colspan=""2"" style=""text-align: center;"">Campaign Data</th>
                            </tr>
                                   <tr>
                                        <td>No</td>
                                        <td>{items.No}</td>
                                   </tr>
                                   <tr>
                                        <td>Name</td>
                                        <td>{items.Name}</td>
                                   </tr>
                                   <tr>
                                        <td>Date</td>
                                        <td>{date}</td>
                                   </tr>
                                   <tr>
                                        <td>Description</td>
                                        <td>{items.Description}</td>
                                   </tr>
                                   <tr>
                                        <td>Total Cost</td>
                                        <td>{items.Totalcost}</td>
                                   </tr>
                                   <tr>
                                        <td>Remarks</td>
                                        <td>{items.Remarks}</td>
                                   </tr>
                                   <tr>
                                        <td>Date From</td>
                                        <td>{dateFrom}</td>
                                   </tr>
                                   <tr>
                                        <td>Date To</td>
                                        <td>{dateto}</td>
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

        [HttpPost("insertCampainH")]
        public async Task<ResponseDto> InsertCampain(CampainHDtos CampainHDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(CampainHDto.AuthDto);

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
                Location = AccessLocation.CampaignDetails.ToString()
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
                if (CampainHDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Missing User Data";
                    return _response;
                }

                var existingCampain = await _unitOfWork.campaignInterface.GetCompaignByIdAsync(CampainHDto.No);

                if (existingCampain != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid Id";
                    return _response;
                };

                var newItem = new TblCampainH
                {
                    No = CampainHDto.No,
                    Date = CampainHDto.Date,
                    Name = CampainHDto.Name,
                    Datefrom = CampainHDto.Datefrom,
                    Dateto = CampainHDto.Dateto,
                    Description = CampainHDto.Description,
                    Totalcost = CampainHDto.Totalcost,
                    Remarks = CampainHDto.Remarks,
                    Status = CampainHDto.Status,
                    Addby = authResponse.Result.Userid,
                    Addon = DateTime.UtcNow.Date
                };

                _unitOfWork.campaignInterface.AddNewCampaignH(newItem);

                await _db.SaveChangesAsync();

                foreach (var item in CampainHDto.mediaIds)
                {
                    var existingCampaign = await _unitOfWork.campaignInterface.GetCompaignByIdAsync(newItem.No);

                    if (existingCampaign == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "CampainH with " + item + " does not exist";
                        return _response;
                    }

                    var existingCMediaWithId = await _unitOfWork.mediaInterface.GetMediaWithId(item);

                    if (existingCMediaWithId == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Media with " + item + " does not exist";
                        return _response;
                    }

                    var newexistingMedia = new TblCampainMedium
                    {
                        Addby = authResponse.Result.Userid,
                        Addon = DateTime.UtcNow,
                        Mediaid = existingCMediaWithId.Id.ToString(),
                        Campainno = newItem.No,
                    };

                    await _db.TblCampainMedia.AddAsync(newexistingMedia);
                }

                await _db.SaveChangesAsync();

 
                var newMedialink = new TblMediaLink
                {
                    Addby = authResponse.Result.Userid,
                    Addon = DateTime.UtcNow,
                    Medialink = CampainHDto.medialink,
                    Campainno = newItem.No,
                };

                await _db.TblMediaLinks.AddAsync(newMedialink);

                await _db.SaveChangesAsync();


                if (await _unitOfWork.Complete())
                {
                    _response.IsSuccess = true;
                    _response.Message = "Successfully created " + newItem.No;
                    _response.Result = newItem;
                    return _response;
                }
                
                return _response;

            }
            catch (Exception ex)
            {
                _response.Message = "Error while inserting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }


        [HttpPost("updateCampainH")]
        public async Task<ResponseDto> UpdateCampain(CampainHDtos CampainHDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(CampainHDto.AuthDto);

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
                Location = AccessLocation.CampaignDetails.ToString()
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
                    if (CampainHDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingCampain = await _unitOfWork.campaignInterface.GetCompaignByIdAsync(CampainHDto.No);

                    if (existingCampain == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Id not found";
                        return _response;
                    }

                    existingCampain.No = CampainHDto.No;
                    existingCampain.Date = CampainHDto.Date;
                    existingCampain.Name = CampainHDto.Name;
                    existingCampain.Datefrom = CampainHDto.Datefrom;
                    existingCampain.Dateto = CampainHDto.Dateto;
                    existingCampain.Description = CampainHDto.Description;
                    existingCampain.Totalcost = CampainHDto.Totalcost;
                    existingCampain.Remarks = CampainHDto.Remarks;
                    existingCampain.Status = CampainHDto.Status;


                    var existingMedias = await _db.TblCampainMedia
                        .Where(x => x.Campainno == existingCampain.No)
                        .ToListAsync();

                    _db.TblCampainMedia.RemoveRange(existingMedias);

                    foreach (var item in CampainHDto.mediaIds)
                    {
                        var existingMediaWithId = await _unitOfWork.mediaInterface.GetMediaWithId(item);

                        if (existingMediaWithId == null)
                        {
                            _response.IsSuccess = false;
                            _response.Message = "Staff with " + item + " does not exist";
                            return _response;
                        }

                        var updateforcampaign = new TblCampainMedium
                        {
                            Mediaid = existingMediaWithId.Id.ToString(),
                            Campainno = existingCampain.No,
                            Addby = authResponse.Result.Userid,
                            Addon = DateTime.UtcNow.Date,
                        };

                        await _db.TblCampainMedia.AddAsync(updateforcampaign);
                    }


                    var existingMedialinks = await _db.TblMediaLinks
                        .Where(x => x.Campainno == existingCampain.No)
                        .ToListAsync();

                    _db.TblMediaLinks.RemoveRange(existingMedialinks);

                    var newMedialink = new TblMediaLink
                    {
                        Medialink = CampainHDto.medialink,
                        Campainno = existingCampain.No,
                        Addon = DateTime.UtcNow,
                        Addby = authResponse.Result.Userid
                    };

                    await _db.TblMediaLinks.AddAsync(newMedialink);

                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully updated CampainH: " + CampainHDto.No;
                    _response.Result = CampainHDto;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while updating CampainH! " + ex.Message;
                    _response.IsSuccess = false;
                    return _response;
                }
            
        }

        [HttpPost("getCampainH")]
        public async Task<ResponseDto> GetAllCompaignsAsync(AuthDto authDto)
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
                Location = AccessLocation.CampaignDetails.ToString()
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
                var Meeting = await _unitOfWork.campaignInterface.GetAllCompaignsAsync();
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

        [HttpPost("GetCompaignNameId")]
        public async Task<ActionResult<List<BankInfoDto>>> GetCompaignIds(AuthDto authDto)
        {
            if (authDto.Hash == null)
            {
                return BadRequest("Please provide hash");
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(authDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == authDto.Hash);

            if (_user == null)
            {
                return BadRequest("Invalid Hash");
            }

            var decryptedDateWithOffset = decodedValues.Date.AddDays(1);
            var currentDate = DateTime.UtcNow.Date;

            if (currentDate < decryptedDateWithOffset.Date)
            {
                try
                {
                    var newList = new List<BankInfoDto>();
                    var existingBanks = await _unitOfWork.campaignInterface.GetAllCompaignsAsync();

                    foreach (var item in existingBanks)
                    {
                        var newItemToAdd = new BankInfoDto
                        {
                            textValue = item.Name,
                            value = item.No
                        };

                        newList.Add(newItemToAdd);
                    }

                    return Ok(newList);
                }
                catch (Exception ex)
                {
                    return BadRequest("Error while getting banks! " + ex.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Hash");
            }
        }


        [HttpPost("GetMediaIdAll")]
        public async Task<ActionResult<List<CommonDto>>> GetMediaIdAll(AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                return BadRequest(authResponse.Message);
            }

            try
            {
                var newList = new List<CommonDto>();
                var existingBanks = await _db.TblMedia.ToListAsync();

                foreach (var item in existingBanks)
                {
                    var newItemToAdd = new CommonDto
                    {
                        textValue = item.Media,
                        value = item.Id
                    };

                    newList.Add(newItemToAdd);
                }

                return Ok(newList);
            }
            catch (Exception ex)
            {
                return BadRequest("Error! " + ex.Message);
            }
        }

    }
}
