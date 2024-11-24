namespace Stretch4U.Business.Messages.Models;

public class ChatBotResponse
{
    public List<ChatBotChoice> Choices { get; set; }
}

public class ChatBotChoice
{
    public ChatBotResponseMessage Message { get; set; }
}

public class ChatBotResponseMessage
{
    public string Role { get; set; }
    public string Content { get; set; }
}
