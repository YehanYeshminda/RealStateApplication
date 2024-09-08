using System;
using API.Repos.Interfaces;
using Microsoft.Data.SqlClient;

namespace API.Repos.Services
{
    public class RsvpService : IRsvpInterface
    {
        private readonly IConfiguration _configuration;

        public RsvpService(IConfiguration configuration)
		{
            _configuration = configuration;
        }

        public async Task<IEnumerable<object>> GetExistingRsvpTypeAll()
        {
            List<dynamic> vstaffs = new List<dynamic>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM tblRsvpType where status = 0", connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dynamic vstaff = new System.Dynamic.ExpandoObject();
                            vstaff.Id = reader.GetInt32(reader.GetOrdinal("TypeId"));
                            vstaff.Name = reader.IsDBNull(reader.GetOrdinal("TypeName")) ? null : reader.GetString(reader.GetOrdinal("TypeName"));
                            vstaff.Designation = reader.IsDBNull(reader.GetOrdinal("Remark")) ? null : reader.GetString(reader.GetOrdinal("Remark"));
                            vstaff.Mobileno = reader.IsDBNull(reader.GetOrdinal("Status")) ? 0 : reader.GetInt32(reader.GetOrdinal("Status"));
                            vstaff.Parentid = reader.IsDBNull(reader.GetOrdinal("Cid")) ? 0 : reader.GetInt32(reader.GetOrdinal("Cid"));
                            vstaffs.Add(vstaff);
                        }
                    }
                }
            }

            return vstaffs;
        }

        public class RsvpType
        {
            public int TypeId { get; set; }
            public string TypeName { get; set; }
            public string Remark { get; set; }
            public int Status { get; set; }
            public int Cid { get; set; }
        }

        public async Task<RsvpType> GetExistingTypeId(int id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("SELECT * FROM [tblRsvpType] WHERE [TypeId] = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var result = new RsvpType
                            {
                                TypeId = reader.IsDBNull(reader.GetOrdinal("TypeId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TypeId")),
                                TypeName = reader.IsDBNull(reader.GetOrdinal("TypeName")) ? null : reader.GetString(reader.GetOrdinal("TypeName")),
                                Remark = reader.IsDBNull(reader.GetOrdinal("Remark")) ? null : reader.GetString(reader.GetOrdinal("Remark")),
                                Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? 0 : reader.GetInt32(reader.GetOrdinal("Status")),
                                Cid = reader.IsDBNull(reader.GetOrdinal("Cid")) ? 0 : reader.GetInt32(reader.GetOrdinal("Cid"))
                            };

                            return result;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }



        public async Task<(bool success, string error)> InsertDataAsync(string tableName, Dictionary<string, object> data)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    var columns = string.Join(", ", data.Keys);
                    var values = string.Join(", ", data.Keys.Select(key => "@" + key));
                    var query = $"INSERT INTO {tableName} ({columns}) VALUES ({values});";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        foreach (var kvp in data)
                        {
                            command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
                        }

                        await command.ExecuteNonQueryAsync();
                    }
                }

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message); // Return error
            }
        }
    }
}

