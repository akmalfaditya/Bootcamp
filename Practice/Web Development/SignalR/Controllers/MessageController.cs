using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Contracts;
using SignalR.Hubs;
using SignalR.Models;

namespace SignalR.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController(IHubContext<ChatHub, IChatClient> hubContext) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> BroadcastMessage([FromBody] MessageDto payload, CancellationToken cancellationToken)
    {
        await hubContext.Clients.All.ReceiveMessage(payload.User, payload.Message);
        return Ok();
    }

    [HttpPost("group/{groupName}")]
    public async Task<IActionResult> BroadcastToGroup(string groupName, [FromBody] MessageDto payload, CancellationToken cancellationToken)
    {
        await hubContext.Clients.Group(groupName).ReceiveGroupMessage(groupName, payload.User, payload.Message);
        return Ok();
    }
}
