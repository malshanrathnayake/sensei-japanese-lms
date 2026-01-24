using System;
using System.Collections.Generic;

namespace SENSEI.DOMAIN
{
    public class StudentBatch
    {
        public long StudentBatchId { get; set; }
        public long BatchId { get; set; }
        public long StudentId { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation
        public Batch Batch { get; set; }
        public Student Student { get; set; }
        public ICollection<StudentBatchPayment> Payments { get; set; } = new List<StudentBatchPayment>();
    }
}