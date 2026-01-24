using System;
using System.Collections.Generic;

namespace SENSEI.DOMAIN
{
    public class Course
    {
        public long CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public ICollection<Batch> Batches { get; set; } = new List<Batch>();
    }
}