using System;
using System.Collections.Generic;

namespace SENSEI.DOMAIN
{
    public class BatchLesson
    {
        public long BatchLessonId { get; set; }
        public long LessonId { get; set; }
        public long BatchId { get; set; }
        public DateTime LessonStartDateTime { get; set; }
        public DateTime LessonEndDateTime { get; set; }
        public string RecordingUrl { get; set; }
        public DateTime RecordingExpireDate { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation
        public Lesson Lesson { get; set; }
        public Batch Batch { get; set; }
        public ICollection<BatchStudentLessonAccess> BatchStudentLessonAccesses { get; set; } = new List<BatchStudentLessonAccess>();
    }
}