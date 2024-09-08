namespace API.Repos.Dtos.CampainHDtos
{
    public class CampainHDtos
    { 
        public AuthDto AuthDto { get; set; }
        public string No { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Name { get; set; } = null!;
        public DateTime Datefrom { get; set; }
        public DateTime Dateto { get; set; }
        public string Description { get; set; } = null!;
        public string Totalcost { get; set; } = null!;
        public string Remarks { get; set; } = null!;
        public int Status { get; set; }
        public List<int> mediaIds { get; set; }

        public string medialink { get; set; }
    }
}
