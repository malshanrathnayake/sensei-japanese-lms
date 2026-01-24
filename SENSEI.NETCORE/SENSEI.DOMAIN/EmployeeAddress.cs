using System;

namespace SENSEI.DOMAIN
{
    public class EmployeeAddress
    {
        public long EmployeeAddressId { get; set; }
        public long EmployeeId { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string City { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation
        public Staff Employee { get; set; }
    }
}