using API.Models;
using API.Repos.Dtos;
using API.Repos.Dtos.AccountDtos;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using API.Repos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly GlobalDataService _globalDataService;
        private readonly IUnitOfWork _iunitofwork;

        public AccountController(CRMContext db, IWebHostEnvironment webHostEnvironment, GlobalDataService globalDataService, IUnitOfWork iunitofwork)
        {
            _response = new ResponseDto();
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _globalDataService = globalDataService;
            _iunitofwork = iunitofwork;
        }


        [HttpPost("Login")]
        public async Task<ResponseDto> LoginUser(LoginUserDto loginUserDto)
        {
            try
            {
                var existingUser =
                    await _iunitofwork.userInterface.GetExistingUserByUsernamePasswordStatus(loginUserDto.Username,
                        loginUserDto.Password);

                if (existingUser == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid username or password";
                    return _response;
                }

                var existingBranch =
                    await _iunitofwork.BranchService.GetExistingBranchByBranchId(loginUserDto.BranchId);

                if (existingBranch == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid branch with this id";
                    return _response;
                }

                var existingCompany = await _iunitofwork.companyInterface.GetCompanyByIdAsync(existingBranch.Cid);

                if (existingCompany == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid company with this id";
                    return _response;
                }

                string encodedValue = EncodeValue(existingUser.Userid);

                existingUser.Hash = encodedValue;
                existingUser.Logintime = DateTime.Now;
                existingUser.Ud = existingUser.Ud + 1;

                var itemsToReturn = new LoginResponseDto { Hash = existingUser.Hash };

                _db.Tblusers.Update(existingUser);
                await _db.SaveChangesAsync();

                _globalDataService.CId = existingCompany.Id;
                _globalDataService.BrId = existingBranch.BrId;
                _globalDataService.UserId = existingUser.Userid;

                _response.IsSuccess = true;
                _response.Message = "Login Success";
                _response.Result = itemsToReturn;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while login! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [NonAction]
        private string GetFilePath()
        {
            return _webHostEnvironment.WebRootPath + "\\upload\\user";
        }

        [HttpPost("Register")]
        public async Task<ResponseDto> RegisterUser(RegisterUserDto registerUserDto)
        {
            if (registerUserDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(registerUserDto.AuthDto.Hash);

            var _user = _db.Tblusers.FirstOrDefaultAsync(x => x.Userid == decodedValues.UserId && x.Hash == registerUserDto.AuthDto.Hash);

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
                    if (registerUserDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingUserWithUsername = await _db.Tblusers.FirstOrDefaultAsync(x => x.Loginname == registerUserDto.LoginName || x.Username == registerUserDto.LoginName || x.Username == registerUserDto.UserName);

                    if (existingUserWithUsername != null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "User with this loginname or username already exists";
                        return _response;
                    }

                    if (registerUserDto.UserCode.Length > 2)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Usercode can only contain 2 digits";
                        return _response;
                    }

                    var existingCompany = await _db.Tblcompanies.FirstOrDefaultAsync(x => x.Id == registerUserDto.Cid);

                    if (existingCompany == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Invalid company Id";
                        return _response;
                    }

                    var newUser = new Tbluser
                    {
                        Username = registerUserDto.UserName,
                        Loginname = registerUserDto.LoginName,
                        Logintime = DateTime.UtcNow,
                        Logouttime = DateTime.UtcNow,
                        Fullaccess = registerUserDto.FullAccess,
                        Superuser = registerUserDto.SuperUser,
                        Password = registerUserDto.Password,
                        Cid = registerUserDto.Cid,
                        Status = registerUserDto.Status,
                        Openbalance = 0,
                        Discount = 0,
                        Ud = 0,
                        Usercode = registerUserDto.UserCode,
                    };

                    string filePath = GetFilePath();

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    string imagePath = filePath + "\\" + registerUserDto.UserLogo + registerUserDto.UserName + ".png";

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await registerUserDto.UserLogo.CopyToAsync(stream);
                        newUser.Userimage = registerUserDto.UserLogo + registerUserDto.UserName + ".png";
                    }

                    if (newUser.Userimage == "")
                    {
                        _response.Message = "Error while saving image";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    await _db.Tblusers.AddAsync(newUser);
                    await _db.SaveChangesAsync();

                    string encodedValue = EncodeValue(newUser.Userid);

                    _response.IsSuccess = true;
                    _response.Message = "Successfully created user with the username " + newUser.Username;
                    _response.Result = newUser;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while registering user! " + ex.Message;
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

        private string EncodeValue(int userId)
        {
            string datetimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            string valueToEncode = $"{userId}!{datetimeString}";

            byte[] bytes = Encoding.UTF8.GetBytes(valueToEncode);
            string encodedValue = Convert.ToBase64String(bytes);

            return encodedValue;
        }


        [HttpPost("Getloginusername")]
        public async Task<ResponseDto> Getname(AuthDto authDto)
        {
            var authResponse = _iunitofwork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var user = await _iunitofwork.accountInterface.Getloginuser(authResponse.Result.Userid);

                _response.Result = user;
                _response.IsSuccess = true;
                return _response;
            }

            catch (Exception ex)
            {
                _response.Message = "Error while Getting! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        public class PasswordResetDto
        {
            public AuthDto authDto { get; set; }
            public string OldPassword { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("UpdateCancel")]
        public async Task<ResponseDto> PasswordReset([FromBody] AuthDto authDto)
        {
            var authResponse = _iunitofwork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var existingUser = await _db.Tblusers.FirstOrDefaultAsync(x => x.Userid == authResponse.Result.Userid);

                if (existingUser == null)
                {
                    _response.Message = "User with this information does not exist";
                    _response.IsSuccess = false;
                    return _response;
                };

                existingUser.Ud = existingUser.Ud + 1;

                _db.Tblusers.Update(existingUser);

                await _db.SaveChangesAsync();

                _response.Message = "Successfully updated user password";
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        [HttpPost("PasswordReset")]
        public async Task<ResponseDto> PasswordReset([FromBody] PasswordResetDto passwordResetDto)
        {
            var authResponse = _iunitofwork.authenticationService.ValidateAuthentication(passwordResetDto.authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var existingUser = await _db.Tblusers.FirstOrDefaultAsync(x => x.Userid == authResponse.Result.Userid);

                if (existingUser == null)
                {
                    _response.Message = "User with this information does not exist";
                    _response.IsSuccess = false;
                    return _response;
                };

                if (existingUser.Password != passwordResetDto.OldPassword)
                {
                    _response.Message = "Old password is incorrect";
                    _response.IsSuccess = false;
                    return _response;
                }

                existingUser.Password = passwordResetDto.Password;
                existingUser.Ud = existingUser.Ud + 1;

                _db.Tblusers.Update(existingUser);

                await _db.SaveChangesAsync();

                _response.Message = "Successfully updated user password";
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }

        [HttpPost("LoginFirst")]
        public async Task<ResponseDto> CheckIfFirstLogin([FromBody] AuthDto authDto)
        {
            var authResponse = _iunitofwork.authenticationService.ValidateAuthentication(authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                var existingUser = await _db.Tblusers.FirstOrDefaultAsync(x => x.Userid == authResponse.Result.Userid);

                if (existingUser == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "User with this information does not exist";
                    return _response;
                }

                if (existingUser.Ud == 1)
                {
                    _response.Result = "1";
                    _response.IsSuccess = true;
                    return _response;
                }


                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }
    }
}
