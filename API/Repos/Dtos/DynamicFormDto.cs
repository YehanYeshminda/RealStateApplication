using API.Repos.Dtos;

namespace API.Repos.Dtos
{
    public class DynamicFormDto
    {
        public AuthDto AuthDto { get; set; }
        public string DynamicField { get; set; }
        public string CatergoryName { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
    }


    public class DynamicExpenseDto
    {
        public AuthDto AuthDto { get; set; }
        public int Id { get; set; }
        public string DynamicField { get; set; }
        public string MainCatId { get; set; } = null!;
        public string SubCatId { get; set; } = null!;
        public int Status { get; set; }
    }

}

