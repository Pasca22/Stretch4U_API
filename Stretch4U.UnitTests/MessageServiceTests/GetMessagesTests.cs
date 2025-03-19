using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Stretch4U.Business.Messages.Services;
using Stretch4U.Data;
using Stretch4U.Data.Models.Messages;

namespace Stretch4U.UnitTests.MessageServiceTests;

public class GetMessagesTests : BaseTests
{
    private MessageService _service;
    private Mock<DbSet<Message>> _mockSet;
    private List<Message> _data;

    [SetUp]
    public void Setup()
    {
        _data = new List<Message>();
        _mockSet = CreateMockDbSet(_data);

        SetupContext();
        _service = new MessageService(_context.Object, SetupConfiguration());
    }

    private void SetupContext()
    {
        _context = new Mock<Stretch4UContext>();
        _context.Setup(c => c.Messages).Returns(_mockSet.Object);
    }

    private IConfiguration SetupConfiguration()
    {
        var configMapper = new Dictionary<string, string>
        {
            {"ChatBot:Url", "https://model/ai/test"},
            {"ChatBot:Key", "keyadasdadadasadasdadadadadadaadadadaad"},
            {"ChatBot:Model", "model"}
        };
        return new ConfigurationBuilder()
            .AddInMemoryCollection(configMapper)
            .Build();
    }

    [Test]
    public async Task GetMessages_WithValidUser_ReturnsMessages()
    {
        // Arrange
        var message = new Message
        {
            Id = 1,
            UserId = 1,
            Content = "test",
            CreatedAt = DateTime.Now
        };
        _data.Add(message);

        //Act
        var result = await _service.GetMessages(1);

        //Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Content.Should().Be("test");
    }

    [Test]
    public async Task GetMessages_WithInvalidUser_ReturnsEmptyList()
    {
        //Arrange

        //Act
        var result = await _service.GetMessages(1);

        //Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(0);
    }
}
