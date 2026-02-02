using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class UserNotification
    {
        public long UserNotificationId { get; set; }
        public string NotificationType { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Icon { get; set; }
        public bool IsDeleted { get; set; }

        #region NAVIGATIONAL PROPERTIES
        public ICollection<UserNotificationRead> UserNotificationReads { get; set; }
        #endregion
    }
}
