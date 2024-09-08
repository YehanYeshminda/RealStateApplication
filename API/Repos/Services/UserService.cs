using API.Models;
using API.Repos.Dtos.UserDtos;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class UserService : IUserInterface
    {
        private readonly CRMContext _db;

        public UserService(CRMContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<GetUserNameAndId>> GetAllUsersNameAndId()
        {
            var users = await _db.Tblusers
            .Where(u => u.Userid > 0)
            .Select(u => new GetUserNameAndId
            {
                UserId = u.Userid,
                Username = u.Username
            })
            .AsNoTracking()
            .ToListAsync();

            return users;
        }

        public async Task<Tbluser?> GetExistingUserByUsername(string username)
        {
            return await _db.Tblusers.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<Tbluser?> GetExistingUserByUsernamePasswordStatus(string username, string password)
        {
            return await _db.Tblusers.FirstOrDefaultAsync(x => x.Username != null && x.Username.Equals(username) && x.Password != null && x.Password.Equals(password) && x.Status == 0);
        }

        public async Task<Tbluser> GetUserById(int id)
        {
            return await _db.Tblusers.FirstOrDefaultAsync(x => x.Userid == id);
        }
    }
}
