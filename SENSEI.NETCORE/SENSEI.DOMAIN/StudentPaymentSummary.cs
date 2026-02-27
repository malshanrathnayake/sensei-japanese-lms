using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class StudentPaymentSummary
    {
        public long StudentBatchId { get; set; }
        public string BatchName { get; set; }
        public string CourseName { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalApproved { get; set; }
    }
}
