using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.BLL.SystemService;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;
using SENSEI.WEB.Helpers;

namespace SENSEI.WEB.Areas.AdminPortal.Controllers
{
    [Area("AdminPortal")]
    [Authorize(Roles = "Admin,Manager")]
    public class BatchLessonController : Controller
    {
        private readonly IBatchLessonService _batchLessonService;
        private readonly IBatchService _batchService;
        private readonly ILessonService _lessonService;
        private readonly IDataProtector _protector;
        private readonly IViewRenderService _viewRenderService;
        private readonly IMailService _mailService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly ICourseService _courseService;

        public BatchLessonController(
            IBatchLessonService batchLessonService,
            IBatchService batchService,
            ILessonService lessonService,
            IDataProtectionProvider provider,
            IViewRenderService viewRenderService,
            IMailService mailService,
            IUserNotificationService userNotificationService,
            ICourseService courseService
        )
        {
            _batchLessonService = batchLessonService;
            _batchService = batchService;
            _lessonService = lessonService;
            _protector = provider.CreateProtector("BatchLessonProtector");
            _viewRenderService = viewRenderService;
            _mailService = mailService;
            _userNotificationService = userNotificationService;
            _courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> ListOfBatchLessons(long courseId = 0, long batchId = 0)
        {
            int draw = int.Parse(Request.Form["draw"]);
            int start = int.Parse(Request.Form["start"]);
            int length = int.Parse(Request.Form["length"]);

            string searchValue = Request.Form["search[value]"];
            int sortColumnIndex = int.Parse(Request.Form["order[0][column]"]);
            string sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
            string sortDirection = Request.Form["order[0][dir]"]; // asc | desc

            IQueryable<BatchLesson> batchLessons = new List<BatchLesson>().AsQueryable();

            var (batchLessonsList, count) = await _batchLessonService.SearchBatchLessons(courseId, batchId, start, length, searchValue, sortColumn, sortDirection);
            batchLessons = batchLessonsList.AsQueryable();

            batchLessons.ToList().ForEach(e =>
            {
                e.EncryptedKey = _protector.Protect(e.BatchLessonId.ToString());
            });

            return Json(new { draw, recordsTotal = count, recordsFiltered = count, data = batchLessons });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BatchLesson batchLesson)
        {
            var (status, batchLessonId) = await _batchLessonService.UpdateBatchLesson(batchLesson);

            if (status)
            {
                var batch = await _batchService.GetBatch(batchLesson.BatchId);

                var userNotification = new UserNotification
                {
                    UserId = null,
                    UserTypeEnum = UserTypeEnum.Student,
                    NotificationType = "New Recording",
                    Message = "New Recording for " + batch.Course.CourseName.ToString() + " Course on " + batchLesson.LessonDateTime.ToString("yyyy-MM-dd") + " has been uploaded.",
                    Icon = GlobalHelpers.GetEnumDisplayName(FeatherIconEnum.MessageCircle),
                    BatchId = batchLesson.BatchId,
                    CourseId = batch.CourseId,
                };

                await _userNotificationService.UpdateUserNotification(userNotification);

                return Json(new { success = status, message = "Batch lesson saved successfully" });
            }
            else
            {
                return Json(new { success = status, message = "Failed to save batch lesson" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string q)
        {
            long batchLessonId = Convert.ToInt64(_protector.Unprotect(q));

            var batchLesson = await _batchLessonService.GetBatchLesson(batchLessonId);
            batchLesson.EncryptedKey = q;

            return View(batchLesson);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BatchLesson batchLesson)
        {
            var batchLesson2 = await _batchLessonService.GetBatchLesson(batchLesson.BatchLessonId);

            var (status, batchLessonId) = await _batchLessonService.UpdateBatchLesson(batchLesson);

            if (status)
            {
                var batch = await _batchService.GetBatch(batchLesson.BatchId);

                var userNotification = new UserNotification
                {
                    UserId = null,
                    UserTypeEnum = UserTypeEnum.Student,
                    NotificationType = "Recording Updated",
                    Message = "The Recording for " + batch.Course.CourseName.ToString() + " Course on " + batchLesson2.LessonDateTime.ToString("yyyy-MM-dd") + " has been updated.",
                    Icon = GlobalHelpers.GetEnumDisplayName(FeatherIconEnum.MessageCircle),
                    BatchId = batchLesson.BatchId,
                    CourseId = batch.CourseId,
                };

                await _userNotificationService.UpdateUserNotification(userNotification);

                return Json(new { success = status, message = "Batch lesson updated successfully" });
            }
            else
            {
                return Json(new { success = status, message = "Failed to update batch lesson" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string q)
        {
            try
            {
                long batchLessonId = Convert.ToInt64(_protector.Unprotect(q));

                var result = await _batchLessonService.DeleteBatchLesson(batchLessonId);

                if (result)
                {
                    return Json(new { success = true, message = "Batch lesson deleted successfully" });
                }

                return Json(new { success = false, message = "Unable to delete batch lesson" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Invalid request" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddReference(string q)
        {
            long batchLessonId = Convert.ToInt64(_protector.Unprotect(q));

            var batchLesson = await _batchLessonService.GetBatchLesson(batchLessonId);
            batchLesson.EncryptedKey = q;

            var newBatchLessonReference = new BatchLessonReference
            {
                BatchLessonId = batchLesson.BatchLessonId,
                BatchLesson = batchLesson
            };

            return View(newBatchLessonReference);
        }

        [HttpPost]
        public async Task<IActionResult> AddReference(BatchLessonReference batchLessonReference)
        {
            var (status, batchLessonId) = await _batchLessonService.UpdateBatchLessonReference(batchLessonReference);

            if (status)
            {

                var batchLesson = await _batchLessonService.GetBatchLesson(batchLessonReference.BatchLessonId);

                var userNotification = new UserNotification
                {
                    UserId = null,
                    UserTypeEnum = UserTypeEnum.Student,
                    NotificationType = "New Reference Added",
                    Message = "A new reference has been added for " + batchLesson.Batch.Course.CourseName.ToString() + " Course on " + batchLessonReference.BatchLesson.LessonDateTime.ToString("yyyy-MM-dd") + ".",
                    Icon = GlobalHelpers.GetEnumDisplayName(FeatherIconEnum.MessageCircle),
                    BatchId = batchLesson.BatchId,
                    CourseId = batchLesson.Batch.CourseId,
                };

                await _userNotificationService.UpdateUserNotification(userNotification);

                return Json(new { success = status, message = "Batch lesson reference updated successfully" });
            }
            else
            {
                return Json(new { success = status, message = "Failed to update batch lesson reference" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> View(string q)
        {
            long batchLessonId = Convert.ToInt64(_protector.Unprotect(q));

            var batchLesson = await _batchLessonService.GetBatchLesson(batchLessonId);
            batchLesson.EncryptedKey = q;

            batchLesson.BatchLessonReferences.ToList().ForEach(e => e.EncryptedKey = _protector.Protect(e.BatchLessonReferenceId.ToString()));

            return View(batchLesson);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBatchLessonReference(string q)
        {
            long batchLessonReferenceId = Convert.ToInt64(_protector.Unprotect(q));

            var status = await _batchLessonService.DeleteBatchLessonReference(batchLessonReferenceId);

            if (status)
            {
                return Json(new { success = status, message = "Batch lesson reference deleted successfully" });
            }
            else
            {
                return Json(new { success = status, message = "Failed to delete batch reference" });
            }
        }

        public async Task<IActionResult> BatchLessonOffCanvas()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetBatchListJsonResult()
        {
            var batches = await _batchService.GetBatches();

            var result = batches
                .OrderBy(e => e.BatchName)
                .Select(e => new { id = e.BatchId, text = e.BatchName })
                .ToList();

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetLessonListJsonResult(long batchId = 0)
        {
            var lessons = await _lessonService.GetLessons();

            var batch = batchId != 0
                ? await _batchService.GetBatch(batchId)
                : null;

            var result = lessons
                .Where(e => batchId == 0 || e.CourseId == batch.CourseId)
                .OrderBy(e => e.LessonName)
                .Select(e => new { id = e.LessonId, text = e.LessonName })
                .ToList();

            if(batchId == 0)
            {
                result.Clear();
            }

            return Json(result);
        }
    }
}
