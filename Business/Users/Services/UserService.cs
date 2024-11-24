using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stretch4U.Business.Users.Models;
using Stretch4U.Data;
using Stretch4U.Data.Models.Messages;
using Stretch4U.Data.Models.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Stretch4U.Business.Users.Services;

internal class UserService : IUserService
{
    private readonly Stretch4UContext _context;
    private readonly IConfiguration _configuration;

    public UserService(Stretch4UContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<UserData> Signup(SignupRequestDTO request)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Username = request.Username,
            Password = hashedPassword,
            Messages = new List<Message>()
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = GenerateToken();

        var userData = new UserData
        {
            Id = user.Id,
            Username = user.Username,
            Messages = user.Messages,
            Token = token
        };

        return userData;
    }

    public async Task<UserData> Login(LoginRequestDTO request)
    {
        var user = await _context.Users.Include(u => u.Messages)
                                       .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            throw new Exception("Invalid password");
        }

        var oldMessages = await _context.Messages.Where(m => m.UserId == user.Id && m.CreatedAt < DateTime.UtcNow.AddDays(-1))
                                                 .ToListAsync();
        _context.Messages.RemoveRange(oldMessages);
        await _context.SaveChangesAsync();

        var messages = await _context.Messages.Where(m => m.UserId == user.Id)
                                              .ToListAsync();
        user.Messages = messages;

        var token = GenerateToken();

        var userData = new UserData
        {
            Id = user.Id,
            Username = user.Username,
            Messages = user.Messages,
            Token = token
        };

        return userData;
    }

    private JwtToken GenerateToken()
    {
        var issuer = _configuration[ConfigurationConstants.JwtIssuer];
        var audience = _configuration[ConfigurationConstants.JwtAudience];
        var key = _configuration[ConfigurationConstants.JwtKey];

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            signingCredentials: GenerateSigninCredentials(key),
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddYears(1000)
            );

        return new JwtToken
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
    }

    private static SigningCredentials GenerateSigninCredentials(string? key) =>
        new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                       SecurityAlgorithms.HmacSha256Signature);
}
