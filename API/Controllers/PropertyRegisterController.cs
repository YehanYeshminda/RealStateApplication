using API.Models;
using API.Repos.Dtos;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using API.Repos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Repos.Dtos.PropertyRegiterDtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos;

namespace API.Controllers
{
    [Route("api/PropertyRegister")]
    [ApiController]
    public class PropertyRegisterController : ControllerBase
    {

        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly GlobalDataService _globalDataService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public PropertyRegisterController(IConfiguration configuration, CRMContext db, IUnitOfWork unitOfWork, GlobalDataService globalDataService, IWebHostEnvironment webHostEnvironment)
        {
            _response = new ResponseDto();
            _db = db;
            _unitOfWork = unitOfWork;
            _globalDataService = globalDataService;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        [HttpGet("report")]
        public async Task<ActionResult<Tblpropertyregister>> ReturnHtmlReport()
        {
            var tableForReport = await _db.Tblpropertyregisters.ToListAsync();

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
                                <th class='border' colspan=""21"" style=""text-align: center;"">Property Development Data</th>
                            </tr>
                            <tr>
                                <th class='border'>Id</th>
                                <th class='border'>Type</th>
                                <th class='border'>Category</th>
                                <th class='border'>Subcategory</th>
                                <th class='border'>City</th>
                                <th class='border'>Nationality</th>
                                <th class='border'>Address</th>
                                <th class='border'>Vender</th>
                                <th class='border'>Costanually</th>
                                <th class='border'>Othercost</th>
                                <th class='border'>Rulesregulations</th>
                                <th class='border'>Status</th>
                                <th class='border'>Sellingprice</th>
                                <th class='border'>Anualcostforbuyer</th>
                                <th class='border'>Deposit</th>
                                <th class='border'>Contacttype</th>
                                <th class='border'>Socialmedia</th>
                                <th class='border'>Dateofpurchorrent</th>
                                <th class='border'>Renewdate</th>
                                <th class='border'>Venderpaymenthate</th>
                                <th class='border'>Paymentscheduleno</th>
                            </tr>";


            foreach (var items in tableForReport)
            {

                string dateofpurchorrent = Convert.ToDateTime(items.Dateofpurchorrent).ToString("yyyy-MM-dd");
                string renewdate = Convert.ToDateTime(items.Renewdate).ToString("yyyy-MM-dd");
                string venderpaymentdate = Convert.ToDateTime(items.Venderpaymentdate).ToString("yyyy-MM-dd");

                html += $@"
                   <tr>
                        <td class='border'>{items.Id}</td>
                        <td class='border'>{items.Type}</td>
                        <td class='border'>{items.Category}</td>
                        <td class='border'>{items.Subcategory}</td>
                        <td class='border'>{items.City}</td>
                        <td class='border'>{items.Nationality}</td>
                        <td class='border'>{items.Address}</td>
                        <td class='border'>{items.Vender}</td>
                        <td class='border'>{items.Costanually}</td>
                        <td class='border'>{items.Othercost}</td>
                        <td class='border'>{items.Rulesregulations}</td>
                        <td class='border'>{items.Status}</td>
                        <td class='border'>{items.Sellingprice}</td>
                        <td class='border'>{items.Anualcostforbuyer}</td>
                        <td class='border'>{items.Deposit}</td>
                        <td class='border'>{items.Contacttype}</td>
                        <td class='border'>{items.Socialmedia}</td>
                        <td class='border'>{dateofpurchorrent}</td>
                        <td class='border'>{renewdate}</td>
                        <td class='border'>{venderpaymentdate}</td>
                        <td class='border'>{items.Paymentscheduleno}</td>
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
        public async Task<ActionResult<Tblpropertyregister>> ReturnHtmlCellReport(string id)
        {
            var tableForReport = await _db.Tblpropertyregisters
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

                string dateofpurchorrent = Convert.ToDateTime(items.Dateofpurchorrent).ToString("yyyy-MM-dd");
                string renewdate = Convert.ToDateTime(items.Renewdate).ToString("yyyy-MM-dd");
                string venderpaymentdate = Convert.ToDateTime(items.Venderpaymentdate).ToString("yyyy-MM-dd");

                html += $@"
                                   <tr>
                                       <th colspan=""2"" style=""text-align: center;"">Property Registration Data</th>
                                   </tr>
                                   <tr>
                                        <td>No</td>
                                        <td class='border'>{items.Id}</td>
                                   </tr>
                                   <tr>
                                        <td>Type</td>
                                        <td class='border'>{items.Type}</td>
                                   </tr>
                                   <tr>
                                        <td>Category</td>
                                        <td class='border'>{items.Category}</td>
                                   </tr>
                                   <tr>
                                        <td>Sub Category</td>
                                        <td class='border'>{items.Subcategory}</td>
                                   </tr>
                                   <tr>
                                        <td>City</td>
                                        <td class='border'>{items.City}</td>
                                   </tr>
                                   <tr>
                                        <td>Nationality</td>
                                        <td class='border'>{items.Nationality}</td>
                                   </tr>
                                   <tr>
                                        <td>Address</td>
                                        <td class='border'>{items.Address}</td>
                                   </tr>
                                   <tr>
                                        <td>Vender</td>
                                        <td class='border'>{items.Vender}</td>
                                   </tr>
                                   <tr>
                                        <td>Anual Cost</td>
                                        <td class='border'>{items.Costanually}</td>
                                   </tr>
                                   <tr>
                                        <td>Other Cost</td>
                                        <td class='border'>{items.Othercost}</td>
                                   </tr>
                                   <tr>
                                        <td>Rules and Regulations</td>
                                        <td class='border'>{items.Rulesregulations}</td>
                                   </tr>
                                   <tr>
                                        <td>Status</td>
                                        <td class='border'>{items.Status}</td>
                                   </tr>
                                   <tr>
                                        <td>Selling Price</td>
                                        <td class='border'>{items.Sellingprice}</td>
                                   </tr>
                                   <tr>
                                        <td>Anual cost for buyers</td>
                                        <td class='border'>{items.Anualcostforbuyer}</td>
                                   </tr>
                                   <tr>
                                        <td>Deposit</td>
                                        <td class='border'>{items.Deposit}</td>
                                   </tr>
                                   <tr>
                                        <td>Contact Type</td>
                                        <td class='border'>{items.Contacttype}</td>
                                   </tr>
                                   <tr>
                                        <td>Social Media</td>
                                        <td class='border'>{items.Socialmedia}</td>
                                   </tr>
                                   <tr>
                                        <td>Date of Purchase or Rent</td>
                                        <td class='border'>{dateofpurchorrent}</td>
                                   </tr>
                                   <tr>
                                        <td>Renew Date</td>
                                        <td class='border'>{renewdate}</td>
                                   </tr>
                                   <tr>
                                        <td>Vender payment Date</td>
                                        <td class='border'>{venderpaymentdate}</td>
                                   </tr>
                                   <tr>
                                        <td><Payment Schedule No/td>
                                        <td class='border'>{items.Paymentscheduleno}</td>
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

        //[HttpPost("getpropreg")]
        //public async Task<ResponseDto> Getpropreg(AuthDto authDto)
        //{
        //    var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

        //    if (!authResponse.IsSuccess)
        //    {
        //        _response.Message = authResponse.Message;
        //        _response.IsSuccess = authResponse.IsSuccess;
        //        _response.Result = authResponse.Result;
        //        return _response;
        //    }

        //    try
        //    {
        //        var Meeting = await _unitOfWork.propertyRegisterInterface.GetViewPropertyRegisterAll();
        //        _response.IsSuccess = true;
        //        _response.Message = "";
        //        _response.Result = Meeting;
        //        return _response;
        //    }

        //    catch (Exception ex)
        //    {
        //        _response.Message = "Error while Getting! " + ex.Message;
        //        _response.IsSuccess = false;
        //        return _response;
        //    }
        //}

        [HttpPost("getpropreg")]
        public async Task<ResponseDto> Getpropreg(AuthDto authDto, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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
                Location = AccessLocation.Staffs.ToString()
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
                var datalists =  await _unitOfWork.propertyRegisterInterface.GetViewPropertyRegisterAll();

                var totalCount = datalists.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                var leadsPerPage = datalists.Skip((page - 1) * pageSize).Take(pageSize).ToList();


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

        [HttpPost("insertpropreg")]
        public async Task<ResponseDto> Insert([FromForm] PropertyRegisterDto propregDto)
        {
            if (propregDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(propregDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == propregDto.Hash);

            if (_user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid Hash";
                return _response;
            }

            var decryptedDateWithOffset = decodedValues.Date.AddDays(1);
            var currentDate = DateTime.UtcNow.Date;

            if (currentDate < decryptedDateWithOffset.Date)
            {
                try
                {
                    if (propregDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingpropreg = await _db.Tblpropertyregisters.FirstOrDefaultAsync(x => x.Id == propregDto.Id);

                    if (existingpropreg != null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Invalid Id";
                        return _response;
                    }

                    var existingname = await _db.Tblpropertyregisters.FirstOrDefaultAsync(x => x.Propertname == propregDto.Propertname);

                    if (existingname != null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Name Already exist";
                        return _response;
                    }

                    IPasswordGenerator passwordGenerator = new PasswordGenerator();
                    string randomPassword = passwordGenerator.GeneratePassword(8);

                    var newUser = new Tblpropertyregister
                    {
                        Id = propregDto.Id,
                        Propertname = propregDto.Propertname,
                        Type = propregDto.Type,
                        Category = propregDto.Category,
                        Subcategory = propregDto.Subcategory,
                        City = propregDto.City,
                        Nationality = propregDto.Nationality,
                        Address = propregDto.Address,
                        Geolocation = propregDto.Geolocation,
                        Vender = propregDto.Vender,
                        Costanually = propregDto.Costanually,
                        Othercost = propregDto.Othercost,
                        Rulesregulations = propregDto.Rulesregulations,
                        Status = propregDto.Status,
                        Sellingprice = propregDto.Sellingprice,
                        Minsellingprice = propregDto.Minsellingprice,
                        Anualcostforbuyer = propregDto.Anualcostforbuyer,
                        Deposit = propregDto.Deposit,
                        Contacttype = propregDto.Contacttype,
                        Socialmedia = propregDto.Socialmedia,
                        Otherimg = propregDto.Id,
                        Dateofpurchorrent = propregDto.Dateofpurchorrent,
                        Renewdate = propregDto.Renewdate,
                        Venderpaymentdate = propregDto.Venderpaymentdate,
                        Paymentscheduleno = propregDto.Paymentscheduleno,
                        Addby = _user.Userid,
                        Addon = DateTime.UtcNow,
                    };

                    string filePath = GetFilePath();
                    string filePathOther = GetFilePathOther(propregDto.Id);

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    if (!Directory.Exists(filePathOther))
                    {
                        Directory.CreateDirectory(filePathOther);
                    }

                    string imagePath = filePath + "\\" + propregDto.MainImg.FileName + ".png";
                    string imagePathOther = filePathOther;

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    if (System.IO.File.Exists(imagePathOther))
                    {
                        System.IO.File.Delete(imagePathOther);
                    }

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await propregDto.MainImg.CopyToAsync(stream);
                        newUser.Mainimg = propregDto.MainImg.FileName + ".png";
                    }

                    if (propregDto.OtherImages0 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages0.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages0.CopyToAsync(stream);
                        }
                    }


                    if (propregDto.OtherImages1 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages1.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages1.CopyToAsync(stream);
                        }
                    }


                    if (propregDto.OtherImages2 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages2.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages2.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages3 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages3.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages3.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages4 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages4.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages4.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages5 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages5.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages5.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages6 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages6.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages6.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages7 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages7.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages7.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages8 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages8.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages8.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages9 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages9.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages9.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages10 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages10.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages10.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages11 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages11.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages10.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages12 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages12.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages12.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages13 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages13.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages13.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages14 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages14.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages14.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages15 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages15.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages12.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages16 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages16.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages16.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages17 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages17.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages17.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages18 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages18.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages18.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages19 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages19.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages19.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages20 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages20.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages20.CopyToAsync(stream);
                        }
                    }


                    newUser.Otherimg = propregDto.Id;

                    if (newUser.Mainimg == "")
                    {
                        _response.Message = "Error while saving image";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    if (newUser.Otherimg == "")
                    {
                        _response.Message = "Error while saving other image";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    await _db.Tblpropertyregisters.AddAsync(newUser);
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
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid Hash";
                return _response;
            }
        }

        [NonAction]
        private string GetFilePath()
        {
            return _webHostEnvironment.WebRootPath + "\\upload\\property\\main";
        }

        [NonAction]
        private string GetFilePathOther(string id)
        {
            return _webHostEnvironment.WebRootPath + "\\upload\\property\\main\\other\\" + id;
        }


        [HttpPost("updatepropreg")]
        public async Task<ResponseDto> Updatepropreg([FromForm] PropertyRegisterDto propregDto)
        {
            if (propregDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(propregDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == propregDto.Hash);

            if (_user == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid Hash";
                return _response;
            }

            var decryptedDateWithOffset = decodedValues.Date.AddDays(1);
            var currentDate = DateTime.UtcNow.Date;

            if (currentDate < decryptedDateWithOffset.Date)
            {
                try
                {
                    if (propregDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingpropreg = await _db.Tblpropertyregisters.FirstOrDefaultAsync(x => x.Id == propregDto.Id);

                    if (existingpropreg == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Id not found";
                        return _response;
                    }
                    existingpropreg.Id = propregDto.Id;
                    existingpropreg.Propertname = propregDto.Propertname;
                    existingpropreg.Type = propregDto.Type;
                    existingpropreg.Category = propregDto.Category;
                    existingpropreg.Subcategory = propregDto.Subcategory;
                    existingpropreg.City = propregDto.City;
                    existingpropreg.Nationality = propregDto.Nationality;
                    existingpropreg.Address = propregDto.Address;
                    existingpropreg.Geolocation = propregDto.Geolocation;
                    existingpropreg.Vender = propregDto.Vender;
                    existingpropreg.Costanually = propregDto.Costanually;
                    existingpropreg.Othercost = propregDto.Othercost;
                    existingpropreg.Rulesregulations = propregDto.Rulesregulations;
                    existingpropreg.Status = propregDto.Status;
                    existingpropreg.Sellingprice = propregDto.Sellingprice;
                    existingpropreg.Minsellingprice = propregDto.Minsellingprice;
                    existingpropreg.Anualcostforbuyer = propregDto.Anualcostforbuyer;
                    existingpropreg.Deposit = propregDto.Deposit;
                    existingpropreg.Contacttype = propregDto.Contacttype;
                    existingpropreg.Socialmedia = propregDto.Socialmedia;
                    //existingpropreg.Mainimg = propregDto.Mainimg;
                    existingpropreg.Otherimg = propregDto.Id;
                    existingpropreg.Dateofpurchorrent = propregDto.Dateofpurchorrent;
                    existingpropreg.Renewdate = propregDto.Renewdate;
                    existingpropreg.Venderpaymentdate = propregDto.Venderpaymentdate;
                    existingpropreg.Paymentscheduleno = propregDto.Paymentscheduleno;
                    string filePath = GetFilePath();
                    string filePathOther = GetFilePathOther(propregDto.Id);

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    if (!Directory.Exists(filePathOther))
                    {
                        Directory.CreateDirectory(filePathOther);
                    }

                    string imagePath = filePath + "\\" + propregDto.MainImg.FileName + ".png";
                    string imagePathOther = filePathOther;

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    if (System.IO.File.Exists(imagePathOther))
                    {
                        System.IO.File.Delete(imagePathOther);
                    }

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await propregDto.MainImg.CopyToAsync(stream);
                        existingpropreg.Mainimg = propregDto.MainImg.FileName + ".png";
                    }

                    if (propregDto.OtherImages0 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages0.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages0.CopyToAsync(stream);
                        }
                    }


                    if (propregDto.OtherImages1 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages1.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages1.CopyToAsync(stream);
                        }
                    }


                    if (propregDto.OtherImages2 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages2.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages2.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages3 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages3.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages3.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages4 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages4.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages4.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages5 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages5.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages5.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages6 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages6.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages6.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages7 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages7.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages7.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages8 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages8.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages8.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages9 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages9.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages9.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages10 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages10.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages10.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages11 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages11.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages10.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages12 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages12.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages12.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages13 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages13.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages13.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages14 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages14.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages14.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages15 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages15.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages12.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages16 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages16.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages16.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages17 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages17.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages17.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages18 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages18.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages18.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages19 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages19.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages19.CopyToAsync(stream);
                        }
                    }

                    if (propregDto.OtherImages20 != null)
                    {
                        using (var stream = new FileStream(imagePathOther + "\\" + propregDto.OtherImages20.FileName + ".png", FileMode.Create))
                        {
                            await propregDto.OtherImages20.CopyToAsync(stream);
                        }
                    }


                    existingpropreg.Otherimg = propregDto.Id;

                    if (existingpropreg.Mainimg == "")
                    {
                        _response.Message = "Error while saving image";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    if (existingpropreg.Otherimg == "")
                    {
                        _response.Message = "Error while saving other image";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully updated propreg: " + existingpropreg.Id;
                    _response.Result = existingpropreg;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while updating propreg! " + ex.Message;
                    _response.IsSuccess = false;
                    return _response;
                }
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid Hash";
                return _response;
            }
        }
    }
}
