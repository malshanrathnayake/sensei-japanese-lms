using System;

namespace SENSEI.DOMAIN
{
    public class StudentAddress
    {
        public long StudentAddressId { get; set; }
        public long StudentId { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public string City { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation
        public Student Student { get; set; }
    }
}