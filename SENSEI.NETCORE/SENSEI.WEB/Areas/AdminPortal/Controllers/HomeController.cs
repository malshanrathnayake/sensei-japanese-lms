using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.WEB.Models;
using System.Threading.Tasks;

namespace SENSEI.WEB.Areas.AdminPortal.Controllers
{
    [Area("AdminPortal")]
    [Authorize(Roles = "Admin,Manager")]
    public class HomeController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IBatchService _batchService;
        private readonly ICourseService _courseService;
        private readonly IStudentRegistrationService _registrationService;

        public HomeController(
            IStudentService studentService,
            IBatchService batchService,
            ICourseService courseService,
            IStudentRegistrationService registrationService)
        {
            _studentService = studentService;
            _batchService = batchService;
            _courseService = courseService;
            _registrationService = registrationService;
        }

        public async Task<IActionResult> Index()
        {
            var (_, totalStudents) = await _studentService.SearchStudent(length: 1);
            var courses = await _courseService.GetCourses();
            var batches = await _batchService.GetBatches();
            
            // Explicitly sort by CreatedDateTime DESC to get the actual "Recent" registrations
            var (recentRegs, pendingRegs) = await _registrationService.SearchStudentRegistraion(
                length: 5, 
                sortColumn: "createdDateTime", 
                sortDirection: "DESC"
            );

            var model = new AdminDashboardViewModel
            {
                TotalStudents = totalStudents,
                TotalCourses = courses.Count(),
                TotalBatches = batches.Count(),
                PendingRegistrations = pendingRegs,
                RecentRegistrations = recentRegs,
                CourseSummary = courses.Take(5)
            };

            return View(model);
        }

        public async Task<IActionResult> Sample()
        {
            return View();
        }
    }
}
