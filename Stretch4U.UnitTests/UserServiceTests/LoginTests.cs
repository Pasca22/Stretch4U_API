using Microsoft.EntityFrameworkCore;
using Moq;
using Stretch4U.Data;
using Stretch4U.Business.Users.Services;
using Stretch4U.Data.Models.Users;
using Stretch4U.Business.Users.Models;
using Microsoft.Extensions.Configuration;

namespace Stretch4U.UnitTests.UserServiceTests;

[TestFixture]
public class LoginTests : BaseTests
{
    private UserService _service;
    private Mock<DbSet<User>> _mockSet;
    private List<User> _data;

    [SetUp]
    public void Setup()
    {
        _data = new List<User>();
        _mockSet = CreateMockDbSet(_data);

        SetupContext();
        _service = new UserService(_context.Object, SetupConfiguration());
    }

    private void SetupContext()
    {
        _context = new Mock<Stretch4UContext>();
        _context.Setup(c => c.Users).Returns(_mockSet.Object);
    }

    private IConfiguration SetupConfiguration()
    {
        var configMapper = new Dictionary<string, string>
        {
            {"Jwt:Issuer", "issuer"},
            {"Jwt:Key", "keyadasdadadasadasdadadadadadaadadadaad"},
            {"Jwt:Audience", "audience"}
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(configMapper)
            .Build();
    }

    [Test]
    public async Task Login_WithValidUser_ReturnsUserData()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "test",
            Password = BCrypt.Net.BCrypt.HashPassword("test")
        };
        _data.Add(user);

        //Act
        var request = new LoginRequestDTO
        {
            Username = "test",
            Password = "test"
        };
        var result = await _service.Login(request);

        //Assert
        Assert.NotNull(result);
        Assert.AreEqual(result.Id, user.Id);
        Assert.AreEqual(result.Username, user.Username);
    }

    [Test]
    public async Task Login_WithInvalidUser_ThrowsException()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "test",
            Password = BCrypt.Net.BCrypt.HashPassword("test")
        };
        _data.Add(user);

        //Act
        var request = new LoginRequestDTO
        {
            Username = "test",
            Password = "wrong"
        };

        //Assert
        Assert.ThrowsAsync<Exception>(() => _service.Login(request));
    }
}
