using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.AdminPortalService.Interface
{
    public interface IBatchStudentLessonService
    {
        Task<(IEnumerable<BatchStudentLessonAccessRequest>, long)> SearchBatchStudentLesson(long courseId = 0, long batchId = 0, string indexNumber = "", int status = -1, int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "");
        Task<BatchStudentLessonAccessRequest> GetBatchStudentLessonRequest(long batchStudentLessonRequestId);
        Task<bool> ApproveBatchStudentLessonRequest(long batchStudentLessonRequestId, long userId, DateTime? accessEndDate = null);
        Task<(long pending, long approved, long total)> GetLessonRequestStats();
    }
}
