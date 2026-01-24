    using System;
using System.Collections.Generic;

namespace SENSEI.DOMAIN
{
    public class Lesson
    {
        public long LessonId { get; set; }
        public long CourseId { get; set; }
        public string LessonName { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation
        public Course Course { get; set; }
        public ICollection<BatchLesson> BatchLessons { get; set; } = new List<BatchLesson>();
    }
}