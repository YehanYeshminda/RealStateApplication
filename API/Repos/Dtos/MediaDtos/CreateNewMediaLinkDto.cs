namespace API.Repos.Dtos.MediaDtos
{
    public class CreateNewMediaLinkDto
    {
        public string Campainno { get; set; } = null!;
        public string Mediaid { get; set; } = null!;
        public AuthDto AuthDto { get; set; }
    }

    public class EditExistingMediaLinkDto
    {
        public string Campainno { get; set; } = null!;
        public string Mediaid { get; set; } = null!;
        public int Id { get; set; }
        public AuthDto AuthDto { get; set; }
    }
}
