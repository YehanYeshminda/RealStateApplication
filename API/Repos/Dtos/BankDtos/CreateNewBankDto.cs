namespace API.Repos.Dtos.BankDtos
{
    public class CreateNewBankDto
    {
        public AuthDto AuthDto { get; set; }
        public string BankName { get; set; }
        public int Status { get; set; }
    }

    public class EditExistingBankDto
    {
        public AuthDto AuthDto { get; set; }
        public string BankId { get; set; }
        public string BankName { get; set; }
        public int Status { get; set; }
    }
}
