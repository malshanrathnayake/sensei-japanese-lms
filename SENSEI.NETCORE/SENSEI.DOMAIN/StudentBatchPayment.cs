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

        [DisplayName("Payment Date")]
        public DateTime? PaymentDate { get; set; }

        [DisplayName("Slup URL")]
        public string SlipUrl { get; set; }

        [DisplayName("Approved")]
        public bool IsApproved { get; set; }

        [DisplayName("Approved By")]
        public long? ApprovedById { get; set; }

        [DisplayName("Changed Date")]
        public DateTime? ChangeDateTIme { get; set; }

        [DisplayName("Rejected")]
        public bool IsRejected { get; set; }

        [DisplayName("Is Deleted")]
        public bool IsDeleted { get; set; }

        public IFormFile SlipImage { get; set; }
        public string EncryptedKey { get; set; }

        // Navigation
        public StudentBatch StudentBatch { get; set; }
        public Staff ApprovedBy { get; set; }
    }
}