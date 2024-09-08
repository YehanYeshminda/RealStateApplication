namespace API.Repos.Dtos.UserPermissionDtos
{
    public class UpdateUserPermissionMultiDto
    {
        public AuthDto AuthDto { get; set; }
        public string UserId { get; set; }
        public string HasPermission { get; set; }
    }
}
