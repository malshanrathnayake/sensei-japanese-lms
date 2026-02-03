using System;
using System.Collections.Generic;

namespace SENSEI.DOMAIN
{
    public class Student
    {
        public long StudentId { get; set; }
        public long UserId { get; set; }
        public int IndexNumber { get; set; }
        public string Email { get; set; }
        public int PhoneNo { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Initials { get; set; }
        public string CallingName { get; set; }
        public string NIC { get; set; }
        public bool IsDeleted { get; set; }
        public long? StudentRegistrationId { get; set; }
        public DateTime DateOfBirth { get; set; }

        #region NAVIGATIONAL PROPERTIES

        public User User { get; set; }
        public ICollection<StudentAddress> Addresses { get; set; } = new List<StudentAddress>();
        public ICollection<StudentBatch> StudentBatches { get; set; } = new List<StudentBatch>();
        public ICollection<BatchStudentLessonAccess> LessonAccesses { get; set; } = new List<BatchStudentLessonAccess>();
        public StudentRegistration StudentRegistration { get; set; }

        #endregion
    }
}