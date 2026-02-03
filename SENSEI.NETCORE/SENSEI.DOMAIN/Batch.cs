using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SENSEI.DOMAIN
{
    public class Batch
    {
        public long BatchId { get; set; }
        [Required]
        public long CourseId { get; set; }
        [Required]
        public string BatchName { get; set; }
        [Required]
        public DateTime BatchStartDate { get; set; }
        [Required]
        public DateTime BatchEndDate { get; set; }
        public bool IsDeleted { get; set; }
        public string EncryptedKey { get; set; }

        // Navigation
        public Course Course { get; set; }
        public ICollection<BatchLesson> BatchLessons { get; set; } = new List<BatchLesson>();
        public ICollection<StudentBatch> StudentBatches { get; set; } = new List<StudentBatch>();
    }
}