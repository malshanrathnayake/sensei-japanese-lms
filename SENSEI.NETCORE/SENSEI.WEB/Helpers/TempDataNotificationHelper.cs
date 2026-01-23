using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using SENSEI.DOMAIN;

namespace SENSEI.WEB.Helpers
{
    public static class TempDataNotificationHelper
    {
        private const string Key = "Notifications";

        public static void AddNotification(
            this ITempDataDictionary tempData,
            NotificationMessage notification)
        {
            var notifications = new List<NotificationMessage>();

            if (tempData.ContainsKey(Key))
            {
                notifications = JsonConvert.DeserializeObject<List<NotificationMessage>>(tempData[Key].ToString());
            }

            notifications.Add(notification);

            tempData[Key] = JsonConvert.SerializeObject(notifications);
        }
    }
}
