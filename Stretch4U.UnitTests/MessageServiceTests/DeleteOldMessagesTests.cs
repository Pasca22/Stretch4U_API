using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Stretch4U.Business.Messages.Services;
using Stretch4U.Data;
using Stretch4U.Data.Models.Messages;

namespace Stretch4U.UnitTests.MessageServiceTests;

public class DeleteOldMessagesTests : BaseTests
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
    public async Task DeleteOldMessages_ShouldRemainOnlyMessagesSentInTheLastDay()
    {
        // Arrange
        var message1 = new Message
        {
            Id = 1,
            UserId = 1,
            Content = "test",
            CreatedAt = DateTime.Now
        };

        var message2 = new Message
        {
            Id = 1,
            UserId = 1,
            Content = "test",
            CreatedAt = DateTime.Now.AddDays(-3)
        };

        _data.Add(message1);

        //Act
        await _service.DeleteOldMessages();

        //Assert
        Assert.AreEqual(_data.Count, 1);
    }
}
