using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.DOMAIN;

namespace SENSEI.WEB.Areas.AdminPortal.Controllers
{
    [Area("AdminPortal")]
    public class BatchLessonController : Controller
    {
        private readonly IBatchLessonService _batchLessonService;
        private readonly IBatchService _batchService;
        private readonly ILessonService _lessonService;
        private readonly IDataProtector _protector;

        public BatchLessonController(
            IBatchLessonService batchLessonService,
            IBatchService batchService,
            ILessonService lessonService,
            IDataProtectionProvider provider
        )
        {
            _batchLessonService = batchLessonService;
            _batchService = batchService;
            _lessonService = lessonService;
            _protector = provider.CreateProtector("BatchLessonProtector");
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
            var (status, batchLessonId) = await _batchLessonService.UpdateBatchLesson(batchLesson);

            if (status)
            {
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
        public async Task<JsonResult> GetLessonListJsonResult()
        {
            var lessons = await _lessonService.GetLessons();

            var result = lessons
                .OrderBy(e => e.LessonName)
                .Select(e => new { id = e.LessonId, text = e.LessonName })
                .ToList();

            return Json(result);
        }
    }
}
