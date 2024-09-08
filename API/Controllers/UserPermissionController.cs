using API.Models;
using API.Repos;
using API.Repos.Dtos;
using API.Repos.Dtos.UserDtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Controllers
{
    [Route("api/up")]
    [ApiController]
    public class UserPermissionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly CRMContext _context;
        private readonly ResponseDto _response;

        public UserPermissionController(IUnitOfWork unitOfWork, IConfiguration configuration, CRMContext context)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _context = context;
            _response = new ResponseDto();
        }

        [HttpPost("GetToSideNavShow")]
        public async Task<ResponseDto> GetAllUserPermissionUponDestination([FromBody] AuthDto authDto)
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
                var existingStaff = await _unitOfWork.staffInterface.GetStaffByIdAsync(authResponse.Result.Userid);

                if (existingStaff == null)
                {
                    _response.Message = "Unable to find staff with this Id";
                    _response.IsSuccess = false;
                    return _response;
                }

                var designationByName = await _context.TblDesignationtypes.FirstOrDefaultAsync(x => x.TypeName == existingStaff.Designation);

                var existingPermissions = await _unitOfWork.userpermissionInterface.GetDesignationById(designationByName.TypeId.ToString());

                HashSet<string> uniqueAccessLocations = new HashSet<string>();

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    SqlCommand getChequeInfoBybankId = new SqlCommand("SELECT U" + designationByName.TypeId + ", accesslocation, event FROM tblDesignationPermission;", connection);

                    using (SqlDataReader reader = await getChequeInfoBybankId.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                    {
                        while (await reader.ReadAsync())
                        {
                            int userValue = reader.GetInt32(0);

                            if (userValue == 1)
                            {
                                string accessLocation = reader.GetString(1);

                                uniqueAccessLocations.Add(accessLocation);
                            }
                        }
                    }
                }

                List<string> uniqueAccessLocationList = uniqueAccessLocations.ToList();

                _response.Result = uniqueAccessLocationList;
                _response.IsSuccess = true;

                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting user permission. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }



        [HttpPut("UpdateAllUserPermission")]
        public async Task<ResponseDto> UpdateAllUserPermission(UpdateUserPermissionMultiDto updateUserPermissionMultiDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(updateUserPermissionMultiDto.AuthDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                await _unitOfWork.userpermissionInterface.UpdateUserPermissionAsync(updateUserPermissionMultiDto);
                _response.IsSuccess = true;
                _response.Message = "Given to full access to " + updateUserPermissionMultiDto.UserId;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating user permission. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPut("UpdateAllUserPermissionDesignation")]
        public async Task<ResponseDto> UpdateAllUserPermissionDesignation(UpdateUserPermissionMultiDto updateUserPermissionMultiDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(updateUserPermissionMultiDto.AuthDto);

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
                Location = AccessLocation.UserPermission.ToString()
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
                await _unitOfWork.userpermissionInterface.UpdateUserPermissionDesignationAsync(updateUserPermissionMultiDto);
                _response.IsSuccess = true;
                _response.Message = "Given to full access to " + updateUserPermissionMultiDto.UserId;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating user permission. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPut("UpdateUserPermission")]
        public async Task<ResponseDto> UpdateUserPermission([FromBody] UpdateUserPermissionDto updateUserPermissionDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(updateUserPermissionDto.AuthDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    string updateQuery = $"UPDATE tbluserpermission SET U{updateUserPermissionDto.UserId} = @PermissionValue " +
                        "WHERE accesslocation = @accessLocation AND event = @event;";

                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);

                    updateCommand.Parameters.AddWithValue("@PermissionValue", updateUserPermissionDto.hasPermission);
                    updateCommand.Parameters.AddWithValue("@accessLocation", updateUserPermissionDto.AccessLocation);
                    updateCommand.Parameters.AddWithValue("@event", updateUserPermissionDto.Event);

                    int rowsAffected = await updateCommand.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        await connection.CloseAsync();
                        _response.Message = "User permission updated successfully.";
                        _response.IsSuccess = true;
                        return _response;
                    }

                    await connection.CloseAsync();
                    _response.Message = "Failed to update user permission. User not found.";
                    _response.IsSuccess = false;
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating user permission. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPut("UpdateUserPermissionDesignation")]
        public async Task<ResponseDto> UpdateUserPermissionDesignation([FromBody] UpdateUserPermissionDto updateUserPermissionDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(updateUserPermissionDto.AuthDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    string updateQuery = $"UPDATE tblDesignationPermission SET U{updateUserPermissionDto.UserId} = @PermissionValue " +
                        "WHERE accesslocation = @accessLocation AND event = @event;";

                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);

                    updateCommand.Parameters.AddWithValue("@PermissionValue", updateUserPermissionDto.hasPermission);
                    updateCommand.Parameters.AddWithValue("@accessLocation", updateUserPermissionDto.AccessLocation);
                    updateCommand.Parameters.AddWithValue("@event", updateUserPermissionDto.Event);

                    int rowsAffected = await updateCommand.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        await connection.CloseAsync();
                        _response.Message = "User permission updated successfully.";
                        _response.IsSuccess = true;
                        return _response;
                    }

                    await connection.CloseAsync();
                    _response.Message = "Failed to update user permission. User not found.";
                    _response.IsSuccess = false;
                    return _response;
                }
            }
            catch (Exception ex)
            {
                _response.Message = "Error while updating user permission. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("GetUserPermissionForView")]
        public async Task<ActionResult<dynamic>> GetUserpermissionForView([FromBody] SendGetUserPermission sendGetUserPermission)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(sendGetUserPermission.authDto);

            if (!authResponse.IsSuccess)
            {
                return Unauthorized(authResponse.Message);
            }

            try
            {

                var existingStaff = await _unitOfWork.staffInterface.GetStaffByIdAsync(authResponse.Result.Userid);

                if (existingStaff == null)
                {
                    _response.Message = "Unable to find staff with this Id";
                    _response.IsSuccess = false;
                    return _response;
                }

                var designationByName = await _context.TblDesignationtypes.FirstOrDefaultAsync(x => x.TypeName == existingStaff.Designation);

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    SqlCommand getChequeInfoBybankId = new SqlCommand("SELECT U" + designationByName.TypeId + ", Accesslocation, Event FROM tblDesignationPermission WHERE Accesslocation = @Location AND Event = @Event", connection);
                    getChequeInfoBybankId.Parameters.AddWithValue("@Location", sendGetUserPermission.Location);
                    getChequeInfoBybankId.Parameters.AddWithValue("@Event", sendGetUserPermission.Event);

                    try
                    {
                        using (SqlDataReader reader = await getChequeInfoBybankId.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                        {
                            if (await reader.ReadAsync())
                            {
                                string dictionary = reader[0].ToString();
                                await connection.CloseAsync();
                                return Ok(new 
                                { 
                                    HasAccess = dictionary
                                });
                            }

                            await connection.CloseAsync();
                            return BadRequest("Unable to user information!");
                        }
                    }
                    catch (Exception ex)
                    {
                        await connection.CloseAsync();
                        return BadRequest("Error while getting user information. " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("InsertUserPermission")]
        public async Task<IActionResult> InsertUserPermission([FromBody] SendGetUserPermission sendUserPermission)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(sendUserPermission.authDto);

            if (!authResponse.IsSuccess)
            {
                return Unauthorized(authResponse.Message);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    string checkColumnQuery = $"IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = 'U{authResponse.Result.Userid}' AND Object_ID = Object_ID('tbluserpermission')) " +
                                              $"BEGIN " +
                                              $"    ALTER TABLE tbluserpermission ADD U{authResponse.Result.Userid} INT; " +
                                              $"END";

                    SqlCommand checkColumnCommand = new SqlCommand(checkColumnQuery, connection);
                    await checkColumnCommand.ExecuteNonQueryAsync();
                    string insertQuery = $"INSERT INTO tbluserpermission (U{authResponse.Result.Userid}, Accesslocation, Event) " +
                        "VALUES (@PermissionValue, @Location, @Event)";

                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);

                    insertCommand.Parameters.AddWithValue("@PermissionValue", sendUserPermission.HasPermission);
                    insertCommand.Parameters.AddWithValue("@Location", sendUserPermission.Location);
                    insertCommand.Parameters.AddWithValue("@Event", sendUserPermission.Event);

                    int rowsAffected = await insertCommand.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        await connection.CloseAsync();
                        return Ok("User permission inserted successfully.");
                    }

                    await connection.CloseAsync();
                    return BadRequest("Failed to insert user permission.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error while inserting user permission. " + ex.Message);
            }
        }


        [HttpPost("CheckUserRole")]
        public async Task<ResponseDto> CheckUserRolePermission(AuthDto authDto)
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
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    try
                    {
                        await connection.OpenAsync();

                        var selectUserQuery = "SELECT * FROM tblusers WHERE tblusers.userid = @UserId";
                        SqlCommand selectUserCommand = new SqlCommand(selectUserQuery, connection);
                        selectUserCommand.Parameters.AddWithValue("@UserId", authResponse.Result.Userid);

                        using (SqlDataReader userReader = await selectUserCommand.ExecuteReaderAsync())
                        {
                            if (await userReader.ReadAsync())
                            {
                                var userId = userReader.GetInt32(userReader.GetOrdinal("userid"));

                                var selectStaffQuery = "SELECT * FROM tblstaffs WHERE id = @UserId";
                                SqlCommand selectStaffCommand = new SqlCommand(selectStaffQuery, connection);
                                selectStaffCommand.Parameters.AddWithValue("@UserId", userId);

                                using (SqlDataReader staffReader = await selectStaffCommand.ExecuteReaderAsync())
                                {
                                    if (await staffReader.ReadAsync())
                                    {
                                        var selectedUserDesignation = staffReader.GetString(staffReader.GetOrdinal("designation"));

                                        if (selectedUserDesignation.ToLower() == "reporting manager")
                                        {
                                            _response.Message = "User is a reporting manager";
                                            _response.Result = true;
                                            return _response;
                                        }
                                        else
                                        {
                                            _response.Message = "User is not a reporting manager";
                                            _response.Result = false;
                                            return _response;
                                        }
                                    }
                                    else
                                    {
                                        _response.Message = "No staff record found for this user";
                                        _response.IsSuccess = false;
                                        return _response;
                                    }
                                }
                            }
                            else
                            {
                                _response.Message = "No user record found with this ID";
                                _response.IsSuccess = false;
                                return _response;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _response.Message = "Error while querying the database: " + ex.Message;
                        _response.IsSuccess = false;
                        return _response;
                    }
                }

            }
            catch (Exception ex)
            {
                _response.Message = "Error while getting user permission. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }

        [HttpPost("CheckUserRoleForDashbaord")]
        public async Task<ResponseDto> CheckUserRoleForDashboard([FromBody] AuthDto authDto, [FromQuery] string role)
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
                var existingStaff = await _context.Tblstaffs.FirstOrDefaultAsync(x => x.Id == authResponse.Result.Userid);

                if (existingStaff == null)
                {
                    _response.Message = "User with this information does not exist";
                    _response.IsSuccess = false;
                    return _response;
                }

                if (existingStaff.Designation != role)
                {
                    _response.Message = "";
                    _response.Result = false;
                    _response.IsSuccess = true;
                    return _response;
                }

                _response.Result = true;
                _response.Message = "User has access to dashboard";
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = "error while getting user permission. " + ex.Message;
                _response.IsSuccess = false;
                return _response;
            }
        }
    }
}
