using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.AdminPortalService.Interface
{
    public interface IBatchLessonService
    {
        Task<(bool, long)> UpdateBatchLesson(BatchLesson batchLesson);
        Task<BatchLesson> GetBatchLesson(long batchLessonId);
        Task<(IEnumerable<BatchLesson>, long)> SearchBatchLessons(long courseId = 0, long batchId = 0, int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "");
        Task<bool> DeleteBatchLesson(long batchLessonId);
        Task<IEnumerable<BatchLesson>> GetBatchLessons(long batchId = 0);
    }
}
