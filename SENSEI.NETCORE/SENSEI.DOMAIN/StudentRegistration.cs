using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class StudentRegistration
    {
        public long StudentRegistrationId { get; set; }
        public string Email { get; set; }
        public int PhoneNo { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Initials { get; set; }
        public string CallingName { get; set; }
        public string NIC { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsApproved { get; set; }
        public long ApprovedById { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string PostalCode { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "The City field is required.")]
        public int CityId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "The Branch field is required.")]
        public int BranchId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "The Learning Mode field is required.")]
        public int StudentLearningModeId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "The Course field is required.")]
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
