namespace API.Repos.Dtos.StaffDtos
{
    public class StaffDto
    {
        public string Hash { get; set; }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string Mobileno { get; set; } = null!;
        public int Parentid { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public IFormFile? Passport { get; set; }
        public IFormFile? Userimage { get; set; }
        public string VisaIssueDate { get; set; }
        public string? Password { get; set; }
    }

    //public class StaffDto
    //{
    //    public AuthDto AuthDto { get; set; }
    //    public int Id { get; set; }
    //    public string Name { get; set; } = null!;
    //    public string Designation { get; set; } = null!;
    //    public string Mobileno { get; set; } = null!;
    //    public int Parentid { get; set; }
    //    public int Status { get; set; }
    //}
}
