using API.Repos.Dtos;
using API.Repos.Dtos.AuthBase;

namespace API.Repos.Interfaces
{
    public interface IAuthenticationService
    {
        AuthBaseResponseDto ValidateAuthentication(AuthDto authDto);
    }
}
