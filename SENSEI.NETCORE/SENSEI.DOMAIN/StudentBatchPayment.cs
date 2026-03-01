using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SENSEI.DOMAIN
{
    public class StudentBatchPayment
    {
        [DisplayName("Student Batch Payment")]
        public long StudentBatchPaymentId { get; set; }

        [Required]
        [DisplayName("Student Batch")]
        public long StudentBatchId { get; set; }

        [Required]
        [DisplayName("Payment Month")]
        public DateTime PaymentMonth { get; set; }

        [Required]
        [DisplayName("Amount")]
        public decimal Amount { get; set; }

        public DateTime? PaymentDate { get; set; }
        public string SlipUrl { get; set; }
        public bool IsApproved { get; set; }
        public long? ApprovedById { get; set; }

        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; }

        public IFormFile SlipImage { get; set; }

        // Navigation
        public StudentBatch StudentBatch { get; set; }
    }
}