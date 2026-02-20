using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class State
    {
        [DisplayName("State")]
        public int StateId { get; set; }

        [Required]
        [DisplayName("Country")]
        public int CountryId { get; set; }

        [Required]
        [DisplayName("State Name")]
        public string StateName { get; set; }

        #region Navigation Properties
        public Country Country { get; set; }
        public ICollection<City> Cities { get; set; }
        #endregion
    }
}
