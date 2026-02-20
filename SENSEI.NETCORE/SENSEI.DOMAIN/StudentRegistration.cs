using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class StudentRegistration
    {
        [DisplayName("Student Registration")]
        public long StudentRegistrationId { get; set; }

        [Required]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Phone No")]
        public int PhoneNo { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Initials")]
        public string Initials { get; set; }

        [DisplayName("Calling Name")]
        public string CallingName { get; set; }

        [Required]
        [DisplayName("NIC")]
        public string NIC { get; set; }

        [Required]
        [DisplayName("Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Is Approved")]
        public bool IsApproved { get; set; }

        [DisplayName("Approved By")]
        public long ApprovedById { get; set; }

        [Required]
        [DisplayName("Address Line One")]
        public string AddressLineOne { get; set; }

        [DisplayName("Address Line Two")]
        public string AddressLineTwo { get; set; }

        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "The City field is required.")]
        [DisplayName("City")]
        public int CityId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "The Branch field is required.")]
        [DisplayName("Branch")]
        public int BranchId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "The Learning Mode field is required.")]
        [DisplayName("Learning Mode")]
        public int StudentLearningModeId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "The Course field is required.")]
        [DisplayName("Course")]
        public long CourseId { get; set; }

        public string StudentRegistrationPopulatedName
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

        public string EncryptedKey { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CountryId { get; set; }

        #region NAVIGATIONAL PROPERTIES
        public Staff ApprovedBy { get; set; }
        public City City { get; set; }
        public Branch Branch { get; set; }
        public StudentLearningMode StudentLearningMode { get; set; }
        public Course Course { get; set; }
        #endregion
    }
}
