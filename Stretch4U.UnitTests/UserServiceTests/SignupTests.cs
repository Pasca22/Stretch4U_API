using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Stretch4U.Business.Users.Services;
using Stretch4U.Data.Models.Users;
using Stretch4U.Data;
using Stretch4U.Business.Users.Models;

namespace Stretch4U.UnitTests.UserServiceTests;

public class SignupTests : BaseTests
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
    public async Task Signup_WithValidUser_ReturnsUserData()
    {
        // Arrange
        var request = new SignupRequestDTO
        {
            Username = "test",
            Password = "testASDDS32"
        };

        //Act
        var result = await _service.Signup(request);

        //Assert
        Assert.AreEqual(1, _data.Count);
        Assert.AreEqual(request.Username, _data[0].Username);
        Assert.IsTrue(BCrypt.Net.BCrypt.Verify(request.Password, _data[0].Password));
        Assert.AreEqual(_data[0].Id, result.Id);
        Assert.AreEqual(_data[0].Username, result.Username);
        Assert.IsNotNull(result.Token);
    }

    [Test]
    public async Task Signup_WithExistingUser_ReturnsError()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "test",
            Password = BCrypt.Net.BCrypt.HashPassword("test")
        };
        _data.Add(user);
        var request = new SignupRequestDTO
        {
            Username = "test",
            Password = "test"
        };

        //Act

        //Assert
        Assert.AreEqual(1, _data.Count);
        Assert.ThrowsAsync<Exception>(() => _service.Signup(request));
    }
}
