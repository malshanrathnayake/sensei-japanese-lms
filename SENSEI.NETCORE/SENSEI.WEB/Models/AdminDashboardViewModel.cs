using SENSEI.DOMAIN;
using System.Collections.Generic;

namespace SENSEI.WEB.Models
{
    public class AdminDashboardViewModel
    {
        public long TotalStudents { get; set; }
        public long TotalCourses { get; set; }
        public long TotalBatches { get; set; }
        public long PendingRegistrations { get; set; }
        public long PendingPayments { get; set; }
        
        public IEnumerable<StudentRegistration> RecentRegistrations { get; set; }
        public IEnumerable<Course> CourseSummary { get; set; }
    }
}
