using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SENSEI.DOMAIN
{
    public class Course
    {
        [DisplayName("Course")]
        public long CourseId { get; set; }

        [Required]
        [DisplayName("Course Name")]
        public string CourseName { get; set; }

        [Required]
        [DisplayName("Course Code")]
        public string CourseCode { get; set; }

        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; }

        [DisplayName("Encrypted Key")]
        public string EncryptedKey { get; set; }

        // Navigation
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public ICollection<Batch> Batches { get; set; } = new List<Batch>();
    }
}