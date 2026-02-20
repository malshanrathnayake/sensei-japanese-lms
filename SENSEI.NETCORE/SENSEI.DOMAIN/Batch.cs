using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SENSEI.DOMAIN
{
    public class Batch
    {
        [DisplayName("Batch")]
        public long BatchId { get; set; }

        [Required]
        [DisplayName("Course")]
        public long CourseId { get; set; }

        [Required]
        [DisplayName("Batch Name")]
        public string BatchName { get; set; }

        [Required]
        [DisplayName("Start Date")]
        public DateTime BatchStartDate { get; set; }

        [Required]
        [DisplayName("End Date")]
        public DateTime BatchEndDate { get; set; }

        public bool IsDeleted { get; set; }

        public string EncryptedKey { get; set; }

        // Navigation
        public Course Course { get; set; }
        public ICollection<BatchLesson> BatchLessons { get; set; } = new List<BatchLesson>();
        public ICollection<StudentBatch> StudentBatches { get; set; } = new List<StudentBatch>();
    }
}