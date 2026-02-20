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
        public bool AdminApproved { get; set; }

        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; }

        // Navigation
        public BatchStudentLessonAccess BatchStudentLessonAccess { get; set; }
    }
}