using API.Models;

namespace API.Repos.Dtos.LeadsDtos
{
    public class ReturnLeadDto
    {
        public string? LeadName { get; set; }
        public string? ContactMethod { get; set; }
        public Tbllead Lead { get; set; }
    }
}
