using Microsoft.EntityFrameworkCore;
using Moq;
using Stretch4U.Data;
using Stretch4U.Business.Users.Services;
using Stretch4U.Data.Models.Users;
using Microsoft.Extensions.Configuration;

namespace Stretch4U.UnitTests.UserServiceTests;

public class GetUserDataTests : BaseTests
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
    public async Task GetUserData_WithValidUser_ReturnsUserData()
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
        var result = await _service.GetUserData(user.Id);

        //Assert
        Assert.AreEqual(user.Id, result.Id);
        Assert.AreEqual(user.Username, result.Username);
    }

    [Test]
    public async Task GetUserData_WithInvalidUser_ThrowsException()
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

        //Assert
        Assert.ThrowsAsync<Exception>(() => _service.GetUserData(2));
    }
}
