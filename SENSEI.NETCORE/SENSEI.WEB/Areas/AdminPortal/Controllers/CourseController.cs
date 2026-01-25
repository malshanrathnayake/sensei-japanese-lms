using Microsoft.AspNetCore.Mvc;
using SENSEI.DOMAIN;

namespace SENSEI.WEB.Areas.AdminPortal.Controllers
{
    [Area("AdminPortal")]
    public class CourseController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> ListOfCourses()
        {
            IEnumerable<Course> courses = new List<Course>();
            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            return Json(new { success = true, message = "Course created successfully" });
        }

        public async Task<IActionResult> CourseOffCanvas()
        {
            return View();
        }
    }
}
