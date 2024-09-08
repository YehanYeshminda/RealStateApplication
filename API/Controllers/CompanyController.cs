using API.Models;
using API.Repos.Dtos;
using API.Repos.Dtos.CompanyDtos;
using API.Repos.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CRMContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ResponseDto _response;
        public CompanyController(CRMContext context, IWebHostEnvironment webHostEnvironment)
        {
            _db = context;
            _webHostEnvironment = webHostEnvironment;
            _response = new ResponseDto();
        }

        [HttpPost("GetAllCompanyDetails")]
        public async Task<ResponseDto> GetAllCompanyDetails(AuthDto authDto)
        {
            if (authDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(authDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == authDto.Hash);

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
                    var existingCompanies = await _db.Tblcompanydetails.ToListAsync();
                    
                    _response.IsSuccess = true;
                    _response.Message = "";
                    _response.Result = existingCompanies;

                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while getting company! " + ex.Message;
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

        [HttpPost("CreateNewCompanyDetails")]
        public async Task<ResponseDto> CreateNewCompanyDetails(CreateCompanyDetailDto createCompanyDetailDto)
        {
            if (createCompanyDetailDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(createCompanyDetailDto.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == createCompanyDetailDto.AuthDto.Hash);

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
                    var existingCompanyWithNo = await _db.Tblcompanydetails.FirstOrDefaultAsync(x => x.Phone == createCompanyDetailDto.Phone || x.Mobile == createCompanyDetailDto.Mobile);

                    if (existingCompanyWithNo != null)
                    {
                        _response.Message = "Company with this phone number or mobile number already exists";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    var companyWithExistingEmail = await _db.Tblcompanydetails.FirstOrDefaultAsync(x => x.Email == createCompanyDetailDto.Email);

                    if (companyWithExistingEmail != null)
                    {
                        _response.Message = "Company with this email already exists";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    var companyWithExistingWebsite = await _db.Tblcompanydetails.FirstOrDefaultAsync(x => x.Website == createCompanyDetailDto.Website);

                    if (companyWithExistingWebsite != null)
                    {
                        _response.Message = "Company with this website already exists";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    var newCompany = new Tblcompanydetail
                    {
                        Address = createCompanyDetailDto.Address,
                        CompanyName = createCompanyDetailDto.CompanyName,
                        Email = createCompanyDetailDto.Email,
                        Fax = createCompanyDetailDto.Fax,
                        Mlname = createCompanyDetailDto.Mlname,
                        Mobile = createCompanyDetailDto.Mobile,
                        Phone = createCompanyDetailDto.Phone,
                        Status = createCompanyDetailDto.Status,
                        RegNo = createCompanyDetailDto.RegNo,
                        TaxMethod = createCompanyDetailDto.TaxMethod,
                        VatNo = createCompanyDetailDto.VatNo,
                        Website = createCompanyDetailDto.Website,
                    };

                    string filePath = GetFilePath();

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    string imagePath = filePath + "\\" + createCompanyDetailDto.Logo + createCompanyDetailDto.CompanyName + ".png";

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await createCompanyDetailDto.Logo.CopyToAsync(stream);
                        newCompany.Logo = createCompanyDetailDto.Logo + createCompanyDetailDto.CompanyName + ".png";
                    }

                    if (newCompany.Logo == "")
                    {
                        _response.Message = "Error while saving image";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    await _db.Tblcompanydetails.AddAsync(newCompany);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "";
                    _response.Result = newCompany;

                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while getting company! " + ex.Message;
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

        [HttpPut("EditCompanyDetails")]
        public async Task<ResponseDto> EditExistingCompanyDetails(EdtiCompanyDetailDto editCompanyDetailDto)
        {
            if (editCompanyDetailDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(editCompanyDetailDto.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == editCompanyDetailDto.AuthDto.Hash);

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
                    var existingCompanies = await _db.Tblcompanydetails.FirstOrDefaultAsync(x => x.CompanyId == editCompanyDetailDto.CompanyId);

                    if (existingCompanies == null)
                    {
                        _response.Message = "Unable to find company detials with this ID";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    existingCompanies.Address = editCompanyDetailDto.Address;
                    existingCompanies.Email = editCompanyDetailDto.Email;
                    existingCompanies.Status = editCompanyDetailDto.Status;
                    existingCompanies.TaxMethod = editCompanyDetailDto.TaxMethod;
                    existingCompanies.VatNo = editCompanyDetailDto.VatNo;
                    existingCompanies.CompanyName = editCompanyDetailDto.CompanyName;
                    existingCompanies.Mlname = editCompanyDetailDto.Mlname;
                    existingCompanies.Mobile = editCompanyDetailDto.Mobile;
                    existingCompanies.Phone = editCompanyDetailDto.Phone;
                    existingCompanies.RegNo = editCompanyDetailDto.RegNo;
                    existingCompanies.Website = editCompanyDetailDto.Website;

                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "";
                    _response.Result = existingCompanies;

                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while getting company! " + ex.Message;
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

        [HttpPost("GetSingleCompanyDetails")]
        public async Task<ResponseDto> GetSingleCompanyById([FromQuery] string id, [FromBody] AuthDto authDto)
        {
            if (authDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(authDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == authDto.Hash);

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
                    var existingCompanies = await _db.Tblcompanydetails.FirstOrDefaultAsync(x => x.CompanyId == Convert.ToInt32(id));

                    if (existingCompanies == null)
                    {
                        _response.Message = "Unable to find company with this ID";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    _response.IsSuccess = true;
                    _response.Message = "";
                    _response.Result = existingCompanies;

                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while getting company! " + ex.Message;
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

        [HttpPost("AddCompany")]
        public async Task<ResponseDto> AddNewCompany(CreateCompanyDto createCompanyDto)
        {
            if (createCompanyDto.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(createCompanyDto.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == createCompanyDto.AuthDto.Hash);

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
                    var newItem = new Tblcompany
                    {
                        Address = createCompanyDto.Address,
                        AutoBulkInvoice = createCompanyDto.AutoBulkInvoice,
                        BarcodeTitle = createCompanyDto.BarcodeTitle,
                        CashBookH = createCompanyDto.CashBookH,
                        CompanyName = createCompanyDto.CompanyName,
                        Email = createCompanyDto.Email,
                        Fax = createCompanyDto.Fax,
                        Phone = createCompanyDto.Phone,
                        ServiceCharge = createCompanyDto.ServiceCharge,
                        Status = createCompanyDto.Status,
                        Web = createCompanyDto.Web,
                    };

                    string filePath = GetFilePath();

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    string imagePath = filePath + "\\" + createCompanyDto.CompanyLogo + createCompanyDto.CompanyName + ".png";

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await createCompanyDto.CompanyLogo.CopyToAsync(stream);
                        newItem.CompanyLogo = createCompanyDto.CompanyLogo + createCompanyDto.CompanyName + ".png";
                    }

                    if (newItem.CompanyLogo == "")
                    {
                        _response.Message = "Error while saving image";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    await _db.Tblcompanies.AddAsync(newItem);
                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Company has been added successfully";
                    _response.Result = newItem;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while adding company! " + ex.Message;
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

        [HttpPost("EditCompany")]
        public async Task<ResponseDto> EditExistingCompany(EditCompanyDetails editCompanyDetails)
        {
            if (editCompanyDetails.AuthDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(editCompanyDetails.AuthDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == editCompanyDetails.AuthDto.Hash);

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
                    var existingCompany = await _db.Tblcompanies.FirstOrDefaultAsync(x => x.Id == editCompanyDetails.Id);

                    if (existingCompany == null)
                    {
                        _response.Message = "Unable to find company with this Id";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    existingCompany.Fax = editCompanyDetails.Fax;
                    existingCompany.Status = editCompanyDetails.Status;
                    existingCompany.Email = editCompanyDetails.Email;
                    existingCompany.Address = editCompanyDetails.Address;
                    existingCompany.AutoBulkInvoice = editCompanyDetails.AutoBulkInvoice;
                    existingCompany.BarcodeTitle = editCompanyDetails.BarcodeTitle;
                    existingCompany.CashBookH = editCompanyDetails.CashBookH;
                    existingCompany.CompanyName = editCompanyDetails.CompanyName;
                    existingCompany.Phone = editCompanyDetails.Phone;
                    existingCompany.ServiceCharge = editCompanyDetails.ServiceCharge;
                    existingCompany.Web = editCompanyDetails.Web;

                    string filePath = GetFilePath();

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    string imagePath = filePath + "\\" + editCompanyDetails.CompanyLogo + editCompanyDetails.CompanyName + ".png";

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await editCompanyDetails.CompanyLogo.CopyToAsync(stream);
                        existingCompany.CompanyLogo = editCompanyDetails.CompanyLogo + editCompanyDetails.CompanyName + ".png";
                    }

                    if (existingCompany.CompanyLogo == "")
                    {
                        _response.Message = "Error while saving image";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Company has been edited successfully";
                    _response.Result = existingCompany;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while editing company! " + ex.Message;
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

        [HttpGet("GetCompanyAll")]
        public async Task<ActionResult<IEnumerable<Tblcompany>>> GetExistingCompanies()
        {
            try
            {
                var existingCompany = await _db.Tblcompanies.ToListAsync();
                return existingCompany;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetCompanySingle")]
        public async Task<ResponseDto> GetExistingCompaniesByID([FromQuery] string id, [FromBody]AuthDto authDto)
        {
            if (authDto.Hash == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Please provide hash";
                return _response;
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(authDto.Hash);

            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == authDto.Hash);

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
                    var existingCompany = await _db.Tblcompanies.FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(id));

                    if (existingCompany == null)
                    {
                        _response.Message = "Unable to find company with this Id";
                        _response.IsSuccess = false;
                        return _response;
                    }

                    _response.IsSuccess = true;
                    _response.Message = "Companies has been retrived successfully";
                    _response.Result = existingCompany;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while getting single company! " + ex.Message;
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
            return _webHostEnvironment.WebRootPath + "\\upload\\company";
        }
    }
}
