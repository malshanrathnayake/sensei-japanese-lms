using System;
using System.Collections.Generic;

namespace SENSEI.DOMAIN
{
    public class Staff
    {
        public long EmployeeId { get; set; }
        public long UserId { get; set; }
        public string EmployeeCode { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Initials { get; set; }
        public string CallingName { get; set; }
        public string NIC { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation
        public User User { get; set; }
        public ICollection<EmployeeAddress> Addresses { get; set; } = new List<EmployeeAddress>();
    }
}