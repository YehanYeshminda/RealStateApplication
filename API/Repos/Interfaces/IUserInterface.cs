using API.Models;
using API.Repos.Dtos.UserDtos;

namespace API.Repos.Interfaces
{
    public interface IUserInterface
    {
        Task<Tbluser> GetUserById(int id);
        Task<IEnumerable<GetUserNameAndId>> GetAllUsersNameAndId();
        Task<Tbluser?> GetExistingUserByUsername(string username);
        Task<Tbluser?> GetExistingUserByUsernamePasswordStatus(string username, string password);
    }
}
