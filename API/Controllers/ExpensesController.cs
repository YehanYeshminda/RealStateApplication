using API.Models;
using API.Repos;
using API.Repos.Dtos;
using API.Repos.Dtos.AdvPaymentDtos;
using API.Repos.Dtos.CommonDto;
using API.Repos.Dtos.ExpensesDtos;
using API.Repos.Dtos.StaffDtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos.Helpers;
using API.Repos.Interfaces;
using API.Repos.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace API.Controllers
{
    [Route("api/Expenses")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly GlobalDataService _globalDataService;
        private readonly IConfiguration _configuration;

        public ExpensesController(IConfiguration configuration, CRMContext db, IUnitOfWork unitOfWork, GlobalDataService globalDataService)
        {
            _response = new ResponseDto();
            _db = db;
            _unitOfWork = unitOfWork;
            _globalDataService = globalDataService;
            _configuration = configuration;
        }

        [HttpGet("report")]
        public async Task<ActionResult<TblvExpense>> ReturnHtmlReport()
        {
            var tableForReport = await _db.TblvExpenses.ToListAsync();

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
                                <th class='border' colspan=""20"" style=""text-align: center;"">Expense Data</th>
                            </tr>
                            <tr>
                                <th class='border'>Id</th>
                                <th class='border'>VDate</th>
                                <th class='border'>Supplier</th>
                                <th class='border'>MainCat</th>
                                <th class='border'>Subcat</th>
                                <th class='border'>Description</th>
                                <th class='border'>CashPaid</th>
                                <th class='border'>ChequePaid</th>
                                <th class='border'>ChequeNo</th>
                                <th class='border'>AuthBy</th>
                                <th class='border'>ReceiptNo</th>
                                <th class='border'>Account</th>
                                <th class='border'>RDate</th>
                                <th class='border'>Branch</th>
                                <th class='border'>TotalValue</th>
                                <th class='border'>Vatp</th>
                                <th class='border'>NetTotal</th>
                                <th class='border'>Paid</th>
                            </tr>";


            foreach (var items in tableForReport)
            {

                string vDate = Convert.ToDateTime(items.VDate).ToString("yyyy-MM-dd");
                string rDate = Convert.ToDateTime(items.RDate).ToString("yyyy-MM-dd");

                html += $@"
                   <tr>
                        <td class='border'>{items.Id}</td>
                        <td class='border'>{vDate}</td>
                        <td class='border'>{items.SupplierName}</td>
                        <td class='border'>{items.MainCategory}</td>
                        <td class='border'>{items.SubCategory}</td>
                        <td class='border'>{items.Description}</td>
                        <td class='border'>{items.CashPaid}</td>
                        <td class='border'>{items.ChequePaid}</td>
                        <td class='border'>{items.ChequeNo}</td>
                        <td class='border'>{items.Username}</td>
                        <td class='border'>{items.ReceiptNo}</td>
                        <td class='border'>{items.AccountId}</td>
                        <td class='border'>{rDate}</td>
                        <td class='border'>{items.BranchName}</td>
                        <td class='border'>{items.TotalValue}</td>
                        <td class='border'>{items.Vatp}</td>
                        <td class='border'>{items.NetTotal}</td>
                        <td class='border'>{items.Paid}</td>
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
        public async Task<ActionResult<TblvExpense>> ReturnHtmlCellReport(string id)
        {
            var tableForReport = await _db.TblvExpenses
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
                string vDate = Convert.ToDateTime(items.VDate).ToString("yyyy-MM-dd");
                string rDate = Convert.ToDateTime(items.RDate).ToString("yyyy-MM-dd");

                html += $@"
             
                            <tr>
                                <th colspan=""2"" style=""text-align: center;"">Expense Data</th>
                            </tr> 

                                   <tr>
                                        <td>No</td>
                                        <td>{items.Id}</td>
                                   </tr>
                                   <tr>
                                        <td>Date</td>
                                        <td>{vDate}</td>
                                   </tr>
                                   <tr>
                                        <td>Supplier</td>
                                        <td>{items.SupplierName}</td>
                                   </tr>
                                   <tr>
                                        <td>Main Category</td>
                                        <td>{items.MainCategory}</td>
                                   </tr>
                                   <tr>
                                        <td>Sub Category</td>
                                        <td>{items.SubCategory}</td>
                                   </tr>
                                   <tr>
                                        <td>Description</td>
                                        <td>{items.Description}</td>
                                   </tr>
                                   <tr>
                                        <td>Cash Paid</td>
                                        <td>{items.CashPaid}</td>
                                   </tr>
                                   <tr>
                                        <td>Cheque Paid</td>
                                        <td>{items.ChequePaid}</td>
                                   </tr>
                                   <tr>
                                        <td>Cheque No</td>
                                        <td>{items.ChequeNo}</td>
                                   </tr>
                                   <tr>
                                        <td>Autherized By</td>
                                        <td>{items.Username}</td>
                                   </tr>
                                   <tr>
                                        <td>Receipt No</td>
                                        <td>{items.ReceiptNo}</td>
                                   </tr>
                                   <tr>
                                        <td>Request Date</td>
                                        <td>{rDate}</td>
                                   </tr>
                                   <tr>
                                        <td>Branch</td>
                                        <td>{items.BranchName}</td>
                                   </tr>
                                   <tr>
                                        <td>Total Value</td>
                                        <td>{items.TotalValue}</td>
                                   </tr>
                                   <tr>
                                        <td>Vat Amount</td>
                                        <td>{items.Vat}</td>
                                   </tr>
                                   <tr>
                                        <td>Vat Paid</td>
                                        <td>{items.Vatp}</td>
                                   </tr>
                                   <tr>
                                        <td>Net Total</td>
                                        <td>{items.NetTotal}</td>
                                   </tr>
                                   <tr>
                                        <td>Paid</td>
                                        <td>{items.Paid}</td>
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

        [HttpPost("getexpense")]
        public async Task<ResponseDto> Getexpense(AuthDto authDto)
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
                Location = AccessLocation.Expenses.ToString()
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
                var Meeting = await _unitOfWork.expenseInterface.GetAllVExpense();
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

        [HttpPost("insertexpense")]
        public async Task<ResponseDto> Insert(ExpensesDto expenseDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(expenseDto.AuthDto);

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
                Location = AccessLocation.Expenses.ToString()
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
                    if (expenseDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingexpense = await _db.Tblexpenses.FirstOrDefaultAsync(x => x.Id == expenseDto.Id);

                    if (existingexpense != null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Invalid Id";
                        return _response;
                    }

                    var newUser = new Tblexpense
                    {
                        Id = expenseDto.Id,
                        VDate = expenseDto.VDate,
                        SupplierId = expenseDto.SupplierId,
                        MainCatId = expenseDto.MainCatId,
                        SubcatId = expenseDto.SubcatId,
                        Description = expenseDto.Description,
                        CashPaid = expenseDto.CashPaid,
                        ChequePaid = expenseDto.ChequePaid,
                        ChequeNo = expenseDto.ChequeNo,
                        Cid = _globalDataService.CId.ToString(),
                        Status = expenseDto.Status,
                        AuthBy = expenseDto.AuthBy,
                        ReceiptNo = expenseDto.ReceiptNo,
                        AccountId = expenseDto.AccountId,
                        UserId = authResponse.Result.Userid,
                        RDate = expenseDto.RDate,
                        UniqueId = expenseDto.UniqueId,
                        BrId = expenseDto.BrId,
                        TotalValue = expenseDto.TotalValue,
                        Vatp = expenseDto.Vatp,
                        Vat = expenseDto.Vat,
                        NetTotal = expenseDto.NetTotal,
                        Paid = expenseDto.Paid
                    };

                    await _db.Tblexpenses.AddAsync(newUser);
                    var branchControl = await _db.TblBranchControls.FirstOrDefaultAsync();
                    if (branchControl != null)
                    {
                        
                        branchControl.VoucherNo += 1;
                        _db.TblBranchControls.Update(branchControl);
                        
                    }

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


        [HttpPost("updateexpense")]
        public async Task<ResponseDto> Updateexpense(ExpensesDto expenseDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(expenseDto.AuthDto);

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
                Location = AccessLocation.Expenses.ToString()
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
                    if (expenseDto == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Missing User Data";
                        return _response;
                    }

                    var existingexpense = await _db.Tblexpenses.FirstOrDefaultAsync(x => x.Id == expenseDto.Id);

                    if (existingexpense == null)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Id not found";
                        return _response;
                    }
                    existingexpense.Id = expenseDto.Id;
                    existingexpense.VDate = expenseDto.VDate;
                    existingexpense.SupplierId = expenseDto.SupplierId;
                    existingexpense.MainCatId = expenseDto.MainCatId;
                    existingexpense.SubcatId = expenseDto.SubcatId;
                    existingexpense.Description = expenseDto.Description;
                    existingexpense.CashPaid = expenseDto.CashPaid;
                    existingexpense.ChequePaid = expenseDto.ChequePaid;
                    existingexpense.ChequeNo = expenseDto.ChequeNo;
                    existingexpense.Cid = _globalDataService.CId.ToString();
                    existingexpense.Status = expenseDto.Status;
                    existingexpense.AuthBy = expenseDto.AuthBy;
                    existingexpense.ReceiptNo = expenseDto.ReceiptNo;
                    existingexpense.AccountId = expenseDto.AccountId;
                    existingexpense.UserId = authResponse.Result.Userid;
                    existingexpense.RDate = expenseDto.RDate;
                    existingexpense.UniqueId = expenseDto.UniqueId;
                    existingexpense.BrId = expenseDto.BrId;
                    existingexpense.TotalValue = expenseDto.TotalValue;
                    existingexpense.Vatp = expenseDto.Vatp;
                    existingexpense.Vat = expenseDto.Vat;
                    existingexpense.NetTotal = expenseDto.NetTotal;
                    existingexpense.Paid = expenseDto.Paid;

                    await _db.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Successfully updated expense: " + existingexpense.Id;
                    _response.Result = existingexpense;
                    return _response;
                }
                catch (Exception ex)
                {
                    _response.Message = "Error while updating expense! " + ex.Message;
                    _response.IsSuccess = false;
                    return _response;
                }
        }

        [HttpPost("AddItemAll")]
        public async Task<IActionResult> AddItems(DynamicExpenseDto dynamicFormDto)
        {
            if (dynamicFormDto.AuthDto == null)
            {
                return BadRequest("Please provide Authentication Data");
            }

            HelperAuth decodedValues = AuthValidator.DecodeValue(dynamicFormDto.AuthDto.Hash);
            var _user = _db.Tblusers.SingleOrDefault(x => x.Userid == decodedValues.UserId && x.Hash == dynamicFormDto.AuthDto.Hash);
            if (_user == null)
            {
                return Unauthorized("Invalid User or Login again!");
            }

            var decryptedDateWithOffset = decodedValues.Date.AddDays(1);
            var currentDate = DateTime.UtcNow.Date;
            if (currentDate < decryptedDateWithOffset.Date)
            {
                try
                {
                    if (dynamicFormDto.DynamicField == "ExpAccount")
                    {
                        var existing = await _db.Tblexpensesaccounts.FirstOrDefaultAsync(x => x.Id == dynamicFormDto.Id);

                        if (existing != null)
                        {
                            return BadRequest("Item with this id already exist");
                        }

                        var existingdata = await _db.Tblexpensesaccounts.FirstOrDefaultAsync(x => x.MainCatId == dynamicFormDto.MainCatId &&  x.SubCatId == dynamicFormDto.SubCatId);
                        if (existingdata != null)
                        {
                            return BadRequest("Item with this combination already exist");
                        }

                        var newItemType = new Tblexpensesaccount
                        {
                            MainCatId = dynamicFormDto.MainCatId,
                            SubCatId = dynamicFormDto.SubCatId,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        return Ok(newItemType);
                    }


                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest("Error occured while adding items. " + ex.Message);
                }
            }
            else
            {
                return Unauthorized("Invalid User or Login again!");
            }
        }

        [HttpGet("{table}")]
        public async Task<ActionResult<List<CustomExpenseTableDto>>> GetCustomTableData(string table)
        {
            if (string.IsNullOrEmpty(table))
            {
                return BadRequest("Table is required.");
            }

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand($"SELECT * FROM {table}", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                var tableData = new List<CustomExpenseTableDto>();

                                var columnCount = reader.FieldCount;

                                while (await reader.ReadAsync())
                                {
                                    var data = new CustomExpenseTableDto();

                                    for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                                    {
                                        var value = reader.GetValue(columnIndex);
                                        if (value != DBNull.Value)
                                        {
                                            switch (columnIndex)
                                            {
                                                case 0:
                                                    data.Id = Convert.ToInt32(value);
                                                    break;
                                                case 1:
                                                    data.MainCategory = value.ToString();
                                                    break;
                                                case 2:
                                                    data.SubCategory = value.ToString();
                                                    break;
                                                case 3:
                                                    data.Status = Convert.ToInt32(value);
                                                    break;
                                            }
                                        }
                                    }

                                    tableData.Add(data);
                                }

                                return tableData;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }

            return new List<CustomExpenseTableDto>();
        }

        [HttpPost("viewVExpenseById")]
        public async Task<ActionResult<GetTblExpenseAccountDto>> ViewVExpenseId([FromQuery] int id)
        {


            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                SqlCommand getvoucherById = new SqlCommand("SELECT * FROM vExepnsesAccount WHERE Id = @Id", connection);
                getvoucherById.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = await getvoucherById.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    if (await reader.ReadAsync())
                    {
                        GetTblExpenseAccountDto expenseAccount = new GetTblExpenseAccountDto
                        {
                            MainCatId = (string)reader["MainCatId"],
                            Id = (int)reader["Id"],
                            SubCatId = (string)reader["SubCatId"],
                            Status = (int)reader["Status"],
                            MainCatergory = (string)reader["MainCategory"],
                            SubCatergory = (string)reader["SubCategory"]
                        };
                        await connection.CloseAsync();
                        return Ok(expenseAccount);
                    }

                    await connection.CloseAsync();
                    return NotFound();
                }
            }
        }

    }
}
