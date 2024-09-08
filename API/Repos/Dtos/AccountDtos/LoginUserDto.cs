using System.ComponentModel.DataAnnotations;

namespace API.Repos.Dtos.AccountDtos
{
    public class LoginUserDto
    {
        [Required]
        public int BranchId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
