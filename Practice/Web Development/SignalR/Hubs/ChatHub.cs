using Microsoft.AspNetCore.SignalR;
using SignalR.Contracts;

namespace SignalR.Hubs;

public class ChatHub(ILogger<ChatHub> logger) : Hub<IChatClient>
{
    public override async Task OnConnectedAsync()
    {
        logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception == null)
        {
            logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
        }
        else
        {
            logger.LogWarning(exception, "Client disconnected: {ConnectionId}", Context.ConnectionId);
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message)
    {
        // Broadcast to all connected clients
        await Clients.All.ReceiveMessage(user, message);
    }

    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName, Context.ConnectionAborted);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName, Context.ConnectionAborted);
    }

    public async Task SendMessageToGroup(string groupName, string user, string message)
    {
        await Clients.Group(groupName).ReceiveGroupMessage(groupName, user, message);
    }
}
