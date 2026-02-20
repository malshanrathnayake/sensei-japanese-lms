using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class Branch
    {
        [DisplayName("Branch")]
        public int BranchId { get; set; }

        [Required]
        [DisplayName("Branch Name")]
        public string BranchName { get; set; }

        [Required]
        [DisplayName("City")]
        public int CityId { get; set; }

        [DisplayName("Contact Number")]
        public string ContactNumber { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Address")]
        public string Address { get; set; }

        #region Navigation Properties
        public City City { get; set; }
        #endregion
    }
}
