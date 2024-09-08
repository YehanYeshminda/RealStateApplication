using System.Data;
using API.Models;
using API.Repos.Dtos;
using API.Repos.FileUpload;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;

namespace API.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ResponseDto _response;
        private readonly CRMContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public FileUploadController(CRMContext context, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _response = new ResponseDto();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _db = context;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public class FormDataValues
        {
            public IFormFile File { get; set; }
            public string PhoneNoCol { get; set; }
            public string EmailCol { get; set; }
            public string CustomerName { get; set; }
        }

        [HttpPost("excel")]
        public async Task<ResponseDto> UploadExcelFile([FromForm] FormDataValues formDataValues)
        {
            try
            {
                if (formDataValues.File == null || formDataValues.File.Length <= 0)
                {
                    _response.IsSuccess = false;
                    _response.Message = "No file uploaded.";
                    return _response;
                }

                using (var package = new ExcelPackage(formDataValues.File.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    if (rowCount < 2)
                    {
                        _response.IsSuccess = false;
                        _response.Message = "The Excel file should have at least 2 rows (header and data).";
                        return _response;
                    }

                    var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns];
                    var columnNames = headerRow.Select(cell => cell.Text.ToLower()).ToList();

                    if (!columnNames.Contains(formDataValues.PhoneNoCol.ToLower()))
                    {
                        _response.IsSuccess = false;
                        _response.Message = "The Excel file must contain columns with the name Phone Number.";
                        return _response;
                    }

                    var excelData = new List<List<string>>();
                    int phoneNoColIndex = columnNames.IndexOf(formDataValues.PhoneNoCol.ToLower());

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var rowData = new List<string>();
                        for (int col = 1; col <= columnNames.Count; col++)
                        {
                            var cellValue = worksheet.Cells[row, col].Value?.ToString();
                            rowData.Add(cellValue);
                        }

                        if (rowData.All(string.IsNullOrEmpty))
                        {
                            continue;
                        }

                        if (phoneNoColIndex >= 0 && phoneNoColIndex < rowData.Count &&
                            !string.IsNullOrEmpty(rowData[phoneNoColIndex]) && rowData[phoneNoColIndex].ToLower() != "null")
                        {
                            excelData.Add(rowData);
                        }
                    }
                    
                    DataTable dt = new DataTable("tblCallInsights");
                    dt.Columns.Add("Id", typeof(int));
                    dt.Columns.Add("FirstName", typeof(string));
                    dt.Columns.Add("LastName", typeof(string));
                    dt.Columns.Add("Email", typeof(string));
                    dt.Columns.Add("PhoneNo", typeof(string));
                    dt.Columns.Add("PhoneNo2", typeof(string));
                    dt.Columns.Add("AssignedTo", typeof(string));
                    dt.Columns.Add("AddOn", typeof(DateTime));
                    dt.Columns.Add("calledOn", typeof(DateTime));
                    dt.Columns.Add("callEndedOn", typeof(DateTime));
                    dt.Columns.Add("Status", typeof(int));
                    dt.Columns.Add("AssignedOn", typeof(DateTime));

                    foreach (var row in excelData)
                    {
                        var name = row.Count > columnNames.IndexOf(formDataValues.CustomerName.ToLower()) ? row[columnNames.IndexOf(formDataValues.CustomerName.ToLower())] : string.Empty;
                        var emailIndex = columnNames.IndexOf(formDataValues.EmailCol.ToLower());
                        var email = emailIndex != -1 && row.Count > emailIndex ? row[emailIndex] : string.Empty;
                        var phone = row.Count > phoneNoColIndex ? row[phoneNoColIndex] : string.Empty;

                        name = name?.ToLower();
                        email = email?.ToLower();
                        phone = phone?.ToLower();

                        var nameParts = name?.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                        var firstName = nameParts?.FirstOrDefault();
                        var lastName = string.Join(" ", nameParts?.Skip(1));
                        
                        
                        DateTime utcNow = DateTime.UtcNow;
                        TimeZoneInfo dubaiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");

                        DateTime dubaiTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, dubaiTimeZone);
                        
                        DataRow newRow = dt.NewRow();
                        newRow["Id"] = 0;
                        newRow["FirstName"] = firstName;
                        newRow["LastName"] = lastName;
                        newRow["Email"] = email;
                        newRow["PhoneNo"] =  phone;
                        newRow["PhoneNo2"] = " ";
                        newRow["AssignedTo"] = "0";
                        newRow["AddOn"] = dubaiTime;
                        newRow["calledOn"] = DBNull.Value;
                        newRow["callEndedOn"] = DBNull.Value;
                        newRow["Status"] = 0;
                        newRow["AssignedOn"] = DBNull.Value;
                        dt.Rows.Add(newRow);
                    }
                    FileUploadService fileUploadService = new FileUploadService(_configuration);
                    
                    if (fileUploadService.InsertBulkContactData(dt) == -1)
                    {
                        fileUploadService.RemoveDuplicateRecordsBasedOnNumber();
                        fileUploadService.RemoveDuplicateRecordsBasedOnEmail();
                    }

                    _response.Message = "Upload files successfully";
                    _response.IsSuccess = true;
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.Message = "Error while Getting notifications! " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }


        [HttpPost("excel/columns")]
        public async Task<ResponseDto> GetExcelColumns(IFormFile file)
        {
            try
            {
                if (file == null || file.Length <= 0)
                {
                    _response.Message = "No file has been uploaded";
                    _response.IsSuccess = false;
                    return _response;
                }

                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var headerRow = worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns];
                    var columnNames = headerRow.Select(cell => cell.Text).ToList();

                    _response.Result = columnNames;
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Error while retrieving column names: " + ex.Message;
                return _response;
            }
        }

        [HttpPost("excel/columndata")]
        public async Task<ResponseDto> GetExcelData(IFormFile file)
        {
            try
            {
                if (file == null || file.Length <= 0)
                {
                    _response.Message = "No file has been uploaded";
                    _response.IsSuccess = false;
                    return _response;
                }

                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    var rowData = worksheet.Cells[2, 1, 2, worksheet.Dimension.Columns]
                        .Select(cell => cell.Text).ToList();

                    _response.Result = rowData;
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Error while retrieving data: " + ex.Message;
                return _response;
            }
        }


        [HttpGet("crmColumns")]
        public async Task<ResponseDto> GetCRMColumns()
        {

            try
            {
                List<string> columnNames = new List<string>();

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    string sqlQuery = @"
                    SELECT COLUMN_NAME
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_SCHEMA = 'dbo'
                    AND TABLE_NAME = 'tblCallInsights'
                    AND COLUMN_NAME <> 'Id'";

                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string columnName = reader["COLUMN_NAME"].ToString();
                        columnNames.Add(columnName);
                    }
                    _response.Result = columnNames;
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Error while retrieving data: " + ex.Message;
                return _response;
            }

        }


    }
}
