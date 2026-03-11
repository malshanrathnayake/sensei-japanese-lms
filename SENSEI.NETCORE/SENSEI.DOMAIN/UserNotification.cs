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

        [DisplayName("User")]
        public long? UserId { get; set; }

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

        [DisplayName("Read At")]
        public DateTime ReadAt { get; set; }

        [DisplayName("User Type")]
        public UserTypeEnum UserTypeEnum { get; set; }

        [DisplayName("Batch")]
        public long? BatchId { get; set; }

        [DisplayName("Course")]
        public long? CourseId { get; set; }

        public string EncryptedKey { get; set; }

        #region NAVIGATIONAL PROPERTIES
        public User User { get; set; }
        public Batch Batch { get; set; }
        public Course Course { get; set; }
        public ICollection<UserNotificationRead> UserNotificationReads { get; set; }
        #endregion
    }
}
