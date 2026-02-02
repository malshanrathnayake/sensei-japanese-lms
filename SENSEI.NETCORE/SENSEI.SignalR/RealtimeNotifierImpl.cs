using Microsoft.AspNetCore.SignalR;
using SENSEI.SignalR.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.SignalR
{
    public class RealtimeNotifierImpl : IRealtimeNotifier
    {
        private readonly IHubContext<NotificationHub> _notificationHub;

        public RealtimeNotifierImpl(IHubContext<NotificationHub> notificationHub)
        {
            _notificationHub = notificationHub;
        }

        public Task NotifyUser(long userId, object payload)
        {
            return _notificationHub.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", payload);
        }
    }
}
