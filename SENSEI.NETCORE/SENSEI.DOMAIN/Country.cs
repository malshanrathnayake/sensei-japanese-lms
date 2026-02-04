using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        #region Navigation Properties
        public ICollection<State> States { get; set; }
        #endregion
    }
}
