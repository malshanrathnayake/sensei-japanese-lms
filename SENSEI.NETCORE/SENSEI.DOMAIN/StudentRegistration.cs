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
    }
}
