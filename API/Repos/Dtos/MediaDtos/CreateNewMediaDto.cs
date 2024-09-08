namespace API.Repos.Dtos.MediaDtos
{
    public class CreateNewMediaDto
    {
        public AuthDto AuthDto { get; set; }
        public string? Media { get; set; }
        public string? Remark { get; set; }
        public int? Status { get; set; }
        public int? Cid { get; set; }
    }

    public class EditExistingMediaDto
    {
        public AuthDto AuthDto { get; set; }
        public int Id { get; set; }
        public string? Media { get; set; }
        public string? Remark { get; set; }
        public int? Status { get; set; }
        public int? Cid { get; set; }
    }

    public class MedialinkDto
    {
        public int Id { get; set; }
        public string Campainno { get; set; } = null!;
        public string Medialink { get; set; } = null!;
        public int Addby { get; set; }
        public DateTime Addon { get; set; }
    }
}
