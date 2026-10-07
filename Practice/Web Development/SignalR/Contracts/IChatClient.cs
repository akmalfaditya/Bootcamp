namespace SignalR.Contracts;

public interface IChatClient
{
    Task ReceiveMessage(string user, string message);
    Task ReceiveGroupMessage(string groupName, string user, string message);
}
