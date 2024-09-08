using API.Models;
using API.Repos.Dtos;
using API.Repos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Repos.Dtos.StaffDtos;
using API.Repos.Dtos.CommonDto;
using API.Repos.Interfaces;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace API.Controllers
{

    [Route("api/staff")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly GlobalDataService _globalDataService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public StaffController(CRMContext db, IWebHostEnvironment webHostEnvironment, GlobalDataService globalDataService, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _response = new ResponseDto();
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _globalDataService = globalDataService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpGet("report")]
        public async Task<ActionResult<Tblstaff>> ReturnHtmlReport()
        {
            var tableForReport = await _db.Tblstaffs.ToListAsync();

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
                        <div class='address'>1303, Grosvenor Business Tower - Barsha Heights - Dubai</div>
                        <div class='telephone'>04 591 3888</div>
                    </div>
                </div>

                 <div class='report-container'>
                        <table>
                                   <tr>
                                       <th class='border' colspan=""9"" style=""text-align: center;"">Staff Data</th>
                                   </tr>
                            <tr>
                                <th class='border'>Id</th>
                                <th class='border'>Name</th>
                                <th class='border'>Designation</th>
                                <th class='border'>Mobile no</th>
                                <th class='border'>Parent id</th>
                                <th class='border'>Status</th>
                                <th class='border'>User id</th>
                                <th class='border'>Add by</th>
                                <th class='border'>Add on</th>
                            </tr>";


            foreach (var items in tableForReport)
            {

                string data = Convert.ToDateTime(items.Addon).ToString("yyyy-MM-dd");

                html += $@"
                                <tr>
                                      <td class='border'>{items.Id}</td>
                                      <td class='border'>{items.Name}</td>
                                      <td class='border'>{items.Designation}</td>
                                      <td class='border'>{items.Mobileno}</td>
                                      <td class='border'>{items.Parentid}</td>
                                      <td class='border'>{items.Status}</td>
                                      <td class='border'>{items.Userid}</td>
                                      <td class='border'>{items.Addby}</td>
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
        public async Task<ActionResult<TblVstaff>> ReturnHtmlCellReport(int id)
        {
            var tableForReport = await _db.TblVstaffs
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
                string data = Convert.ToDateTime(items.Addon).ToString("yyyy-MM-dd");

                html += $@"

                                   <tr>
                                       <th colspan=""2"" style=""text-align: center;"">Staff Data</th>
                                   </tr>
                                   <tr>
                                        <td>Id</td>
                                        <td>{items.Id}</td>
                                   </tr>
                                   <tr>
                                        <td>Name</td>
                                        <td>{items.Name}</td>
                                   </tr>
                                   <tr>
                                        <td>Designation</td>
                                        <td>{items.Designation}</td>
                                   </tr>
                                   <tr>
                                        <td>Mobile</td>
                                        <td>{items.Mobileno}</td>
                                   </tr>
                                   <tr>
                                        <td>Parent</td>
                                        <td>{items.Parentid}</td>
                                   </tr>
                                   <tr>
                                        <td>Status</td>
                                        <td>{items.Status}</td>
                                   </tr>
                                   <tr>
                                        <td>User</td>
                                        <td>{items.Userid}</td>
                                   </tr>
                                   <tr>
                                        <td>Add By</td>
                                        <td>{items.Addby}</td>
                                   </tr>
                                   <tr>
                                        <td>Add On</td>
                                        <td>{data}</td>
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

        [HttpPost("insertstaff")]
        public async Task<ResponseDto> Insert([FromForm] StaffDto staffDto)
        {
            var newAuth = new AuthDto { Hash = staffDto.Hash };

            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(newAuth);

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
                if (staffDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Missing User Data";
                    return _response;
                }

                var existingstaff = await _db.Tblstaffs.FirstOrDefaultAsync(x => x.Id == staffDto.Id && x.Status == 0);

                if (existingstaff != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid Id";
                    return _response;
                }

                var existingUserWithloginName = await _db.Tblusers.FirstOrDefaultAsync(x => x.Username == staffDto.Name && x.Status == 0);

                if (existingUserWithloginName != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "staff with this username already existss";
                    return _response;
                }

                var existingUserWithloginNameStaff = await _db.Tblstaffs.FirstOrDefaultAsync(x => x.Name == staffDto.Name && x.Status == 0);

                if (existingUserWithloginNameStaff != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Staff with this username already existss";
                    return _response;
                }

                var existingUserWithEmail = await _db.Tblusers.FirstOrDefaultAsync(x => x.Email == staffDto.Email && x.Status == 0);

                if (existingUserWithEmail != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "User with this Email already existss";
                    return _response;
                }

                var existingDesignation = await _db.TblDesignationtypes.FirstOrDefaultAsync(x => x.TypeId.ToString() == staffDto.Designation);

                if (existingDesignation == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "designation does not exist with this id";
                    return _response;
                }

                var newUser = new Tblstaff
                {
                    Name = staffDto.Name,
                    Designation = existingDesignation.TypeName,
                    Mobileno = staffDto.Mobileno,
                    Parentid = staffDto.Parentid,
                    Addby = authResponse.Result.Userid,
                    Addon = DateTime.UtcNow.Date,
                    Status = 0,
                    Firstname = staffDto.Firstname,
                    Lastname = staffDto.Lastname,
                    MonthlyTarget = 0,
                    CallsMonthlyTarget = 0,
                };

                IPasswordGenerator passwordGenerator = new PasswordGenerator();
                string randomPassword = passwordGenerator.GeneratePassword(8);


                var newExistingUser = new Tbluser
                {
                    Firstname = staffDto.Firstname,
                    Cid = 1,
                    Discount = 0,
                    Email = staffDto.Email,
                    Fullaccess = 0,
                    LastName = staffDto.Lastname,
                    Loginname = staffDto.Email,
                    Openbalance = 0,
                    Status = 0,
                    Username = staffDto.Email,
                    VisaIssuedate = Convert.ToDateTime(staffDto.VisaIssueDate),
                    Logintime = DateTime.UtcNow.Date,
                    Password = randomPassword,
                };

                //var filePath = GetFilePath();

                //if (!Directory.Exists(filePath))
                //{
                //    Directory.CreateDirectory(filePath);
                //}

                //string passportPath = filePath + "\\" + staffDto.Id + "_" + staffDto.Passport.FileName + ".png" ;

                //if (System.IO.File.Exists(passportPath))
                //{
                //    System.IO.File.Delete(passportPath);
                //}
                //using (var stream = new FileStream(passportPath, FileMode.Create))
                //{
                //    await staffDto.Passport.CopyToAsync(stream);
                //    newExistingUser.Passport = staffDto.Id + "_" + staffDto.Passport.FileName + ".png";
                //}

                //string userImagePath = filePath + "\\" + staffDto.Id + "_" + staffDto.Userimage.FileName + ".png";

                //if (System.IO.File.Exists(userImagePath))
                //{
                //    System.IO.File.Delete(userImagePath);
                //}
                //using (var stream = new FileStream(userImagePath, FileMode.Create))
                //{
                //    await staffDto.Userimage.CopyToAsync(stream);
                //    newExistingUser.Userimage = staffDto.Id + "_" + staffDto.Userimage.FileName + ".png";
                //}

                await _db.Tblstaffs.AddAsync(newUser);
                await _db.Tblusers.AddAsync(newExistingUser);
                await _db.SaveChangesAsync();
                var staffUserId = newUser.Id;


                var existinguser = await _db.Tblusers.FirstOrDefaultAsync(x => x.Userid == staffUserId);

                if (existinguser == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Staff not found";
                    return _response;
                }

                var filePath = GetFilePath();
                var filePathPassport = GetFilePathPassport();

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                if (!Directory.Exists(filePathPassport))
                {
                    Directory.CreateDirectory(filePathPassport);
                }

                string passportPath = filePathPassport + "//" + newUser.Id + ".png";

                if (System.IO.File.Exists(passportPath))
                {
                    System.IO.File.Delete(passportPath);
                }

                using (var stream = new FileStream(passportPath, FileMode.Create))
                {
                    await staffDto.Passport.CopyToAsync(stream);
                    existinguser.Passport = newUser.Id + ".png";
                }

                string userImagePath = filePath + "//" + newUser.Id + ".png";

                if (System.IO.File.Exists(userImagePath))
                {
                    System.IO.File.Delete(userImagePath);
                }
                using (var stream = new FileStream(userImagePath, FileMode.Create))
                {
                    await staffDto.Userimage.CopyToAsync(stream);
                    existinguser.Userimage = newUser.Id + ".png";
                }
                await _db.SaveChangesAsync();


                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    string updateQuery = $"ALTER TABLE tbluserpermission ADD U{newExistingUser.Userid} int";

                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);

                    int rowsAffected = await updateCommand.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        await connection.CloseAsync();
                    }

                    await connection.CloseAsync();
                }

                string htmlString = $@"
<!DOCTYPE html>
<html>
<head>
    <title>Welcome to Our CRM System</title>
</head>
<body>
    <p>Dear {newExistingUser.Username},</p>
    
    <p>Welcome to our Customer Relationship Management (CRM) system! We are thrilled to have you on board as a valuable member of our team.</p>

    <p>To access the CRM system, please find your login credentials below:</p>

    <p><strong>Username:</strong> {newExistingUser.Email}<br>
    <strong>Temporary Password:</strong> {randomPassword}</p>

    <p>For security reasons, we strongly recommend that you reset your password on your first login. Here's how you can do it:</p>

    <ol>
        <li>Visit the CRM system login page at http://crm.cparamount.com/</li>
        <li>Enter your username and temporary password provided above.</li>
        <li>You will be prompted to create a new password. Please choose a strong and unique password for your account. Make sure to follow these password guidelines:
            <ul>
                <li>At least 8 characters long</li>
                <li>Includes a mix of upper and lower case letters</li>
                <li>Contains at least one number</li>
                <li>Includes at least one special character (e.g., !, @, #, $, %, etc.)</li>
            </ul>
        </li>
    </ol>

    <p>After you have successfully changed your password, you will have full access to the CRM system, where you can manage customer interactions, track leads, and much more.</p>

    <p>If you encounter any issues during the login process or have any questions about using the CRM system, please don't hesitate to reach out to our support team at [Support Email] or [Support Phone Number]. We are here to assist you.</p>

    <p>Thank you for being a part of our team, and we look forward to achieving great results together using our CRM system.</p>

    <p>Best regards,</p>
    <p>Paramount<br>
    Password Reset<br>
    Paramount CRM<br>
    1303, Grosvenor Business Tower - Barsha Heights - Dubai<br>
    04 591 3888</p>
</body>
</html>
";




                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = "smtp.hostinger.com";
                    smtpClient.Port = 587;

                    smtpClient.Credentials = new NetworkCredential(
                        "crmtest@thinkview.click",
                        "Binance987@#"
                    );

                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("crmtest@thinkview.click"),
                        Subject = "CRM",
                        Body = htmlString,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(staffDto.Email);

                    smtpClient.Send(mailMessage);
                }

                _response.IsSuccess = true;
                _response.Message = "Successfully created " + newUser.Name;
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

        [NonAction]
        private string GetFilePath()
        {
            return _webHostEnvironment.WebRootPath + "//upload//staff";
        }

        [NonAction]
        private string GetFilePathPassport()
        {
            return _webHostEnvironment.WebRootPath + "//upload//staff//passport";
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var image = await _db.Tblusers.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            var baseDirectory = GetFilePath(); 

            if (!System.IO.File.Exists(baseDirectory))
            {
                return NotFound();
            }

            var contentType = GetContentType(image.Userimage); 

            return PhysicalFile(baseDirectory, contentType);
        }

        private string GetContentType(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName)?.ToLowerInvariant();

            switch (fileExtension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".bmp":
                    return "image/bmp";
                default:
                    return "application/octet-stream"; 
            }
        }


        [HttpPost("DeleteStaff")]
        public async Task<ResponseDto> DeleteStaff([FromBody] AuthDto authDto, [FromQuery] int id)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                //var existingStaff = await _unitOfWork.staffInterface.GetStaffByIdAsync(id);

                //if (existingStaff == null)
                //{
                //    _response.Message = "Unable to find staff with this id";
                //    _response.IsSuccess = false;
                //    return _response;
                //}

                //_db.Tblstaffs.Remove(existingStaff);

                //var existingUser = await _db.Tblusers.FirstOrDefaultAsync(x => x.Userid == id);

                //if (existingUser == null)
                //{
                //    _response.Message = "Unable to find user with this id";
                //    _response.IsSuccess = false;
                //    return _response;
                //}

                //_db.Tblusers.Remove(existingUser);

                var existingStaff = await _unitOfWork.staffInterface.GetStaffByIdAsync(id);

                if (existingStaff == null)
                {
                    _response.Message = "Unable to find staff with this id";
                    _response.IsSuccess = false;
                    return _response;
                }

                existingStaff.Status = 1;
                _db.Tblstaffs.Update(existingStaff);

                var existingUser = await _db.Tblusers.FirstOrDefaultAsync(x => x.Userid == id);

                if (existingUser == null)
                {
                    _response.Message = "Unable to find user with this id";
                    _response.IsSuccess = false;
                    return _response;
                }

                existingUser.Status = 1;
                _db.Tblusers.Update(existingUser);

                await _db.SaveChangesAsync();
                
                _response.IsSuccess = true;
                _response.Message = "Successfully deleted staff";
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while inserting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("updatestaff")]
        public async Task<ResponseDto> UpdateStaffAndUser([FromForm] StaffDto staffDto)
        {
            var newAuth = new AuthDto { Hash = staffDto.Hash };

            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(newAuth);

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
                if (staffDto == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Missing User Data";
                    return _response;
                }

                var existingstaff = await _db.Tblstaffs.FirstOrDefaultAsync(x => x.Id == staffDto.Id);

                if (existingstaff == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Staff does not exist";
                    return _response;
                }

                var getDesignation = await _db.TblDesignationtypes.FirstOrDefaultAsync(x => x.TypeId == Convert.ToInt32(staffDto.Designation));

                if (getDesignation == null)
                {
                    _response.Message = "Unable to find destination";
                    _response.IsSuccess = false;
                    return _response;
                }

                existingstaff.Name = staffDto.Name;
                existingstaff.Designation = getDesignation.TypeName;
                existingstaff.Mobileno = staffDto.Mobileno;
                existingstaff.Parentid = staffDto.Parentid;
                existingstaff.Addby = authResponse.Result.Userid;
                existingstaff.Addon = DateTime.UtcNow.Date;
                existingstaff.Status = 0;
                existingstaff.Firstname = staffDto.Firstname;
                existingstaff.Lastname = staffDto.Lastname;

                _db.Tblstaffs.Update(existingstaff);

                var existingUser = await _db.Tblusers.FirstOrDefaultAsync(x => x.Userid == existingstaff.Id);

                if (existingUser == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Staff does not exist";
                    return _response;
                }


                existingUser.Firstname = staffDto.Firstname;
                existingUser.Cid = 1;
                existingUser.Discount = 0;
                existingUser.Email = staffDto.Email;
                existingUser.Fullaccess = 0;
                existingUser.LastName = staffDto.Lastname;
                existingUser.Loginname = staffDto.Email;
                existingUser.Openbalance = 0;
                existingUser.Status = 0;
                existingUser.Username = staffDto.Email;
                existingUser.VisaIssuedate = Convert.ToDateTime(staffDto.VisaIssueDate);
                existingUser.Logintime = DateTime.UtcNow.Date;
                existingUser.Password = staffDto.Password;


                _db.Tblusers.Update(existingUser);
                await _db.SaveChangesAsync();


                if (staffDto.Passport != null || staffDto.Userimage != null)
                {
                    var filePath = GetFilePath();
                    var filePathPassport = GetFilePathPassport();

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }


                    if (!Directory.Exists(filePathPassport))
                    {
                        Directory.CreateDirectory(filePathPassport);
                    }

                    string existingpassportPath = filePathPassport + "//" + existingUser.Passport;

                    if (System.IO.File.Exists(existingpassportPath))
                    {
                        System.IO.File.Delete(existingpassportPath);
                    }

                    string passportPath = filePathPassport + "//" +existingUser.Userid+ ".png";
                    using (var stream = new FileStream(passportPath, FileMode.Create))
                    {
                        await staffDto.Passport.CopyToAsync(stream);
                        existingUser.Passport = existingUser.Userid + ".png";
                    }

                    string existinguserImagePath = filePath + "//" + existingUser.Userimage;

                    if (System.IO.File.Exists(existinguserImagePath))
                    {
                        System.IO.File.Delete(existinguserImagePath);
                    }
                    string userImagePath = filePath + "//" + existingUser.Userid +  ".png";
                    using (var stream = new FileStream(userImagePath, FileMode.Create))
                    {
                        await staffDto.Userimage.CopyToAsync(stream);
                        existingUser.Userimage = existingUser.Userid + ".png";
                    }
                }

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Successfully updated staff: " + existingstaff.Id;
                _response.Result = existingstaff;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating staff! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("getstaff")]
        public async Task<ResponseDto> Getstaff(AuthDto authDto)
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
                var Meeting = await _unitOfWork.staffInterface.GetAllVStaff();
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

        [HttpPost("getstaffNew")]
        public async Task<ResponseDto> GetstaffNew(AuthDto authDto, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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
                var Meeting = await _unitOfWork.staffInterface.GetAllVStaff();

                var totalCount = Meeting.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                var leadsPerPage = Meeting.Skip((page - 1) * pageSize).Take(pageSize).ToList();


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


        [HttpPost("GetStaffNameId")]
        public async Task<ActionResult<List<CommonDto>>> GetStaffNameAndId(AuthDto authDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                return BadRequest(authResponse.Message);
            }

            try
            {
                var staffList = _unitOfWork.staffInterface.GetAllStaffList();
                return Ok(staffList);
            }
            catch (Exception ex)
            {
                return BadRequest("Error while getting banks! " + ex.Message);
            }
        }

        [HttpGet("GetStaffCalls")]
        public async Task<ResponseDto> GetStaffCalls(int staffId, int page, int pageSize) 
        {
            try
            {
                var assignedCallsForToday = _unitOfWork.staffInterface.GetStaffAssignedCallsToday(staffId);

                var totalCount = assignedCallsForToday.Count;
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                var leadsPerPage = assignedCallsForToday.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                _response.IsSuccess = true;
                _response.Message = "";
                _response.Result = _response.Result = new
                {
                    data = leadsPerPage,
                    totalCountPages = totalPages,
                    totalData = totalCount
                };

                return _response;   
            }
            catch (Exception ex)
            {
                _response.Message = "Error while Getting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("UpdateStaffCallsStatus")]
        public async Task<ResponseDto> UpdateCallStatusStaff([FromQuery] int count, [FromQuery] string staffId)
        {
            try
            {
                var removeStaffCalls = _unitOfWork.staffInterface.RemoveAssignedCalls(Convert.ToInt32(staffId), count);

                if (removeStaffCalls == 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Unable to remove calls";
                    return _response;
                }

                _response.IsSuccess = true;
                _response.Message = $"Successfully removed {count} calls";
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

