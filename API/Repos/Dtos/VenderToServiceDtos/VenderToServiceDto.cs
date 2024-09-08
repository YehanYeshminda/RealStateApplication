namespace API.Repos.Dtos.VenderToServiceDto
{
    public class VenderToServiceDto
    {
        public AuthDto AuthDto { get; set; }
        public int Id { get; set; }
        public int Venderid { get; set; }
        public int Serviceid { get; set; }
        public int Status { get; set; }
    }

    public class UpdateVenderToServiceDto
    {
        public AuthDto AuthDto { get; set; }
        public int Id { get; set; }
        public int Venderid { get; set; }
        public int Serviceid { get; set; }
        public int Status { get; set; }
    }
}
