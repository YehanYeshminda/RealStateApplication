namespace API.Repos.Dtos.AccountDtos
{
    public class RegisterUserDto
    {
        public AuthDto AuthDto { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? LoginName { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public decimal? FullAccess { get; set; }
        public int SuperUser { get; set; }
        public string? Password { get; set; }
        public int? Cid { get; set; }
        public decimal Status { get; set; }
        public decimal OpenBalance { get; set; }
        public decimal Discount { get; set; }
        public int Ud { get; set; }
        public string? UserCode { get; set; }
        public IFormFile? UserLogo { get; set; }
    }
}
