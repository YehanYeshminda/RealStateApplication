namespace API.Repos.Dtos.CallInsignt
{
    public class AssignCallInsightDto
    {
        public List<int> CallInsigntIds { get; set; }
        public AuthDto AuthDto { get; set; }
        public int AssignStaff { get; set; }
    }


    public class AssignTopCallInsightDto
    {
        public AuthDto AuthDto { get; set; }
        public int AssignStaff { get; set; } 
        public int NumberOfItemsToAssign { get; set; } 
    }
}
