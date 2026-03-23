using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SENSEI.WEB.Areas.StudentPortal.Controllers
{
    [Area("StudentPortal")]
    [Authorize(Roles = "Student")]
    public class HomeController : Controller
    {
        private readonly SENSEI.BLL.StudentPortalService.Interfaces.IStudentService _studentService;

        public HomeController(SENSEI.BLL.StudentPortalService.Interfaces.IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async System.Threading.Tasks.Task<IActionResult> Index()
        {
            var studentIdStr = HttpContext.Session.GetString("StudentId");
            if (string.IsNullOrEmpty(studentIdStr)) return Redirect("~/auth/login");

            long studentId = Convert.ToInt64(studentIdStr);

            var (courses, totalCourses) = await _studentService.SearchStudentCourses(studentId, 0, 10);
            var (lessons, totalLessons) = await _studentService.SearchStudentBatchLessons(studentId, 0, 0, 5, "", "lessonDateTime", "DESC"); 

            ViewBag.TotalCourses = totalCourses;
            ViewBag.TotalLessons = totalLessons;
            ViewBag.UpcomingLessons = lessons;

            return View();
        }
    }
}
