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
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));
            var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            var student = await _studentService.GetStudentProfile(userId);
            
            var batchId = student.StudentBatches.FirstOrDefault()?.BatchId ?? 0;
            var (lessons, count) = await _studentService.SearchStudentBatchLessons(studentId, batchId, 0, -1, "");
            
            double averageProgress = 0;
            if (lessons != null && lessons.Any())
            {
                var groupedLessons = lessons.GroupBy(e => e.LessonId);
                double totalProgress = 0;
                foreach (var group in groupedLessons)
                {
                    var totalUnits = group.Count();
                    var completedUnits = group.SelectMany(e => e.StudentBatchLessonViews).Count(v => v.StudentId == studentId && v.IsCompleted);
                    totalProgress += totalUnits > 0 ? (completedUnits / (double)totalUnits) * 100 : 0;
                }
                averageProgress = totalProgress / groupedLessons.Count();
            }

            ViewBag.AverageProgress = Math.Round(averageProgress);
            ViewBag.CourseCount = lessons?.GroupBy(e => e.LessonId).Count() ?? 0;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LessonListView(int start, int length, string searchValue, string filter = "all")
        {
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));
            var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            var student = await _studentService.GetStudentProfile(userId);
            var (lessons, count) = await _studentService.SearchStudentBatchLessons(studentId, student.StudentBatches.FirstOrDefault().BatchId, start, length, searchValue);
            
            var lessonsList = lessons.ToList();
            var resultList = lessonsList;

            if (filter != "all")
            {
                var grouped = lessonsList.GroupBy(e => e.LessonId);
                var validLessonIds = new List<long>();

                foreach (var group in grouped)
                {
                    var totalUnits = group.Count();
                    var completedUnits = group.SelectMany(e => e.StudentBatchLessonViews).Count(v => v.StudentId == studentId && v.IsCompleted);
                    var progress = totalUnits > 0 ? (completedUnits / (double)totalUnits) * 100 : 0;

                    if (filter == "completed" && progress == 100)
                    {
                        validLessonIds.Add(group.Key);
                    }
                    else if (filter == "inprogress" && progress < 100) // "In Progress" includes everything not finished
                    {
                        validLessonIds.Add(group.Key);
                    }
                }
                resultList = lessonsList.Where(e => validLessonIds.Contains(e.LessonId)).ToList();
            }

            resultList.ForEach(e =>
            {
                e.EncryptedKey = _protector.Protect(e.BatchLessonId.ToString());
                e.LessonEncryptedKey = _protector.Protect(e.LessonId.ToString());
            });

            return View(resultList);
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
            var filteredLessons = lessons.Where(e => e.LessonId == lessonId).ToList();
            filteredLessons.ForEach(e => e.EncryptedKey = _protector.Protect(e.BatchLessonId.ToString()));

            return View(filteredLessons);
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
