using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;
using SENSEI.SignalR.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SENSEI.BLL.AdminPortalService
{
    public class UserNotificationServiceImpl : IUserNotificationService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IRealtimeNotifier _realtimeNotifier;

        public UserNotificationServiceImpl(IDatabaseService databaseService, IRealtimeNotifier realtimeNotifier)
        {
            _databaseService = databaseService;
            _realtimeNotifier = realtimeNotifier;
        }

        public Task<UserNotification> GetUserNotification(long userNotificationId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserNotification>> GetUserNotificationForUser(long userId)
        {
            IEnumerable<UserNotification> n = new List<UserNotification>();
            return Task.FromResult(n);
        }

        public Task<IEnumerable<UserNotificationRead>> UpdateReadability(long userNotificationId, long userId)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool, long)> UpdateUserNotification(UserNotification userNotification)
        {

            foreach(var user in userNotification.UserNotificationReads)
            {
                await _realtimeNotifier.NotifyUser(user.UserId, new
                {
                    id = userNotification.UserNotificationId,
                    title = userNotification.NotificationType,
                    message = userNotification.Message,
                    icon = userNotification.Icon,
                    createdAt = userNotification.CreatedAt
                });
            }

            throw new NotImplementedException();
        }
    }
}
