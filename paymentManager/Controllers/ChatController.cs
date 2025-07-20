using Microsoft.AspNetCore.Mvc;
using paymentManager.DTOs;
using paymentManager.Services;

namespace paymentManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] ChatMessageDto dto)
    {
        var msg = await _chatService.SendMessageAsync(dto);
        return Ok(msg);
    }

    [HttpGet("conversation")]
    public async Task<IActionResult> GetConversation([FromQuery] int user1Id, [FromQuery] int user2Id)
    {
        var conversation = await _chatService.GetConversationAsync(user1Id, user2Id);
        return Ok(conversation);
    }
}
