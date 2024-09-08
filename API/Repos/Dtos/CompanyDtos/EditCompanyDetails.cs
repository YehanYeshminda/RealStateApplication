namespace API.Repos.Dtos.CompanyDtos
{
    public class EditCompanyDetails
    {
        public AuthDto AuthDto { get; set; }
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? Web { get; set; }
        public int Status { get; set; }
        public decimal CashBookH { get; set; }
        public decimal ServiceCharge { get; set; }
        public int AutoBulkInvoice { get; set; }
        public string? BarcodeTitle { get; set; }
        public IFormFile? CompanyLogo { get; set; }
    }
}
