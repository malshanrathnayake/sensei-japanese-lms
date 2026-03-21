using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;
using SENSEI.WEB.Helpers;

namespace SENSEI.WEB.Areas.AdminPortal.Controllers
{
    [Area("AdminPortal")]
    [Authorize(Roles = "Admin,Manager")]
    public class BatchStudentLessonRequestController : Controller
    {
        private readonly IBatchStudentLessonService _batchStudentLessonService;
        private IBatchService _batchService;
        private readonly ICourseService _courseService;
        private readonly IDataProtector _protector;
        private readonly ISmsService _smsService;
        private readonly IUserNotificationService _userNotificationService;

        public BatchStudentLessonRequestController
        (
            IBatchStudentLessonService batchStudentLessonService,
            IBatchService batchService,
            ICourseService courseService,
            IDataProtectionProvider provider,
            ISmsService smsService,
            IUserNotificationService userNotificationService
        )
        {
            _batchStudentLessonService = batchStudentLessonService;
            _batchService = batchService;
            _courseService = courseService;
            _protector = provider.CreateProtector("CourseProtector");
            _smsService = smsService;
            _userNotificationService = userNotificationService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ListOfBatchStudentLessonRequests(long courseId = 0, long batchId = 0, string indexNumber = "")
        {
            int draw = int.Parse(Request.Form["draw"]);
            int start = int.Parse(Request.Form["start"]);
            int length = int.Parse(Request.Form["length"]);

            string searchValue = Request.Form["search[value]"];
            int sortColumnIndex = int.Parse(Request.Form["order[0][column]"]);
            string sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
            string sortDirection = Request.Form["order[0][dir]"]; // asc | desc

            IQueryable<BatchStudentLessonAccessRequest> batchStudentLessonAccessRequests = new List<BatchStudentLessonAccessRequest>().AsQueryable();

            var (batchStudentLessonAccessRequestList, count) = await _batchStudentLessonService.SearchBatchStudentLesson(courseId, batchId, indexNumber, start, length, searchValue, sortColumn, sortDirection);
            batchStudentLessonAccessRequests = batchStudentLessonAccessRequestList.AsQueryable();

            batchStudentLessonAccessRequests.ToList().ForEach(e =>
            {
                e.EncryptedKey = _protector.Protect(e.BatchStudentLessonAccessRequestId.ToString());
            });


            return Json(new { draw, recordsTotal = count, recordsFiltered = count, data = batchStudentLessonAccessRequests });
        }

        [HttpGet]
        public async Task<IActionResult> Approve(string q)
        {
            long batchStudentLessonRequestId = Convert.ToInt64(_protector.Unprotect(q));

            var batchStudentLessonRequest = await _batchStudentLessonService.GetBatchStudentLessonRequest(batchStudentLessonRequestId);

            return View(batchStudentLessonRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(BatchStudentLessonAccessRequest batchStudentLessonAccessRequest)
        {
            var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));

            var result = await _batchStudentLessonService.ApproveBatchStudentLessonRequest(batchStudentLessonAccessRequest.BatchStudentLessonAccessRequestId, userId);

            if (result)
            {
                var batchStudentLessonRequest = await _batchStudentLessonService.GetBatchStudentLessonRequest(batchStudentLessonAccessRequest.BatchStudentLessonAccessRequestId);
                var phone = batchStudentLessonRequest.BatchStudentLessonAccess.Student.PhoneNo?.Replace("+", "");

                var message =
                        $"Your request for {batchStudentLessonRequest.BatchStudentLessonAccess.BatchLesson.Lesson.LessonName} - {batchStudentLessonRequest.BatchStudentLessonAccess.BatchLesson.Description} was approved. " +
                        $"approved Date date {batchStudentLessonRequest.ChangedDate?.ToString("yyyy-MM-dd")}. " +
                        $"Login using your Google account ({batchStudentLessonRequest.BatchStudentLessonAccess.Student.Email}) " +
                        $"or mobile number ({batchStudentLessonRequest.BatchStudentLessonAccess.Student.PhoneNo}).";

                var messageStatus = await _smsService.SendSingleAsync(phone, message);

                var userNotification = new UserNotification
                {
                    UserId = batchStudentLessonRequest.BatchStudentLessonAccess.Student.UserId,
                    UserTypeEnum = UserTypeEnum.Student,
                    NotificationType = "Lesson Request Approved",
                    Message = "Your request for" + batchStudentLessonRequest.BatchStudentLessonAccess.BatchLesson.Lesson.LessonName + "-" + batchStudentLessonRequest.BatchStudentLessonAccess.BatchLesson.Description + " was approved. ",
                    Icon = GlobalHelpers.GetEnumDisplayName(FeatherIconEnum.CheckCircle),
                    BatchId = null,
                    CourseId = null,
                };

                await _userNotificationService.UpdateUserNotification(userNotification);

                return Json(new { success = result, message = "Student payment approved successfully" });
            }
            else
            {
                return Json(new { success = result, message = "Failed to approve student payment" });
            }

        }

        public async Task<IActionResult> BatchStudentLessonOffCanvas()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetCourseListJsonResult()
        {
            var courses = await _courseService.GetCourses();

            var result = courses.Where(e => !e.IsDeleted).OrderBy(e => e.CourseName).Select(e => new { id = e.CourseId, text = e.CourseName }).ToList();

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetBatchListJsonResult(long courseId = 0)
        {
            var batches = await _batchService.GetBatches();

            var result = courseId != 0 ? batches.Where(e => e.CourseId == courseId).OrderBy(e => e.BatchName).Select(e => new { id = e.BatchId, text = e.BatchName }).ToList()
                : batches.OrderBy(e => e.BatchName).Select(e => new { id = e.BatchId, text = e.BatchName }).ToList();

            return Json(result);
        }
    }
}
