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

        public Task NotifyAll(object payload)
        {
            return _notificationHub.Clients.All.SendAsync("ReceiveNotification", payload);
        }

        public Task NotifyUsers(IEnumerable<long> userIds, object payload)
        {
            var ids = userIds.Select(x => x.ToString()).ToList();

            return _notificationHub.Clients.Users(ids).SendAsync("ReceiveNotification", payload);
        }


    }
}
