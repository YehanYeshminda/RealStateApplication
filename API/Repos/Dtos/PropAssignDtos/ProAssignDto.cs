namespace API.Repos.Dtos.PropAssignDtos
{
    public class ProAssignDto
    {
        public AuthDto AuthDto { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Salesperson { get; set; }
        public int Customerid { get; set; }
        public DateTime Validtill { get; set; }
        public string Advnotno { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
