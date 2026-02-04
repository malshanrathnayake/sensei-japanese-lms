using System;

namespace SENSEI.DOMAIN
{
    public class StudentAddress
    {
        public long StudentAddressId { get; set; }
        public long StudentId { get; set; }
        public string AddressLineOne { get; set; }
        public string AddressLineTwo { get; set; }
        public int CityId { get; set; }
        public bool IsDeleted { get; set; }

        #region NAVIGATIONAL PROPERTIES
        public Student Student { get; set; }
        public City City { get; set; }
        #endregion
    }
}