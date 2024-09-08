namespace API.Repos.Dtos.SourceDto
{
    public class CreateSourceDto
    {
        public AuthDto AuthDto { get; set; }
        public int? Id { get; set; }
        public string? Source { get; set; }
        public string? Remark { get; set; }
        public int? Status { get; set; }
        public string? Cid { get; set; }
    }
}
