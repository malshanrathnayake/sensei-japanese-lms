using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.DOMAIN;

namespace SENSEI.WEB.Areas.AdminPortal.Controllers
{
    [Area("AdminPortal")]
    public class BatchController : Controller
    {
        private IBatchService _batchService;
        private readonly ICourseService _courseService;
        private readonly IDataProtector _protector;

        public BatchController
        (
            IBatchService batchService, 
            ICourseService courseService, 
            IDataProtectionProvider provider
        )
        {
            _batchService = batchService;
            _courseService = courseService;
            _protector = provider.CreateProtector("CourseProtector");
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> ListOfLessons(long courseId = 0)
        {
            int draw = int.Parse(Request.Form["draw"]);
            int start = int.Parse(Request.Form["start"]);
            int length = int.Parse(Request.Form["length"]);

            string searchValue = Request.Form["search[value]"];
            int sortColumnIndex = int.Parse(Request.Form["order[0][column]"]);
            string sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
            string sortDirection = Request.Form["order[0][dir]"]; // asc | desc

            IQueryable<Batch> batches = new List<Batch>().AsQueryable();

            var (batchesList, count) = await _batchService.SearchBatches(courseId, start, length, searchValue, sortColumn, sortDirection);
            batches = batchesList.AsQueryable();

            batches.ToList().ForEach(e =>
            {
                e.EncryptedKey = _protector.Protect(e.BatchId.ToString());
            });


            return Json(new { draw, recordsTotal = count, recordsFiltered = count, data = batches });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Batch batch)
        {
            var (status, batchId) = await _batchService.UpdateBatch(batch);

            if (status)
            {
                return Json(new { success = status, message = "Batch updated successfully" });
            }
            else
            {
                return Json(new { success = status, message = "Failed to update batch" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string q)
        {
            long batchId = Convert.ToInt64(_protector.Unprotect(q));

            var batch = await _batchService.GetBatch(batchId);
            batch.EncryptedKey = q;

            return View(batch);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Batch batch)
        {
            var (status, batchId) = await _batchService.UpdateBatch(batch);

            if (status)
            {
                return Json(new { success = status, message = "Batch updated successfully" });
            }
            else
            {
                return Json(new { success = status, message = "Failed to update batch" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string q)
        {
            try
            {
                long batchId = Convert.ToInt64(_protector.Unprotect(q));

                var result = await _batchService.DeleteBatch(batchId);

                if (result)
                {
                    return Json(new { success = true, message = "Batch deleted successfully" });
                }

                return Json(new { success = false, message = "Unable to delete batch" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Invalid request" });
            }
        }

        public async Task<IActionResult> BatchOffCanvas()
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
    }
}
