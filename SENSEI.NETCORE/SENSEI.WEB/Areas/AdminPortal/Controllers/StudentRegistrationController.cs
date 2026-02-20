using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.DOMAIN;

namespace SENSEI.WEB.Areas.AdminPortal.Controllers
{
    [Area("AdminPortal")]
    public class StudentRegistrationController : Controller
    {
        private readonly IStudentRegistrationService _studentRegistrationService;
        private IBatchService _batchService;
        private readonly ICourseService _courseService;
        private readonly IDataProtector _protector;

        public StudentRegistrationController
        (
            IStudentRegistrationService studentRegistrationService,
            IBatchService batchService,
            ICourseService courseService,
            IDataProtectionProvider provider
        )
        {
            _studentRegistrationService = studentRegistrationService;
            _batchService = batchService;
            _courseService = courseService;
            _protector = provider.CreateProtector("CourseProtector");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> ListOfStudentRegistrations(long courseId = 0)
        {
            int draw = int.Parse(Request.Form["draw"]);
            int start = int.Parse(Request.Form["start"]);
            int length = int.Parse(Request.Form["length"]);

            string searchValue = Request.Form["search[value]"];
            int sortColumnIndex = int.Parse(Request.Form["order[0][column]"]);
            string sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
            string sortDirection = Request.Form["order[0][dir]"]; // asc | desc

            IQueryable<StudentRegistration> studentRegistrations = new List<StudentRegistration>().AsQueryable();

            var (studentRegistrationList, count) = await _studentRegistrationService.SearchStudentRegistraion(courseId, start, length, searchValue, sortColumn, sortDirection);
            studentRegistrations = studentRegistrationList.AsQueryable();

            studentRegistrations.ToList().ForEach(e =>
            {
                e.EncryptedKey = _protector.Protect(e.StudentRegistrationId.ToString());
            });


            return Json(new { draw, recordsTotal = count, recordsFiltered = count, data = studentRegistrations });
        }

        [HttpGet]
        public async Task<IActionResult> Approve(string q)
        {
            long studentRegistrationId = Convert.ToInt64(_protector.Unprotect(q));

            var studentRegistration = await _studentRegistrationService.GetStudentRegistraion(studentRegistrationId);

            return View(studentRegistration);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(long StudentRegistrationId, long batchId, string indexNumber)
        {
            var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));

            var result = await _studentRegistrationService.ApproveStudentRegistraion(StudentRegistrationId, indexNumber, batchId, userId);

            if (result)
            {
                return Json(new { success = result, message = "Student registration approved successfully" });
            }
            else
            {
                return Json(new { success = result, message = "Failed to approve student registration" });
            }

        }

        public async Task<IActionResult> StudentRegistrationOffCanvas()
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
        public async Task<JsonResult> GetBatchListJsonResult(int courseId)
        {
            var batches = await _batchService.GetBatches(courseId);

            var result = batches.Where(e => !e.IsDeleted).OrderBy(e => e.BatchName).Select(e => new { id = e.CourseId, text = e.BatchName }).ToList();

            return Json(result);
        }
    }
}
