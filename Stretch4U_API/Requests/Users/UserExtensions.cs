using Stretch4U.Business.Users.Models;

namespace Stretch4U.API.Requests.Users;

internal static class UserExtensions
{
    public static SignupRequestDTO ToSignupRequestDTO(this SignupRequest request) =>
        new SignupRequestDTO
        {
            Username = request.Username,
            Password = request.Password
        };

    public static LoginRequestDTO ToLoginRequestDTO(this LoginRequest request) =>
        new LoginRequestDTO
        {
            Username = request.Username,
            Password = request.Password
        };
}
