using Flurl.Http.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Stretch4U.Business.Messages.Models;
using Stretch4U.Business.Messages.Services;
using Stretch4U.Data;
using Stretch4U.Data.Models.Messages;

namespace Stretch4U.UnitTests.MessageServiceTests;

public class AddMessageTests : BaseTests
{
    private MessageService _service;
    private Mock<DbSet<Message>> _mockSet;
    private List<Message> _data;
    private HttpTest _httpTest;

    [SetUp]
    public void Setup()
    {
        _data = new List<Message>();
        _mockSet = CreateMockDbSet(_data);

        SetupContext();
        _service = new MessageService(_context.Object, SetupConfiguration());
    }

    [TearDown]
    public void Clean()
    {
        _httpTest.Dispose();
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
            {"ChatBot:Url", "http://fake-api/api/test"},
            {"ChatBot:Key", "keyadasdadadasadasdadadadadadaadadadaad"},
            {"ChatBot:Model", "model"}
        };
        return new ConfigurationBuilder()
            .AddInMemoryCollection(configMapper)
            .Build();
    }

    private HttpTest SetupHttpTest()
    {
        var response = new ChatBotResponse
        {
            Choices = new List<ChatBotChoice>
            {
                new ChatBotChoice
                {
                    Message = new ChatBotResponseMessage
                    {
                        Content = "test"
                    }
                }
            }
        };

        var httpTest = new HttpTest();
        httpTest.ForCallsTo("http://fake-api/api/test")
            .WithVerb(HttpMethod.Post)
            .RespondWith(Newtonsoft.Json.JsonConvert.SerializeObject(response), 200);

        return httpTest;
    }

    [Test]
    public async Task AddMessage_WithValidUser_ReturnsMessage()
    {
        // Arrange
        var request = new AddMessageRequestDTO
        {
            UserId = 1,
            Content = "test"
        };
        var expectedResponse = "test";
        _httpTest = SetupHttpTest();

        // Act
        var result = await _service.AddMessage(request);

        // Assert
        _httpTest.ShouldHaveCalled("http://fake-api/api/test");
        Assert.NotNull(result);
        Assert.AreEqual(expectedResponse, result.Content);
    }
}
