using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SENSEI.DOMAIN
{
    public class BatchLesson
    {
        [DisplayName("Batch Lesson")]
        public long BatchLessonId { get; set; }

        [Required]
        [DisplayName("Lesson")]
        public long LessonId { get; set; }

        [Required]
        [DisplayName("Batch")]
        public long BatchId { get; set; }

        [Required]
        [DisplayName("Lesson Start Date Time")]
        public DateTime LessonStartDateTime { get; set; }

        [Required]
        [DisplayName("Lesson End Date Time")]
        public DateTime LessonEndDateTime { get; set; }

        [DisplayName("Recording URL")]
        public string RecordingUrl { get; set; }

        [DisplayName("Recording Expire Date")]
        public DateTime RecordingExpireDate { get; set; }

        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; }

        [DisplayName("Encrypted Key")]
        public string EncryptedKey { get; set; }

        // Navigation
        public Lesson Lesson { get; set; }
        public Batch Batch { get; set; }
        public ICollection<BatchStudentLessonAccess> BatchStudentLessonAccesses { get; set; } = new List<BatchStudentLessonAccess>();
    }
}