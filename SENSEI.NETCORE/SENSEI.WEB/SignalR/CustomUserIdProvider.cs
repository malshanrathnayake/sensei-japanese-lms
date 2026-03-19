using Microsoft.AspNetCore.SignalR;

namespace SENSEI.WEB.SIGNALR
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirst("UserId")?.Value;
        }
    }
}
