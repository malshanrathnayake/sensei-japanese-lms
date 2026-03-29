using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.AdminPortalService.Interface
{
    public interface IStudentPaymentService
    {
        Task<(bool, long)> UpdateStudentBatchPayment(StudentBatchPayment studentBatchPayment);
        Task<(IEnumerable<StudentBatchPayment>, long)> SearchStudentBatchPayment(long courseId = 0,long batchId = 0, string indexNumber = "", int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "");
        Task<StudentBatchPayment> GetStudentBatchPayment(long studentBatchPaymentId);
        Task<bool> ApproveStudentBatchPayment(long studentBatchPaymentId, long approvedById, DateTime paymentMonth);
        Task<bool> RejectStudentBatchPayment(long studentBatchPaymentId, long rejectedById);
        Task<IEnumerable<dynamic>> GetStudentBatches(long studentId);

    }
}
