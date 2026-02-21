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
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [DisplayName("Last Name *")]
        public string LastName { get; set; }

        [DisplayName("Initials")]
        public string Initials { get; set; }

        [DisplayName("Calling Name")]
        public string CallingName { get; set; }

        [DisplayName("NIC")]
        public string NIC { get; set; } // User didn't specify if NIC is required, but current model has it. I'll leave as is but maybe make it optional for foreign students? Actually current model has it required.

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DisplayName("Date Of Birth *")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Is Approved")]
        public bool IsApproved { get; set; }

        [DisplayName("Approved By")]
        public long ApprovedById { get; set; }

        [Required(ErrorMessage = "Address Line 1 is required.")]
        [DisplayName("Address Line One *")]
        public string AddressLineOne { get; set; }

        [DisplayName("Address Line Two")]
        public string AddressLineTwo { get; set; }

        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        [DisplayName("State / Province / Region")]
        public string State { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [DisplayName("City *")]
        public string City { get; set; }

        [DisplayName("City ID")]
        public int? CityId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "The Branch field is required.")]
        [DisplayName("Branch *")]
        public int BranchId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "The Learning Mode field is required.")]
        [DisplayName("Learning Mode *")]
        public int StudentLearningModeId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "The Course field is required.")]
        [DisplayName("Course *")]
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

        [Required(ErrorMessage = "Country is required.")]
        [DisplayName("Country *")]
        public int CountryId { get; set; }

        #region NAVIGATIONAL PROPERTIES
        public Staff ApprovedBy { get; set; }
        public City CityNav { get; set; }
        public Branch Branch { get; set; }
        public StudentLearningMode StudentLearningMode { get; set; }
        public Course Course { get; set; }
        #endregion
    }
}
