using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SENSEI.DOMAIN
{
    public class StudentBatch
    {
        [DisplayName("Student Batch")]
        public long StudentBatchId { get; set; }

        [Required]
        [DisplayName("Batch")]
        public long BatchId { get; set; }

        [Required]
        [DisplayName("Student")]
        public long StudentId { get; set; }

        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; }

        // Navigation
        public Batch Batch { get; set; }
        public Student Student { get; set; }
        public ICollection<StudentBatchPayment> Payments { get; set; } = new List<StudentBatchPayment>();
    }
}