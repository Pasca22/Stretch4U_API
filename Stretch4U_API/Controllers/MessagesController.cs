using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stretch4U.API.Requests.Messages;
using Stretch4U.Business.Messages.Services;

namespace Stretch4U.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class MessagesController : Controller
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddMessage([FromBody] AddMessageRequest request)
    {
        try
        {
            var result = await _messageService.AddMessage(request.ToAddMessageRequestDTO());
            return Ok(result);
        }
        catch (Exception)
        {
            return new BadRequestObjectResult("Failed to add message");
        }
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetMessages(int userId)
    {
        try
        {
            var result = await _messageService.GetMessages(userId);
            return Ok(result);
        }
        catch (Exception)
        {
            return new BadRequestObjectResult("Failed to get messages");
        }
    }
}
