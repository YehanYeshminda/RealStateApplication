namespace API.Repos.Dtos.ExpensesDtos
{
    public class ExpensesDto
    {
        public AuthDto AuthDto { get; set; }
        public string Id { get; set; } = null!;
        public DateTime? VDate { get; set; }
        public string? SupplierId { get; set; }
        public string? MainCatId { get; set; }
        public string? SubcatId { get; set; }
        public string? Description { get; set; }
        public decimal CashPaid { get; set; }
        public decimal ChequePaid { get; set; }
        public string? ChequeNo { get; set; }
        public decimal Status { get; set; }
        public string? AuthBy { get; set; }
        public string ReceiptNo { get; set; } = null!;
        public int? AccountId { get; set; }
        public int UserId { get; set; }
        public DateTime? RDate { get; set; }
        public string? UniqueId { get; set; }
        public int BrId { get; set; }
        public decimal TotalValue { get; set; }
        public decimal Vatp { get; set; }
        public decimal Vat { get; set; }
        public decimal NetTotal { get; set; }
        public decimal Paid { get; set; }
    }

    public class GetTblExpenseAccountDto
    {
        public AuthDto authDto { get; set; }
        public int Id { get; set; }
        public string MainCatId { get; set; } = null!;
        public string MainCatergory { get; set; }
        public string SubCatId { get; set; } = null!;
        public string SubCatergory { get; set; }
        public int Status { get; set; }
    }
}
