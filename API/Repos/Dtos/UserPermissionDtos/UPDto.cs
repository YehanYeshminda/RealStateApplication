namespace API.Repos.Dtos.UserPermissionDtos
{
    public class GetUserPermission
    {
        public bool HasPermission { get; set; }
    }

    public class GetAllUserPermission
    {
        public string AccessLocation { get; set; }
        public List<PermissionItem> Event { get; set; }
    }

    public class PermissionItem
    {
        public string Value { get; set; }
        public string HasPermission { get; set; }
    }

    public class SendGetUserPermission
    {
        public AuthDto? authDto { get; set; }
        public string Location { get; set; }
        public string Event { get; set; }
        public int? HasPermission { get; set; }
    }

    public class UserPermissionDto
    {
        public int UserValue { get; set; }
        public string Event { get; set; }
        public string AccessLocation { get; set; }
    }
}
