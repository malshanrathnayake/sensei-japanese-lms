using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SENSEI.DOMAIN
{
    public class EmployeeAddress
    {
        [DisplayName("Employee Address")]
        public long EmployeeAddressId { get; set; }

        [Required]
        [DisplayName("Employee")]
        public long EmployeeId { get; set; }

        [Required]
        [DisplayName("Address Line One")]
        public string AddressLineOne { get; set; }

        [DisplayName("Address Line Two")]
        public string AddressLineTwo { get; set; }

        [Required]
        [DisplayName("City")]
        public int CityId { get; set; }

        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; }

        #region NAVIGATIONAL PROPERTIES
        public Staff Employee { get; set; }
        public City City { get; set; }
        #endregion
    }
}