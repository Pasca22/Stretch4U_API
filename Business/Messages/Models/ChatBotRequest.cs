namespace Stretch4U.Business.Messages.Models;

internal class ChatBotRequest
{
    public string Model { get; set; }
    public IList<ChatBotMessage> Messages { get; set; }
}
