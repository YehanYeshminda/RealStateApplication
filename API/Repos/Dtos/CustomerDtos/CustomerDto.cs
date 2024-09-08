namespace API.Repos.Dtos.CustomerDtos
{
    public class CustomerDto
    {
        public AuthDto AuthDto { get; set; }
        public int CustId { get; set; }
        public string? CustName { get; set; }
        public string? CustAddress { get; set; }
        public string? CustCity { get; set; }
        public string? CustMobile { get; set; }
        public string? CustPhone { get; set; }
        public string? Email { get; set; }
        public string? ContPerson { get; set; }
        public int CreditAllow { get; set; }
        public decimal CreditLimit { get; set; }
        public int? CreditDays { get; set; }
        public int Status { get; set; }
        public int? CreditPeriod { get; set; }
        public string? Remarks { get; set; }
        public decimal Points { get; set; }
        public string? CardNo { get; set; }
        public string? VatNo { get; set; }
        public decimal TotRetCheque { get; set; }
    }
}
