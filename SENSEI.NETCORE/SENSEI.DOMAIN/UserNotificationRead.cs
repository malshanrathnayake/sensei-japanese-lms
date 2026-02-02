using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class UserNotificationRead
    {
        public long UserNotificationReadId { get; set; }
        public long UserNotificationId { get; set; }
        public long UserId { get; set; }
        public bool IsRead { get; set; }
        public DateTime ReadAt { get; set; }

        #region NAVIGATIONAL PROPERTIES
        public UserNotification UserNotification { get; set; }
        #endregion
    }
}
