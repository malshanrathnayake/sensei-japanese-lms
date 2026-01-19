using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SENSEI.DOMAIN
{
    public enum UserTypeEnum : int
    {
        [Display(Name = "Admin")]
        Admin = 0,
        [Display(Name = "Student")]
        Student = 1,
        [Display(Name = "Manager")]
        Manager = 2,
    }
}
