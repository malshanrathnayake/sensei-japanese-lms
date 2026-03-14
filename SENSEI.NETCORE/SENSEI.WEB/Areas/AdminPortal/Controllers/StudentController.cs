using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;

namespace SENSEI.WEB.Areas.AdminPortal.Controllers
{
    [Area("AdminPortal")]
    [Authorize(Roles = "Admin,Manager")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private IBatchService _batchService;
        private readonly ICourseService _courseService;
        private readonly IDataProtector _protector;
        private readonly ISmsService _smsService;

        public StudentController
        (
            IStudentService studentService,
            IBatchService batchService,
            ICourseService courseService,
            IDataProtectionProvider provider,
            ISmsService smsService
        )
        {
            _studentService = studentService;
            _batchService = batchService;
            _courseService = courseService;
            _protector = provider.CreateProtector("CourseProtector");
            _smsService = smsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ListOfStudents(long courseId = 0, long batchId = 0)
        {
            int draw = int.Parse(Request.Form["draw"]);
            int start = int.Parse(Request.Form["start"]);
            int length = int.Parse(Request.Form["length"]);

            string searchValue = Request.Form["search[value]"];
            int sortColumnIndex = int.Parse(Request.Form["order[0][column]"]);
            string sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
            string sortDirection = Request.Form["order[0][dir]"]; // asc | desc

            IQueryable<Student> students = new List<Student>().AsQueryable();

            var (studentRegistrationList, count) = await _studentService.SearchStudent(courseId, batchId, start, length, searchValue, sortColumn, sortDirection);
            students = studentRegistrationList.AsQueryable();

            students.ToList().ForEach(e =>
            {
                e.EncryptedKey = _protector.Protect(e.StudentId.ToString());
            });


            return Json(new { draw, recordsTotal = count, recordsFiltered = count, data = students });
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

            var result = batches.Where(e => !e.IsDeleted).OrderBy(e => e.BatchName).Select(e => new { id = e.BatchId, text = e.BatchName }).ToList();

            return Json(result);
        }
    }
}
