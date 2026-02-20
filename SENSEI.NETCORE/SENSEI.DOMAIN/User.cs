using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class User
    {
        [DisplayName("User")]
        public long UserId { get; set; }

        [DisplayName("User Name")]
        public string userName { get; set; }

        [DisplayName("User Global Identity")]
        public Guid UserGlobalidentity { get; set; }

        [DisplayName("Created Date Time")]
        public DateTime CreatedDateTiime { get; set; }

        [DisplayName("User Type")]
        public UserTypeEnum UserTypeEnum { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [DisplayName("Is Suspend")]
        public bool IsSuspend { get; set; }
    }
}
