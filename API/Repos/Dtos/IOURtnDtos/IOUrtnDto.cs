namespace API.Repos.Dtos.IOURtnDtos
{
    public class IOUrtnDto
    {
        public AuthDto AuthDto { get; set; }
        public int Rtnid { get; set; }
        public int Iouid { get; set; }
        public DateTime Retnon { get; set; }
        public string? Desc { get; set; }
    }
}
