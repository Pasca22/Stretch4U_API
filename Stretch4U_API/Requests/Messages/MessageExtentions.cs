using Stretch4U.Business.Messages.Models;

namespace Stretch4U.API.Requests.Messages;

internal static class MessageExtentions
{
    public static AddMessageRequestDTO ToAddMessageRequestDTO(this AddMessageRequest request) =>
        new AddMessageRequestDTO
        {
            UserId = request.UserId,
            Content = request.Content
        };
}
