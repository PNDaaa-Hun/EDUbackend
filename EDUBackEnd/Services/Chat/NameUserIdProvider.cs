using Microsoft.AspNetCore.SignalR;

namespace EDUBackEnd.Services.Chat
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst("sub")?.Value ?? string.Empty;
        }
    }
}
