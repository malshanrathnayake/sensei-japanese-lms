using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.StudentPortalService.Interfaces;
using SENSEI.DOMAIN;
using SENSEI.WEB.Helpers;
using System.Threading.Tasks;

namespace SENSEI.WEB.Areas.StudentPortal.Controllers
{
    [Area("StudentPortal")]
    [Authorize(Roles = "Student")]
    public class MyLearningController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IDataProtector _protector;
        private readonly SENSEI.BLL.AdminPortalService.Interface.IUserNotificationService _userNotificationService;

        public MyLearningController
        (
            IStudentService studentService,
            IDataProtectionProvider provider,
            SENSEI.BLL.AdminPortalService.Interface.IUserNotificationService userNotificationService
        )
        {
            _studentService = studentService;
            _protector = provider.CreateProtector("CourseProtector");
            _userNotificationService = userNotificationService;
        }

        public async Task<IActionResult> Index()
        {
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));
            var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            var student = await _studentService.GetStudentProfile(userId);
            
            if (student == null || student.StudentBatches == null || !student.StudentBatches.Any())
            {
                ViewBag.AverageProgress = 0;
                ViewBag.CourseCount = 0;
                return View();
            }

            var allLessons = new List<BatchLesson>();
            foreach (var sb in student.StudentBatches)
            {
                var (batchLessons, _) = await _studentService.SearchStudentBatchLessons(studentId, sb.BatchId, 0, -1, "");
                if (batchLessons != null) allLessons.AddRange(batchLessons);
            }
            
            double averageProgress = 0;
            if (allLessons.Any())
            {
                var groupedLessons = allLessons.GroupBy(e => e.LessonId);
                List<double> progressList = new List<double>();
                
                foreach (var group in groupedLessons)
                {
                    var totalUnits = group.Count();
                    var completedUnits = group.SelectMany(e => e.StudentBatchLessonViews).Count(v => v.StudentId == studentId && v.IsCompleted);
                    var progress = totalUnits > 0 ? (completedUnits / (double)totalUnits) * 100 : 0;

                    if (progress > 0) progressList.Add(progress);
                }
                averageProgress = progressList.Any() ? progressList.Average() : 0;
            }

            ViewBag.AverageProgress = Math.Round(averageProgress);
            ViewBag.CourseCount = allLessons.GroupBy(e => e.Lesson.CourseId).Count();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LessonListView(int start, int length, string searchValue, string filter = "all")
        {
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));
            var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            var student = await _studentService.GetStudentProfile(userId);
            
            var allLessons = new List<BatchLesson>();
            if (student?.StudentBatches != null)
            {
                foreach (var sb in student.StudentBatches)
                {
                    // Fetch lessons for each batch. We pass search value to handle filtering at DB level where possible.
                    var (batchLessons, _) = await _studentService.SearchStudentBatchLessons(studentId, sb.BatchId, 0, -1, searchValue);
                    if (batchLessons != null) allLessons.AddRange(batchLessons);
                }
            }

            var resultList = allLessons;

            if (filter != "all")
            {
                var grouped = allLessons.GroupBy(e => e.LessonId);
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
                    else if (filter == "inprogress" && progress < 100) 
                    {
                        validLessonIds.Add(group.Key);
                    }
                }
                resultList = allLessons.Where(e => validLessonIds.Contains(e.LessonId)).ToList();
            }

            // Apply pagination if length is specified
            if (length > 0)
            {
                resultList = resultList.Skip(start).Take(length).ToList();
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
        public async Task<IActionResult> LessonSummaryView(int start = 0, int length = 2147483647, string searchValue = "", string lessonEncryptedKey = "")
        {
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));
            var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            var lessonId = Convert.ToInt64(_protector.Unprotect(lessonEncryptedKey));

            var student = await _studentService.GetStudentProfile(userId);
            
            var allLessons = new List<BatchLesson>();
            if (student?.StudentBatches != null)
            {
                foreach (var sb in student.StudentBatches)
                {
                    var (batchLessons, _) = await _studentService.SearchStudentBatchLessons(studentId, sb.BatchId, 0, -1, searchValue);
                    if (batchLessons != null) allLessons.AddRange(batchLessons);
                }
            }

            var filteredLessons = allLessons.Where(e => e.LessonId == lessonId).ToList();
            
            filteredLessons.ForEach(e => e.EncryptedKey = _protector.Protect(e.BatchLessonId.ToString()));
            filteredLessons.ForEach(e => e.BatchStudentLessonAccesses.ToList().ForEach(a => a.EncryptedKey = _protector.Protect(a.BatchStudentLessonAccessId.ToString())));

            return View(filteredLessons);
        }

        [HttpGet]
        public async Task<IActionResult> LessonDetail(string q)
        {
            long batchLessonId = Convert.ToInt64(_protector.Unprotect(q));
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));

            // Use SearchStudentBatchLessons to ensure we get Access and Request data correctly populated
            var (lessons, _) = await _studentService.SearchStudentBatchLessons(studentId);
            var batchLesson = lessons.FirstOrDefault(e => e.BatchLessonId == batchLessonId);
            
            if (batchLesson == null) return RedirectToAction("Index");

            // Check for individual temporary access extension
            var myAccess = batchLesson.BatchStudentLessonAccesses?.FirstOrDefault(a => a.StudentId == studentId);
            var latestApprovedRequest = myAccess?.BatchStudentLessonAccessRequests?
                .Where(r => r.ApproveStatusEnum == ApproveStatusEnum.Approved && r.RequestEndDate > DateTime.Now)
                .OrderByDescending(r => r.RequestEndDate)
                .FirstOrDefault();

            var hasTemporaryAccess = latestApprovedRequest != null;
            var isGloballyExpired = batchLesson.RecordingExpireDate < DateTime.Now;

            // Strict Expiry Enforcement (Allow if NOT globally expired OR has active temporary access)
            if (isGloballyExpired && !hasTemporaryAccess)
            {
                TempData["ErrorMessage"] = "This lesson recording has expired and is no longer available.";
                return RedirectToAction("Index");
            }

            // Also check access for safety
            var hasPaidAccess = batchLesson.BatchStudentLessonAccesses?.Any(e => e.StudentId == studentId && e.BatchLessonId == batchLessonId && e.HasAccess) ?? false;
            var isFirstWeekTrial = batchLesson.Batch != null && batchLesson.LessonDateTime <= batchLesson.Batch.BatchStartDate.AddDays(7);

            if (!hasPaidAccess && !isFirstWeekTrial && !hasTemporaryAccess)
            {
                TempData["ErrorMessage"] = "You do not have access to this lesson.";
                return RedirectToAction("Index");
            }

            batchLesson.EncryptedKey = q;
            return View(batchLesson);
        }

        [HttpGet]
        public async Task<IActionResult> MarkBatchLessonComplete(string q)
        {
            long batchLessonId = Convert.ToInt64(_protector.Unprotect(q));
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));
            //var status = await _studentService.UpdateStudentProgress(batchLessonId, studentId);

            //if (status)
            //{
            //    return Json(new { success = status, message = "Lesson marked as complete" });
            //}
            //else
            //{
            //    return Json(new { success = status, message = "Lesson mot marked as complete" });
            //}

            var batchLesson = await _studentService.GetBatchLesson(batchLessonId);

            var batchStudentLessonAccessesId = batchLesson.BatchStudentLessonAccesses.Where(e => e.BatchLessonId == batchLessonId && e.StudentId == studentId).Select(e => e.BatchStudentLessonAccessId).FirstOrDefault();


            var batchStudentLessonAccess = new BatchStudentLessonAccess
            {
                BatchStudentLessonAccessId = batchStudentLessonAccessesId,
                BatchLessonId = batchLessonId,
                StudentId = studentId,
            };

            return View(batchStudentLessonAccess);
        }

        [HttpPost]
        public async Task<IActionResult> MarkBatchLessonComplete(BatchStudentLessonAccess batchStudentLessonAccess)
        {
            long batchLessonId = batchStudentLessonAccess.BatchLessonId;
            var studentId = batchStudentLessonAccess.StudentId;

            var batchLesson = await _studentService.GetBatchLesson(batchLessonId);
            if (batchLesson == null) return Json(new { success = false, message = "Lesson not marked as complete" });

            var studentBatchLessonView = new StudentBatchLessonView()
            {
                StudentId = studentId,
                BatchLessonId = batchLessonId,
                LessonId = batchLesson.LessonId,
                IsCompleted = true,
                BatchStudentLessonAccess = batchStudentLessonAccess
            };

            var status = await _studentService.UpdateStudentProgress(studentBatchLessonView);

            if (status)
            {
                return Json(new { success = status, message = "Lesson marked as complete" });
            }
            else
            {
                return Json(new { success = status, message = "Lesson not marked as complete" });
            }
        }

        public async Task<IActionResult> MyLearningOffCanvas()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LessonRequestAccess(string q)
        {
            long batchLessonAccessId = Convert.ToInt64(_protector.Unprotect(q));
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));

            // We can pass the encrypted key to the view to use in the form
            ViewBag.EncryptedKey = q;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LessonRequestAccess(string q, string duration = "1d")
        {
            long batchLessonAccessId = Convert.ToInt64(_protector.Unprotect(q));
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));

            DateTime requestEndDate = DateTime.Now;
            if (duration == "3d") requestEndDate = requestEndDate.AddDays(3);
            else if (duration == "7d") requestEndDate = requestEndDate.AddDays(7);
            else requestEndDate = requestEndDate.AddDays(1);

            var status = await _studentService.UpdateBatchLessonAccess(batchLessonAccessId, requestEndDate);

            var userNotification = new UserNotification
            {
                UserId = null,
                UserTypeEnum = UserTypeEnum.Admin,
                NotificationType = "Lesson Requested",
                Message = $"New Lesson Request ({duration}) was initiated by a student",
                Icon = GlobalHelpers.GetEnumDisplayName(FeatherIconEnum.AlertCircle),
                BatchId = null,
                CourseId = null,
            };

            await _userNotificationService.UpdateUserNotification(userNotification);

            if (status)
            {
                return Json(new { success = status, message = "Lesson Access Requested Successfully" });
            }
            else
            {
                return Json(new { success = status, message = "Lesson Access Request Failed" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetPaymentStatus()
        {
            var studentIdStr = HttpContext.Session.GetString("StudentId");
            if (string.IsNullOrEmpty(studentIdStr)) return Json(new { hasAlert = false });

            long studentId = Convert.ToInt64(studentIdStr);
            var summary = await _studentService.GetStudentPaymentSummary(studentId);

            // If a student is in a batch but has 0 total approved payments, show an alert
            var pendingBatches = summary.Where(e => e.TotalApproved <= 0).ToList();

            if (pendingBatches.Any())
            {
                return Json(new { 
                    hasAlert = true, 
                    message = "Your payment for the first month is still pending for: " + string.Join(", ", pendingBatches.Select(b => b.BatchName)),
                    type = "payment_pending"
                });
            }

            return Json(new { hasAlert = false });
        }
    }
}
