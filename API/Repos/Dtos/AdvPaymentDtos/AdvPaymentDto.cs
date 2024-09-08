namespace API.Repos.Dtos.AdvPaymentDtos
{
    public class AdvPaymentDto
    {
        public AuthDto AuthDto { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Salesby { get; set; }
        public int Customer { get; set; }
        public string Address { get; set; } = null!;
        public decimal Chequepaid { get; set; }
        public string Chequeno { get; set; } = null!;
        public decimal Cashpaid { get; set; }
        public decimal Cardpaid { get; set; }
        public int Cardbank { get; set; }
        public string Paymentfor { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
