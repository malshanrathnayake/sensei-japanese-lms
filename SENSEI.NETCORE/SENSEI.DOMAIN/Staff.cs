using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SENSEI.DOMAIN
{
    public class Staff
    {
        [DisplayName("Employee")]
        public long EmployeeId { get; set; }

        [DisplayName("User")]
        public long UserId { get; set; }

        [Required]
        [DisplayName("Employee Code")]
        public string EmployeeCode { get; set; }

        [Required]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Initials")]
        public string Initials { get; set; }

        [DisplayName("Calling Name")]
        public string CallingName { get; set; }

        [DisplayName("NIC")]
        public string NIC { get; set; }

        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; }

        // Navigation
        public User User { get; set; }
        public ICollection<EmployeeAddress> Addresses { get; set; } = new List<EmployeeAddress>();
    }
}