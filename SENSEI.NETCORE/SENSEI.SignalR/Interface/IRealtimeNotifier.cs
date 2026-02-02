using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.SignalR.Interface
{
    public interface IRealtimeNotifier
    {
        Task NotifyUser(long userId, object payload);
    }
}
