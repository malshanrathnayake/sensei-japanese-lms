using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SENSEI.DOMAIN
{
    public class BatchStudentLessonAccessRequest
    {
        [DisplayName("Batch Student Lesson Access Request")]
        public long BatchStudentLessonAccessRequestId { get; set; }

        [Required]
        [DisplayName("Batch Student Lesson Access")]
        public long BatchStudentLessonAccessId { get; set; }

        [Required]
        [DisplayName("Requested Date")]
        public DateTime RequestedDate { get; set; }

        [Required]
        [DisplayName("Request End Date")]
        public DateTime RequestEndDate { get; set; }

        [DisplayName("Admin Approved")]
        public ApproveStatusEnum ApproveStatusEnum { get; set; }

        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; }

        [DisplayName("Changed By")]
        public long? ChangeById { get; set; }

        [DisplayName("Changed Date")]
        public DateTime? ChangedDate { get; set; }

        public string EncryptedKey { get; set; }

        // Navigation
        public BatchStudentLessonAccess BatchStudentLessonAccess { get; set; }
    }
}