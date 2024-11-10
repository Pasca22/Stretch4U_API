using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stretch4U.API.Requests.Users;
using Stretch4U.Business.Users.Services;

namespace Stretch4U.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            try
            {
                var result = await _userService.Signup(request.ToSignupRequestDTO());

                return Ok(result);
            }
            catch (DbUpdateException)
            {
                return new BadRequestObjectResult("Username already exists");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _userService.Login(request.ToLoginRequestDTO());

                return Ok(result);
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Invalid username or password");
            }
        }
    }
}
