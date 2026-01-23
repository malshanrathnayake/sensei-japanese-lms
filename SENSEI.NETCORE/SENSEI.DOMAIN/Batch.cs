using System;
using System.Collections.Generic;

namespace SENSEI.DOMAIN
{
    public class Batch
    {
        public long BatchId { get; set; }
        public long CourseId { get; set; }
        public string BatchName { get; set; }
        public DateTime BatchStartDate { get; set; }
        public DateTime BatchEndDate { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation
        public Course Course { get; set; }
        public ICollection<BatchLesson> BatchLessons { get; set; } = new List<BatchLesson>();
        public ICollection<StudentBatch> StudentBatches { get; set; } = new List<StudentBatch>();
    }
}