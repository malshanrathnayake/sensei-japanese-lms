using System;

namespace SENSEI.DOMAIN
{
    public class EmployeeAddress
    {
        public long EmployeeAddressId { get; set; }
        public long EmployeeId { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public int CityId { get; set; }
        public bool IsDeleted { get; set; }

        #region NAVIGATIONAL PROPERTIES
        public Staff Employee { get; set; }
        public City City { get; set; }
        #endregion
    }
}