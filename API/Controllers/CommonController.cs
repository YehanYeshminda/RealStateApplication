using API.Models;
using API.Repos.Dtos;
using API.Repos.Dtos.CommonDto;
using API.Repos.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/common")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CRMContext _db;
        private readonly GlobalDataService _globalDataService;
        public CommonController(IConfiguration configuration, CRMContext db, GlobalDataService globalDataService)
        {
            _configuration = configuration;
            _db = db;
            _globalDataService = globalDataService;
        }

        [HttpGet("LeadLastValueWithPrefix")]
        public async Task<ActionResult<object>> GetGrnLastValue(string prefix, string tableName, string columnName)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                string query = $"SELECT TOP 1 {columnName} FROM {tableName} ORDER BY {columnName} DESC";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            string lastValue = "0";
                            while (await reader.ReadAsync())
                            {
                                lastValue = reader[columnName].ToString();
                                break;
                            }

                            string numericPart = Regex.Match(lastValue, @"\d+").Value;

                            if (numericPart == "")
                            {
                                lastValue = "0";
                                numericPart = "0";
                            }

                            if (int.TryParse(numericPart, out int lastNumber))
                            {
                                lastNumber++;
                                string formattedValue = prefix + lastNumber.ToString("D2");
                                return Ok(new { lastValue = formattedValue });
                            }
                            else
                            {
                                return BadRequest("Invalid format for the last value.");
                            }
                        }
                        else
                        {
                            return Ok(new { lastValue = prefix + "01" });
                        }
                    }
                }
            }
        }

        [HttpGet("comboData")]
        public async Task<ActionResult<List<CommonDto>>> GetComboIds(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query is required.");
            }

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(query, connection))
                    {
                        var comboItems = new List<CommonDto>();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                var columnNames = Enumerable.Range(0, reader.FieldCount)
                                    .Select(i => reader.GetName(i))
                                    .ToList();

                                while (await reader.ReadAsync())
                                {
                                    var id = reader["_Id"];
                                    var value = reader["_Value"];
                                    comboItems.Add(new CommonDto { value = Convert.ToInt32(id), textValue = value.ToString() });
                                }
                            }
                        }

                        return comboItems;
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("comboDataString")]
        public async Task<ActionResult<List<stringcombobox>>> GetComboStringIds(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query is required.");
            }

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand(query, connection))
                    {
                        var comboItems = new List<stringcombobox>();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                var columnNames = Enumerable.Range(0, reader.FieldCount)
                                    .Select(i => reader.GetName(i))
                                    .ToList();

                                while (await reader.ReadAsync())
                                {
                                    var id = reader["_Id"];
                                    var value = reader["_Value"];
                                    comboItems.Add(new stringcombobox { value = id.ToString(), textValue = value.ToString() });
                                }
                            }
                        }

                        return comboItems;
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{table}")]
        public async Task<ActionResult<List<CustomRetrievalTableDto>>> GetCustomTableData(string table)
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
                                var tableData = new List<CustomRetrievalTableDto>();

                                var columnCount = reader.FieldCount;

                                while (await reader.ReadAsync())
                                {
                                    var data = new CustomRetrievalTableDto();

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
                                                    data.DynamicField = value.ToString();
                                                    break;
                                                case 2:
                                                    data.Remark = value.ToString();
                                                    break;
                                                case 3:
                                                    data.Status = Convert.ToInt32(value);
                                                    break;
                                                case 4:
                                                    data.CId = Convert.ToInt32(value);
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

            return new List<CustomRetrievalTableDto>();
        }


        [HttpPost("AddItemAll")]
        public async Task<IActionResult> AddItems(DynamicFormDto dynamicFormDto)
        {

                try
                {
                    if (dynamicFormDto.DynamicField == "Service")
                    {
                        var existing = await _db.TblServicetypes.FirstOrDefaultAsync(x => x.TypeName == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }
                        
                        var newItemType = new TblServicetype
                        {
                            TypeName = dynamicFormDto.CatergoryName,
                            Cid = _globalDataService.CId,
                            Remark = dynamicFormDto.Remark,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        return Ok(newItemType);
                    }

                    else if (dynamicFormDto.DynamicField == "PriorityType")
                    {
                        var existing = await _db.Tblprioritytypes.FirstOrDefaultAsync(x => x.TypeName == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }

                        var newItemType = new Tblprioritytype
                        {
                            TypeName = dynamicFormDto.CatergoryName,
                            Cid = _globalDataService.CId,
                            Remark = dynamicFormDto.Remark,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        return Ok(newItemType);
                    }
                    else if (dynamicFormDto.DynamicField == "IssuedTo")
                    {
                        var existing = await _db.TblIssuedTos.FirstOrDefaultAsync(x => x.TypeName == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }

                        var newItemType = new TblIssuedTo
                        {
                            TypeName = dynamicFormDto.CatergoryName,
                            Cid = _globalDataService.CId,
                            Remark = dynamicFormDto.Remark,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        return Ok(newItemType);
                    }
                    else if (dynamicFormDto.DynamicField == "AgreeRem")
                    {
                        var existing = await _db.TblAgreementtypes.FirstOrDefaultAsync(x => x.TypeName == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }

                        var newItemType = new TblAgreementtype
                        {
                            TypeName = dynamicFormDto.CatergoryName,
                            Cid = _globalDataService.CId,
                            Remark = dynamicFormDto.Remark,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        return Ok(newItemType);
                    }
                    else if (dynamicFormDto.DynamicField == "Media")
                    {
                        var existing = await _db.TblMedia.FirstOrDefaultAsync(x => x.Media == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }

                        var newItemType = new TblMedium
                        {
                            Media = dynamicFormDto.CatergoryName,
                            Cid = _globalDataService.CId,
                            Remark = dynamicFormDto.Remark,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        return Ok(newItemType);
                    }
                    else if (dynamicFormDto.DynamicField == "MainCat")
                    {
                        var existing = await _db.Tblemaincats.FirstOrDefaultAsync(x => x.MainCategory == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }

                        var newItemType = new Tblemaincat
                        {
                            MainCategory = dynamicFormDto.CatergoryName,
                            Cid = _globalDataService.CId,
                            Remark = dynamicFormDto.Remark,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        return Ok(newItemType);
                    }
                    else if (dynamicFormDto.DynamicField == "RFPT")
                    {
                        var existing = await _db.TblRsvpTypes.FirstOrDefaultAsync(x => x.TypeName == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }

                        var newItemType = new TblRsvpType
                        {
                            TypeName = dynamicFormDto.CatergoryName,
                            Cid = _globalDataService.CId,
                            Remark = dynamicFormDto.Remark,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        return Ok(newItemType);
                    }
                    
                    else if (dynamicFormDto.DynamicField == "LeadStatus")
                    {
                        var existing = await _db.TblLeadStatuses.FirstOrDefaultAsync(x => x.Leadstatus == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }

                        var newItemType = new TblLeadStatus
                        {
                            Leadstatus = dynamicFormDto.CatergoryName,
                            Remark = dynamicFormDto.Remark,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        return Ok(newItemType);
                    }

                    else if (dynamicFormDto.DynamicField == "SubCat")
                    {
                        var existing = await _db.Tblesubcats.FirstOrDefaultAsync(x => x.SubCategory == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }

                        var newItemType = new Tblesubcat
                        {
                            SubCategory = dynamicFormDto.CatergoryName,
                            Cid = _globalDataService.CId,
                            Remark = dynamicFormDto.Remark,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        return Ok(newItemType);
                    }
                    
                    else if (dynamicFormDto.DynamicField == "PlanToDos")
                    {
                        var existing = await _db.TblPlanToDos.FirstOrDefaultAsync(x => x.TypeName == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }

                        var newItemType = new TblPlanToDo
                        {
                            TypeName = dynamicFormDto.CatergoryName,
                            Remark = dynamicFormDto.Remark,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        return Ok(newItemType);
                    }

                    else if (dynamicFormDto.DynamicField == "Salesby")
                    {
                        var existing = await _db.TblSales.FirstOrDefaultAsync(x => x.TypeName == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }

                        var newItemType = new TblSale
                        {
                            TypeName = dynamicFormDto.CatergoryName,
                            Cid = _globalDataService.CId,
                            Remark = dynamicFormDto.Remark,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        return Ok(newItemType);
                    }

                    else if (dynamicFormDto.DynamicField == "Source")
                    {
                        var existing = await _db.Tblsources.FirstOrDefaultAsync(x => x.Source == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }

                        var newItemType = new Tblsource
                        {
                            Source = dynamicFormDto.CatergoryName,
                            Cid = _globalDataService.CId.ToString(),
                            Remark = dynamicFormDto.Remark,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        return Ok(newItemType);
                    }

                    else if (dynamicFormDto.DynamicField == "City")
                    {
                        var existing = await _db.TblCitytypes.FirstOrDefaultAsync(x => x.TypeName == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }

                        var newItemType = new TblCitytype
                        {
                            TypeName = dynamicFormDto.CatergoryName,
                            Cid = _globalDataService.CId,
                            Remark = dynamicFormDto.Remark,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        return Ok(newItemType);
                    }

                    else if (dynamicFormDto.DynamicField == "Designation")
                    {
                        var existing = await _db.TblDesignationtypes.FirstOrDefaultAsync(x => x.TypeName == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }

                        var newItemType = new TblDesignationtype
                        {
                            TypeName = dynamicFormDto.CatergoryName,
                            Cid = _globalDataService.CId,
                            Remark = dynamicFormDto.Remark,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        await connection.OpenAsync();
                        string updateQuery = $"ALTER TABLE tblDesignationPermission ADD U{newItemType.TypeId} int";

                        SqlCommand updateCommand = new SqlCommand(updateQuery, connection);

                        int rowsAffected = await updateCommand.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                                await connection.CloseAsync();
                        }

                        var columnName = $"U{newItemType.TypeId}";
                        var query2 = $"UPDATE tblDesignationPermission SET {columnName} = 0";

                        using (SqlCommand command2 = new SqlCommand(query2, connection))
                        {
                            await command2.ExecuteNonQueryAsync();
                        }

                        await connection.CloseAsync();
                    }

                        return Ok(newItemType);
                    }

                    else if (dynamicFormDto.DynamicField == "PropertyType")
                    {
                        var existing = await _db.Tblpropertytypes.FirstOrDefaultAsync(x => x.Propertytype == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }

                        var newItemType = new Tblpropertytype
                        {
                            Propertytype = dynamicFormDto.CatergoryName,
                            Cid = _globalDataService.CId,
                            Remark = dynamicFormDto.Remark,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        return Ok(newItemType);
                    }

                    else if (dynamicFormDto.DynamicField == "PropertyCategory")
                    {
                        var existing = await _db.TblpropertyCategories.FirstOrDefaultAsync(x => x.PropertyCat == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }

                        var newItemType = new TblpropertyCategory
                        {
                            PropertyCat = dynamicFormDto.CatergoryName,
                            Cid = _globalDataService.CId,
                            Remark = dynamicFormDto.Remark,
                            Status = dynamicFormDto.Status
                        };

                        await _db.AddAsync(newItemType);
                        await _db.SaveChangesAsync();

                        return Ok(newItemType);
                    }

                    else if (dynamicFormDto.DynamicField == "PropertySubcategory")
                    {
                        var existing = await _db.TblpropertySubCategories.FirstOrDefaultAsync(x => x.PropertySubCat == dynamicFormDto.CatergoryName);

                        if (existing != null)
                        {
                            return BadRequest("Item with this name already exist");
                        }

                        var newItemType = new TblpropertySubCategory
                        {
                            PropertySubCat = dynamicFormDto.CatergoryName,
                            Cid = _globalDataService.CId,
                            Remark = dynamicFormDto.Remark,
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



    }
}
