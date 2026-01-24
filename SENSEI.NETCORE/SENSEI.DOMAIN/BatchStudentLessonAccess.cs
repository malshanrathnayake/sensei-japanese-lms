using System;
using System.Collections.Generic;

namespace SENSEI.DOMAIN
{
    public class BatchStudentLessonAccess
    {
        public long BatchStudentLessonAccessId { get; set; }
        public long BatchLessonId { get; set; }
        public long StudentId { get; set; }
        public bool HasAccess { get; set; }
        public string Feedback { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation
        public BatchLesson BatchLesson { get; set; }
        public Student Student { get; set; }
        public ICollection<BatchStudentLessonAccessRequest> Requests { get; set; } = new List<BatchStudentLessonAccessRequest>();
    }
}