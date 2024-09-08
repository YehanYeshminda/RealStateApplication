namespace API.Repos.Dtos.CommonDto
{
    public class CommonDto
    {
        public int value { get; set; }
        public string textValue { get; set; }
    }

    public class BankInfoDto
    {
        public string value { get; set; }
        public string textValue { get; set; }
    }

    public class stringcombobox
    {
        public string value { get; set; }
        public string textValue { get; set; }
    }

    public class CustomRetrievalTableDto
    {
        public int Id { get; set; }
        public string DynamicField { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public int CId { get; set; }
    }

    public class CustomExpenseTableDto
    {
        public int Id { get; set; }
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
        public int Status { get; set; }
    }
}
