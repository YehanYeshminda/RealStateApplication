using System;
namespace API.Repos.Dtos.Designation
{
	public class AddNewDesignationDto
	{
		public AuthDto authDto { get; set; }
		public string Name { get; set; }
		public string Remark { get; set; }
		public string Status { get; set; }
	}
}

