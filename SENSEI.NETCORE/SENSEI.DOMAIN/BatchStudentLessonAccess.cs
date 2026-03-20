using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SENSEI.DOMAIN
{
    public class BatchStudentLessonAccess
    {
        [DisplayName("Batch Student Lesson Access")]
        public long BatchStudentLessonAccessId { get; set; }

        [Required]
        [DisplayName("Batch Lesson")]
        public long BatchLessonId { get; set; }

        [Required]
        [DisplayName("Student")]
        public long StudentId { get; set; }

        [DisplayName("Has Access")]
        public bool HasAccess { get; set; }

        [DisplayName("Feedback")]
        public string Feedback { get; set; }

        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; }

        [DisplayName("Rating")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int? Rating { get; set; }

        // Navigation
        public BatchLesson BatchLesson { get; set; }
        public Student Student { get; set; }
        public ICollection<BatchStudentLessonAccessRequest> Requests { get; set; } = new List<BatchStudentLessonAccessRequest>();
    }
}