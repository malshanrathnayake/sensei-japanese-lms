using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.StudentPortalService.Interfaces;
using System.Threading.Tasks;

namespace SENSEI.WEB.Areas.StudentPortal.Controllers
{
    [Area("StudentPortal")]
    public class MyLearningController : Controller
    {
        private readonly IStudentService _studentService;

        public MyLearningController
        (
            IStudentService studentService
        )
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        {
            //var start = Request.Form["start"].FirstOrDefault();
            //var length = Request.Form["length"].FirstOrDefault();

            //int skip = start != null ? Convert.ToInt32(start) : 0;
            //int pageSize = length != null ? Convert.ToInt32(length) : 10;
            //string searchValue = "";

            //var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));
            //var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            //var student = await _studentService.GetStudentProfile(userId);

            //var lessons = await _studentService.SearchStudentBatchLessons(studentId, student.StudentBatches.FirstOrDefault().BatchId, skip, pageSize, searchValue);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LessonSummaryView(int start, int length, string searchValue)
        {
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));
            var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            var student = await _studentService.GetStudentProfile(userId);
            var (lessons, count) = await _studentService.SearchStudentBatchLessons(studentId, student.StudentBatches.FirstOrDefault().BatchId, start, length, searchValue);
            return View(lessons);
        }

        [HttpGet]
        public async Task<IActionResult> LessonDetail(int batchLessonId)
        {
            var lesson = await _studentService.GetBatchLesson(batchLessonId);
            return View(lesson);
        }
    }
}
