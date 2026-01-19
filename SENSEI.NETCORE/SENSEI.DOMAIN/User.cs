using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class User
    {
        public long UserId { get; set; }
        public string userName { get; set; }
        public Guid UserGlobalidentity { get; set; }
        public DateTime CreatedDateTiime { get; set; }

        public UserTypeEnum UserTypeEnum { get; set; }
        public bool IsActive { get; set; }
        public bool IsSuspend { get; set; }
    }
}
