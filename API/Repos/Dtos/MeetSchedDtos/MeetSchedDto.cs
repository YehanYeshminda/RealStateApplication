namespace API.Repos.Dtos.MeetSchedDtos
{
    public class MeetSchedDto
    {
        public AuthDto AuthDto { get; set; }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Date { get; set; }
        public int Staffid { get; set; }
        public string Reason { get; set; } = null!;
        public int Custid { get; set; }
        public DateTime Meetdate { get; set; }
        public string Meettime { get; set; } = null!;
        public string Venue { get; set; } = null!;
        public string Remarks { get; set; } = null!;
        public int Status { get; set; }
        public string Conclusion { get; set; } = null!;
        public List<int> staffIds { get; set; }
    }


    public class UpdateConclusionDto
    {
        public AuthDto AuthDto { get; set; }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Conclusion { get; set; } = null!;
    }

    public class ReSchedDto
    {
        public AuthDto AuthDto { get; set; }
        public int Id { get; set; }
        public DateTime Meetdate { get; set; }
        public string Meettime { get; set; } = null!;
        public string Venue { get; set; } = null!;
    }

}
