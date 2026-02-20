using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SENSEI.DOMAIN
{
    public class Lesson
    {
        [DisplayName("Lesson")]
        public long LessonId { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "The Course field is required.")]
        [DisplayName("Course")]
        public long CourseId { get; set; }

        [Required]
        [DisplayName("Lesson Name")]
        public string LessonName { get; set; }

        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; }

        [DisplayName("Encrypted Key")]
        public string EncryptedKey { get; set; }

        // Navigation
        public Course Course { get; set; }
        public ICollection<BatchLesson> BatchLessons { get; set; } = new List<BatchLesson>();
    }
}