using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SENSEI.DOMAIN
{
    public class Student
    {
        [DisplayName("Student")]
        public long StudentId { get; set; }

        [DisplayName("User")]
        public long UserId { get; set; }

        [DisplayName("Index Number")]
        public string IndexNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [DisplayName("Email *")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [DisplayName("Phone Number *")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [DisplayName("First Name *")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [DisplayName("Last Name *")]
        public string LastName { get; set; }

        [DisplayName("Initials")]
        public string Initials { get; set; }

        [DisplayName("Calling Name")]
        public string CallingName { get; set; }

        [DisplayName("NIC")]
        public string NIC { get; set; }

        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; }

        [DisplayName("Student Registration")]
        public long? StudentRegistrationId { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DisplayName("Date Of Birth *")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [DisplayName("Country *")]
        public string CountryCode { get; set; }

        [DisplayName("State / Province / Region")]
        public string State { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [DisplayName("City *")]
        public string City { get; set; }

        [DisplayName("City ID")]
        public int? CityId { get; set; }

        [Required]
        [DisplayName("Branch *")]
        public int BranchId { get; set; }

        [Required]
        [DisplayName("Learning Mode *")]
        public int StudentLearningModeId { get; set; }

        public string StudentPopulatedName
        {
            get
            {
                string populatedName = $"{FirstName} ";
                if (!string.IsNullOrEmpty(MiddleName))
                {
                    populatedName += $"{MiddleName} ";
                }
                populatedName += $"{LastName}";
                return populatedName;
            }
        }

        #region NAVIGATIONAL PROPERTIES

        public User User { get; set; }
        public ICollection<StudentAddress> Addresses { get; set; } = new List<StudentAddress>();
        public ICollection<StudentBatch> StudentBatches { get; set; } = new List<StudentBatch>();
        public ICollection<BatchStudentLessonAccess> LessonAccesses { get; set; } = new List<BatchStudentLessonAccess>();
        public StudentRegistration StudentRegistration { get; set; }
        public City CityNav { get; set; }
        public Branch Branch { get; set; }
        public StudentLearningMode StudentLearningMode { get; set; }

        #endregion
    }
}