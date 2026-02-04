using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class Branch
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int CityId { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        #region Navigation Properties
        public City City { get; set; }
        #endregion
    }
}
