using Stretch4U.Business.Users.Models;

namespace Stretch4U.Business.Users.Services;

public interface IUserService
{
    Task<UserData> Signup(SignupRequestDTO request);
    Task<UserData> Login(LoginRequestDTO request);
    Task<UserData> GetUserData(int userId);
}
