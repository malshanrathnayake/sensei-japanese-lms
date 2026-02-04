using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class State
    {
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string StateName { get; set; }

        #region Navigation Properties
        public Country Country { get; set; }
        public ICollection<City> Cities { get; set; }
        #endregion
    }
}
