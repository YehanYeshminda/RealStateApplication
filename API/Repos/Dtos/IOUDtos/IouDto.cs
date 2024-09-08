namespace API.Repos.Dtos.IOUDtos
{
    public class IouDto
    {
        public AuthDto AuthDto { get; set; }
        public int Id { get; set; }
        public int Branchid { get; set; }
        public DateTime Date { get; set; }
        public int Issueto { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime Returnon { get; set; }
        public int Approvedby { get; set; }
        public decimal Value { get; set; }
        public int Returned { get; set; }
    }
}
