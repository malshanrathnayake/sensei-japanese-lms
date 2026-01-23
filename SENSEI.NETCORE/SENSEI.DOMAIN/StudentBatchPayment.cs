using System;

namespace SENSEI.DOMAIN
{
    public class StudentBatchPayment
    {
        public long StudentBatchPaymentId { get; set; }
        public long StudentBatchId { get; set; }
        public DateTime PaymentMonth { get; set; }
        public decimal Amount { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation
        public StudentBatch StudentBatch { get; set; }
    }
}