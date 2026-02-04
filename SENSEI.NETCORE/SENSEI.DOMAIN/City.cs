using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class City
    {
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string CityName { get; set; }

        #region Navigation Properties
        public State State { get; set; }
        public ICollection<Branch> Branches { get; set; }
        #endregion
    }
}
