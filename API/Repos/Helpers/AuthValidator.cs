using API.Models;
using API.Repos.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace API.Repos.Helpers
{
    public static class AuthValidator
    {
        public static async Task<bool> IsUsernameAndPasswordValid(string connectionString, string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(
                    "SELECT COUNT(*) FROM tblusers WHERE loginname = @loginName AND password = @password",
                    connection);

                command.Parameters.AddWithValue("@loginName", username);
                command.Parameters.AddWithValue("@password", password);

                int count = (int)await command.ExecuteScalarAsync();

                return count > 0;
            }
        }

        public static async Task<bool> ValidateUserAsync(CRMContext context, HelperAuth decodedValues, AuthDto authDto)
        {
            var user = await context.Tblusers.FirstOrDefaultAsync(x => x.Userid == decodedValues.UserId && x.Hash == authDto.Hash);

            if (user == null)
            {
                return false;
            }

            return true;
        }

        public static HelperAuth DecodeValue(string encodedValue)
        {
            byte[] bytes = Convert.FromBase64String(encodedValue);
            string valueToDecode = Encoding.UTF8.GetString(bytes);

            string[] parts = valueToDecode.Split('!');
            if (parts.Length == 2)
            {
                int userId = int.Parse(parts[0]);
                DateTime dateTime = DateTime.ParseExact(parts[1], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                return new HelperAuth
                {
                    Date = dateTime,
                    UserId = userId
                };
            }
            else
            {
                return null;
            }
        }
    }
}
