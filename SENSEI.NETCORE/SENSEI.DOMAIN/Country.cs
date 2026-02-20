using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class Country
    {
        [DisplayName("Country")]
        public int CountryId { get; set; }

        [Required]
        [DisplayName("Country Name")]
        public string CountryName { get; set; }

        #region Navigation Properties
        public ICollection<State> States { get; set; }
        #endregion
    }
}
