using System;
namespace API.Repos.Dtos.CallCenterDtos
{
	public class ConvertToCallDto
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string? Status { get; set; }
		public string? NotLookingRadioStatus { get; set; }
		public bool? IsRsvp { get; set; }
		public string? Project { get; set; }
		public int? ClientIs { get; set; }
		public int? PlanToDo { get; set; }
		public DateTime? When { get; set; }
		public string? Attending { get; set; }
		public int? RsvpType { get; set; }
		public bool Cross { get; set; }
        public string Comments { get; set; }
		public string? IsLost { get; set; }
		public AuthDto AuthDto { get; set; }
		public int IsInterested { get; set; }

	}
}

