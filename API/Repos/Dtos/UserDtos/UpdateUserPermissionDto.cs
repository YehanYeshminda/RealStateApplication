namespace API.Repos.Dtos.UserDtos
{
    public class UpdateUserPermissionDto
    {
        public AuthDto? AuthDto { get; set; }
        public string AccessLocation { get; set; }
        public string Event { get; set; }
        public string hasPermission { get; set; }
        public int UserId { get; set; }
    }
}
