using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class StudentPaymentSummary
    {
        public long StudentBatchId { get; set; }
        [DisplayName("Batch Name")]
        public string BatchName { get; set; }

        [DisplayName("Course Name")]
        public string CourseName { get; set; }

        [DisplayName("Total Paid")]
        public decimal TotalPaid { get; set; }

        [DisplayName("Approved Total")]
        public decimal TotalApproved { get; set; }
    }
}
