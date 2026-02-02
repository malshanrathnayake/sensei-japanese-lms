using Microsoft.AspNetCore.SignalR;

namespace SENSEI.WEB.SignalR
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirst("UserId")?.Value;
        }
    }
}
