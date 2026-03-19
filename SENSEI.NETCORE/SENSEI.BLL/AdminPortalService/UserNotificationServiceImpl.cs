using devspark_core_data_access_layer;
using Newtonsoft.Json;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;
using SENSEI.SIGNALR.Interface;
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

        public async Task<IEnumerable<UserNotification>> GetUserNotificationForUser(long userId)
        {
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var notifications = await dataTransactionManager.UserNotificationDataManager.RetrieveData("GetUserNotificationForUser", [
                new Microsoft.Data.SqlClient.SqlParameter("@UserId", userId)
                ]);
            return notifications;
        }

        public async Task<bool> UpdateReadability(long userNotificationId, long userId)
        {
            var userNotificationRead = new UserNotificationRead()
            {
                UserNotificationId = userNotificationId,
                UserId = userId
            };

            string userJsonString = JsonConvert.SerializeObject(userNotificationRead);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (status, primaryKey) = await dataTransactionManager.UserNotificationDataManager.UpdateDataReturnPrimaryKey("UpdateUserNotificationReadability", userJsonString);

            return status;
        }

        public async Task<(bool, long)> UpdateUserNotification(UserNotification userNotification)
        {

            string userJsonString = JsonConvert.SerializeObject(userNotification);
            DataTransactionManager dataTransactionManager = new DataTransactionManager(_databaseService.GetConnectionString());
            var (status, primaryKey) = await dataTransactionManager.UserNotificationDataManager.UpdateDataReturnPrimaryKey("UpdateUserNotification", userJsonString);

            if (userNotification.UserTypeEnum == UserTypeEnum.Admin)
            {
                userNotification.CreatedAt = DateTime.UtcNow;
                await _realtimeNotifier.NotifyUsers(new List<long> { 4, 5, 6 }, userNotification);
            }

            return (status, primaryKey);
        }
    }
}
