using System;
using System.Collections.Generic;
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
        public int CityId { get; set; }
        public int BranchId { get; set; }
        public int StudentLearningModeId { get; set; }

        #region NAVIGATIONAL PROPERTIES
        public Staff ApprovedBy { get; set; }
        public City City { get; set; }
        public Branch Branch { get; set; }
        public StudentLearningMode StudentLearningMode { get; set; }
        #endregion
    }
}
