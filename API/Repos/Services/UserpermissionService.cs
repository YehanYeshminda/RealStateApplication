using API.Models;
using API.Repos.Dtos.UserDtos;
using API.Repos.Dtos.UserPermissionDtos;
using API.Repos.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Repos.Services
{
    public class UserpermissionService : IUserpermissionInterface
    {
        private readonly CRMContext _db;
        private readonly IConfiguration _configuration;

        public UserpermissionService(CRMContext context, IConfiguration configuration)
        {
            _db = context;
            _configuration = configuration;
        }
        public async Task<IEnumerable<Tbluserpermission>> GetAllUPAsync()
        {
            return await _db.Tbluserpermissions.ToListAsync();
        }

        public async Task<IEnumerable<GetAllUserPermission>> GetAllUserPermissionById(string userId)
        {
            var permissionsDictionary = new Dictionary<string, List<PermissionItem>>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    SqlCommand getChequeInfoBybankId = new SqlCommand("SELECT U" + userId + ", AccessLocation, Event FROM tbluserpermission", connection);

                    using (SqlDataReader reader = await getChequeInfoBybankId.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                    {
                        while (await reader.ReadAsync())
                        {
                            string userValue = reader.GetInt32(0).ToString();

                            string accessLocation = reader.GetString(1);
                            string eventValue = reader.GetString(2);

                            if (!permissionsDictionary.ContainsKey(accessLocation))
                            {
                                permissionsDictionary[accessLocation] = new List<PermissionItem>();
                            }

                            permissionsDictionary[accessLocation].Add(new PermissionItem
                            {
                                Value = eventValue,
                                HasPermission = userValue.ToString()
                            });
                        }
                    }
                }

                var permissionsList = permissionsDictionary.Select(kv => new GetAllUserPermission
                {
                    AccessLocation = kv.Key,
                    Event = kv.Value
                }).ToList();

                return permissionsList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<Tbluserpermission> GetUPByIdAsync(int id)
        {
            return await _db.Tbluserpermissions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<GetUserPermission> GetUserPermission(SendGetUserPermission sendGetUserPermission, string userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    var staffDesignation = await _db.Tblstaffs.FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(userId));
                    var existingDesignation = await _db.TblDesignationtypes.FirstOrDefaultAsync(x => x.TypeName == staffDesignation.Designation);

                    SqlCommand getChequeInfoBybankId = new SqlCommand("SELECT U" + existingDesignation.TypeId + ", Accesslocation, Event FROM tblDesignationPermission WHERE Accesslocation = @Location AND Event = @Event", connection);
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

                                if (dictionary == "0")
                                {
                                    return new GetUserPermission
                                    {
                                        HasPermission = false
                                    };
                                } else
                                {
                                    return new GetUserPermission
                                    {
                                        HasPermission = true
                                    };
                                }
                            }

                            await connection.CloseAsync();
                            return new GetUserPermission
                            {
                                HasPermission = false
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        await connection.CloseAsync();
                        return new GetUserPermission
                        {
                            HasPermission = false
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new GetUserPermission
                {
                    HasPermission = false
                };
            }
        }

        public async Task<GetUserPermission> GetUserPermissionByDesignation(SendGetUserPermission sendGetUserPermission, string userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    SqlCommand getChequeInfoBybankId = new SqlCommand("SELECT U" + userId + ", accesslocation, event FROM tblDesignationPermission WHERE accesslocation = @Location AND event = @Event", connection);
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

                                if (dictionary == "0")
                                {
                                    return new GetUserPermission
                                    {
                                        HasPermission = false
                                    };
                                }
                                else
                                {
                                    return new GetUserPermission
                                    {
                                        HasPermission = true
                                    };
                                }
                            }

                            await connection.CloseAsync();
                            return new GetUserPermission
                            {
                                HasPermission = false
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        await connection.CloseAsync();
                        return new GetUserPermission
                        {
                            HasPermission = false
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new GetUserPermission
                {
                    HasPermission = false
                };
            }
        }

        public async Task<Tbluserpermission> GetUserPermissionByLocationEvent(string accessLocation, string eventDone)
        {
            return await _db.Tbluserpermissions.FirstOrDefaultAsync(x => x.Accesslocation == accessLocation && x.Event == eventDone);
        }

        public async Task UpdateUserPermissionAsync(UpdateUserPermissionMultiDto updateUserPermissionMultiDto)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    await connection.OpenAsync();
                    var query = $"UPDATE Tbluserpermission SET U{updateUserPermissionMultiDto.UserId} = {updateUserPermissionMultiDto.HasPermission}";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }

                    await connection.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while updating user permission: " + ex.Message);
                }
            }
        }

        public async Task UpdateUserPermissionDesignationAsync(UpdateUserPermissionMultiDto updateUserPermissionMultiDto)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    await connection.OpenAsync();
                    var query = $"UPDATE tblDesignationPermission SET U{updateUserPermissionMultiDto.UserId} = {updateUserPermissionMultiDto.HasPermission}";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }

                    await connection.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while updating user permission: " + ex.Message);
                }
            }
        }


        public async Task<Tbluserpermission> UpdateUserPermission(UpdateUserPermissionDto updateUserPermissionDto)
        {
            return await _db.Tbluserpermissions.Where(x => x.Accesslocation == updateUserPermissionDto.AccessLocation && x.Event == updateUserPermissionDto.Event).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<GetAllUserPermission>> GetDesignationById(string designationId)
        {
            var permissionsDictionary = new Dictionary<string, List<PermissionItem>>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    SqlCommand getChequeInfoBybankId = new SqlCommand("SELECT U" + designationId + ", accessLocation, event FROM tblDesignationPermission", connection);

                    using (SqlDataReader reader = await getChequeInfoBybankId.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                    {
                        while (await reader.ReadAsync())
                        {
                            string userValue = reader.GetInt32(0).ToString();

                            string accessLocation = reader.GetString(1);
                            string eventValue = reader.GetString(2);

                            if (!permissionsDictionary.ContainsKey(accessLocation))
                            {
                                permissionsDictionary[accessLocation] = new List<PermissionItem>();
                            }

                            permissionsDictionary[accessLocation].Add(new PermissionItem
                            {
                                Value = eventValue,
                                HasPermission = userValue.ToString()
                            });
                        }
                    }
                }

                var permissionsList = permissionsDictionary.Select(kv => new GetAllUserPermission
                {
                    AccessLocation = kv.Key,
                    Event = kv.Value
                }).ToList();

                return permissionsList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
