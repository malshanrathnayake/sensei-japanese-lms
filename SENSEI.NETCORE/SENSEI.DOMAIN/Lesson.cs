    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SENSEI.DOMAIN
{
    public class Lesson
    {
        public long LessonId { get; set; }
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "The Course field is required.")]
        public long CourseId { get; set; }
        [Required]
        public string LessonName { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public string EncryptedKey { get; set; }

        // Navigation
        public Course Course { get; set; }
        public ICollection<BatchLesson> BatchLessons { get; set; } = new List<BatchLesson>();
    }
}