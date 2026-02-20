using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class StudentLearningMode
    {
        [DisplayName("Student Learning Mode")]
        public int StudentLearningModeId { get; set; }

        [Required]
        [DisplayName("Learning Mode Name")]
        public string LearningModeName { get; set; }
    }
}
