using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stretch4U.Business.Messages.Models;
using Stretch4U.Data;
using Stretch4U.Data.Models.Messages;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Stretch4U.Business.Messages.Services;

internal class MessageService : IMessageService
{
    private readonly Stretch4UContext _context;
    private readonly IConfiguration _configuration;
    private static HttpClient _httpClient;

    public MessageService(Stretch4UContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    public async Task<Message> AddMessage(AddMessageRequestDTO request)
    {
        var url = _configuration[ConfigurationConstants.ChatBotUrl];
        var key = _configuration[ConfigurationConstants.ChatBotKey];
        var model = _configuration[ConfigurationConstants.ChatBotModel];

        var userContent = request.Content;
        var systemContent = Constants.SystemContent;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);

        var chatBotRequest = new ChatBotRequest
        {
            Model = model,
            Messages = new List<ChatBotMessage>
            {
                new ChatBotMessage
                {
                    Role = "user",
                    Content = userContent
                },
                new ChatBotMessage
                {
                    Role = "system",
                    Content = systemContent
                }
            }
        };

        var response = await _httpClient.PostAsJsonAsync(url, chatBotRequest);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to communicate with chatbot service: {response.ReasonPhrase}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var chatbotResponse = JsonSerializer.Deserialize<ChatBotResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var assistantContent = chatbotResponse.Choices.FirstOrDefault().Message.Content;

        var message = new Message
        {
            UserId = request.UserId,
            Content = request.Content,
            Response = assistantContent,
            CreatedAt = DateTime.UtcNow,
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return message;
    }

    public async Task<IEnumerable<Message>> GetMessages(int userId)
    {
        var messages = await _context.Messages.Where(m => m.UserId == userId && m.CreatedAt >= DateTime.UtcNow.AddDays(-1))
                                              .OrderByDescending(m => m.CreatedAt)
                                              .ToListAsync();
        return messages;
    }

    public Task DeleteOldMessages()
    {
        var oldMessages = _context.Messages.Where(m => m.CreatedAt < DateTime.UtcNow.AddDays(-1));
        _context.Messages.RemoveRange(oldMessages);
        return _context.SaveChangesAsync();
    }
}
