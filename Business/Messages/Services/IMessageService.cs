using Stretch4U.Business.Messages.Models;
using Stretch4U.Data.Models.Messages;

namespace Stretch4U.Business.Messages.Services;

public interface IMessageService
{
    Task<Message> AddMessage(AddMessageRequestDTO request);
    Task<IEnumerable<Message>> GetMessages(int userId);
    Task DeleteOldMessages();
}
