using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.AdminPortalService.Interface
{
    public interface ILessonService
    {
        Task<(bool, long)> UpdateLesson(Lesson lesson);
        Task<Lesson> GetLesson(long lessonId);
        Task<(IEnumerable<Lesson>, long)> SearchLessons(long courseId = 0, int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "");
        Task<bool> DeleteLesson(long lessonId);
        Task<IEnumerable<Lesson>> GetLessons(int courseId = 0);
    }
}
