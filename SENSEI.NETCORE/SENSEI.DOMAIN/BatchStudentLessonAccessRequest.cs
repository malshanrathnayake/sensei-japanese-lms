using System;

namespace SENSEI.DOMAIN
{
    public class BatchStudentLessonAccessRequest
    {
        public long BatchStudentLessonAccessRequestId { get; set; }
        public long BatchStudentLessonAccessId { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime RequestEndDate { get; set; }
        public bool AdminApproved { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation
        public BatchStudentLessonAccess BatchStudentLessonAccess { get; set; }
    }
}