using Stretch4U.Business.Users.Models;

namespace Stretch4U.Business.Users.Services;

public interface IUserService
{
    Task<JwtToken> Signup(SignupRequestDTO request);

    Task<JwtToken> Login(LoginRequestDTO request);

}
