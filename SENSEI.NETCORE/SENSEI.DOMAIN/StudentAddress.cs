using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SENSEI.DOMAIN
{
    public class StudentAddress
    {
        [DisplayName("Student Address")]
        public long StudentAddressId { get; set; }

        [Required]
        [DisplayName("Student")]
        public long StudentId { get; set; }

        [Required]
        [DisplayName("Address Line One")]
        public string AddressLineOne { get; set; }

        [DisplayName("Address Line Two")]
        public string AddressLineTwo { get; set; }

        [Required]
        [DisplayName("Country")]
        public int CountryId { get; set; }

        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; }

        [DisplayName("Postal Code")]
        public string? PostalCode { get; set; }

        #region NAVIGATIONAL PROPERTIES
        public Student Student { get; set; }
        public Country Country { get; set; }
        #endregion
    }
}