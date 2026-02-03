using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.SignalR.Interface
{
    public interface IRealtimeNotifier
    {
        Task NotifyUser(long userId, object payload);
        Task NotifyAll(object payload);
        Task NotifyUsers(IEnumerable<long> userIds, object payload);
    }
}
