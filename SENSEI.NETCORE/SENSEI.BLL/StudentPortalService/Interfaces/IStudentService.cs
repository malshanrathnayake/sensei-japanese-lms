using SENSEI.DOMAIN;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SENSEI.BLL.StudentPortalService.Interfaces
{
    public interface IStudentService
    {
        Task<Student> GetStudentProfile(long userId);
        Task<(IEnumerable<Batch>, long)> SearchStudentBatches(long studentId, int start = 0, int length = 10, string searchValue = "", string sortColumn = "batchName", string sortDirection = "ASC");
        Task<(IEnumerable<BatchLesson>, long)> SearchStudentBatchLessons(long studentId, long batchId = 0, int start = 0, int length = 10, string searchValue = "", string sortColumn = "lessonDateTime", string sortDirection = "DESC");
        Task<(IEnumerable<Course>, long)> SearchStudentCourses(long studentId, int start = 0, int length = 10, string searchValue = "", string sortColumn = "courseName", string sortDirection = "ASC");
        Task<(IEnumerable<StudentRegistration>, long)> SearchMyRegistrations(string email, int start = 0, int length = 10);
    }
}
