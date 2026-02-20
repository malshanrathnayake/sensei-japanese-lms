using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class UserNotification
    {
        [DisplayName("User Notification")]
        public long UserNotificationId { get; set; }

        [Required]
        [DisplayName("Notification Type")]
        public string NotificationType { get; set; }

        [Required]
        [DisplayName("Message")]
        public string Message { get; set; }

        [DisplayName("Is Read")]
        public bool IsRead { get; set; }

        [DisplayName("Created At")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Icon")]
        public string Icon { get; set; }

        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; }

        #region NAVIGATIONAL PROPERTIES
        public ICollection<UserNotificationRead> UserNotificationReads { get; set; }
        #endregion
    }
}
