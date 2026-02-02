using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.AdminPortalService.Interface
{
    public interface IUserNotificationService
    {
        Task<(bool, long)> UpdateUserNotification(UserNotification userNotification);
        Task<UserNotification> GetUserNotification(long userNotificationId);
        Task<IEnumerable<UserNotification>> GetUserNotificationForUser(long userId);
        Task<IEnumerable<UserNotificationRead>> UpdateReadability(long userNotificationId, long userId);
    }
}
