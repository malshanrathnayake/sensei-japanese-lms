using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.StudentPortalService.Interfaces;
using System.Threading.Tasks;

namespace SENSEI.WEB.Areas.StudentPortal.Controllers
{
    [Area("StudentPortal")]
    public class MyLearningController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IDataProtector _protector;

        public MyLearningController
        (
            IStudentService studentService,
            IDataProtectionProvider provider
        )
        {
            _studentService = studentService;
            _protector = provider.CreateProtector("CourseProtector");
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
        public async Task<IActionResult> LessonListView(int start, int length, string searchValue)
        {
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));
            var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            var student = await _studentService.GetStudentProfile(userId);
            var (lessons, count) = await _studentService.SearchStudentBatchLessons(studentId, student.StudentBatches.FirstOrDefault().BatchId, start, length, searchValue);
            lessons.ToList().ForEach(e =>
                    {
                        e.EncryptedKey = _protector.Protect(e.BatchLessonId.ToString());
                        e.LessonEncryptedKey = _protector.Protect(e.LessonId.ToString());
                    });
            return View(lessons);
        }

        public async Task<IActionResult> IndexLesson(string q, string lessonName)
        {
            ViewBag.EncryptedKey = q;
            ViewBag.LessonName = lessonName;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LessonSummaryView(int start = 0, int length = 10, string searchValue = "", string lessonEncryptedKey = "")
        {
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));
            var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            var lessonId = Convert.ToInt64(_protector.Unprotect(lessonEncryptedKey));

            var student = await _studentService.GetStudentProfile(userId);
            var (lessons, count) = await _studentService.SearchStudentBatchLessons(studentId, student.StudentBatches.FirstOrDefault().BatchId, start, length, searchValue);

            lessons.ToList().ForEach(e => e.EncryptedKey = _protector.Protect(e.BatchLessonId.ToString()));

            var lessonRelatedbatchLessons = lessons.Where(e => e.LessonId == lessonId).ToList();

            return View(lessonRelatedbatchLessons);
        }

        [HttpGet]
        public async Task<IActionResult> LessonDetail(string q)
        {
            long batchLessonId = Convert.ToInt64(_protector.Unprotect(q));

            var batchLesson = await _studentService.GetBatchLesson(batchLessonId);
            batchLesson.EncryptedKey = q;
            return View(batchLesson);
        }

        [HttpPost]
        public async Task<IActionResult> MarkBatchLessonComplete(string q)
        {
            long batchLessonId = Convert.ToInt64(_protector.Unprotect(q));
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));
            var status = await _studentService.UpdateStudentProgress(batchLessonId, studentId);

            if (status)
            {
                return Json(new { success = status, message = "Lesson marked as complete" });
            }
            else
            {
                return Json(new { success = status, message = "Lesson mot marked as complete" });
            }
        }
    }
}
