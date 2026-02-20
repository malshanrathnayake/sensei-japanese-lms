using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class UserNotificationRead
    {
        [DisplayName("User Notification Read")]
        public long UserNotificationReadId { get; set; }

        [Required]
        [DisplayName("User Notification")]
        public long UserNotificationId { get; set; }

        [Required]
        [DisplayName("User")]
        public long UserId { get; set; }

        [DisplayName("Is Read")]
        public bool IsRead { get; set; }

        [DisplayName("Read At")]
        public DateTime ReadAt { get; set; }

        #region NAVIGATIONAL PROPERTIES
        public UserNotification UserNotification { get; set; }
        #endregion
    }
}
