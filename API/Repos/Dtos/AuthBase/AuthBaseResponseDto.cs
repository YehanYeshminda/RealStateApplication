using API.Models;

namespace API.Repos.Dtos.AuthBase
{
    public class AuthBaseResponseDto
    {
        public Tbluser? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
        public string RefNo { get; set; } = "";
    }
}
