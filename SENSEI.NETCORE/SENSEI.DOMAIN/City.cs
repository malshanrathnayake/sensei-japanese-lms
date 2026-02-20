using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class City
    {
        [DisplayName("City")]
        public int CityId { get; set; }

        [Required]
        [DisplayName("State")]
        public int StateId { get; set; }

        [Required]
        [DisplayName("City Name")]
        public string CityName { get; set; }

        #region Navigation Properties
        public State State { get; set; }
        public ICollection<Branch> Branches { get; set; }
        #endregion
    }
}
