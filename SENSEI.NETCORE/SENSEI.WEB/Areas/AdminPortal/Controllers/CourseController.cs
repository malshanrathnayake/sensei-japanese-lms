using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.DOMAIN;
using System.Linq;

namespace SENSEI.WEB.Areas.AdminPortal.Controllers
{
    [Area("AdminPortal")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IDataProtector _protector;

        public CourseController(ICourseService courseService, IDataProtectionProvider provider)
        {
            _courseService = courseService;
            _protector = provider.CreateProtector("CourseProtector");
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> ListOfCourses()
        {
            int draw = int.Parse(Request.Form["draw"]);
            int start = int.Parse(Request.Form["start"]);
            int length = int.Parse(Request.Form["length"]);

            string searchValue = Request.Form["search[value]"];
            int sortColumnIndex = int.Parse(Request.Form["order[0][column]"]);
            string sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
            string sortDirection = Request.Form["order[0][dir]"]; // asc | desc

            IQueryable<Course> courses = new List<Course>().AsQueryable();

            var (courseslist, count) = await _courseService.SearchCourses(start, length, searchValue, sortColumn, sortDirection);
            courses = courseslist.AsQueryable();

            courses.ToList().ForEach(e =>
            {
                e.EncryptedKey = _protector.Protect(e.CourseId.ToString());
            });


            return Json(new { draw, recordsTotal = count, recordsFiltered = count, data = courses });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            var (status, courseId) = await _courseService.UpdateCourse(course);

            if (status)
            {
                return Json(new { success = status, message = "Course updated successfully" });
            }
            else
            {
                return Json(new { success = status, message = "Failed to update course" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string q)
        {
            long courseId = Convert.ToInt64(_protector.Unprotect(q));

            var course = await _courseService.GetCourse(courseId);
            course.EncryptedKey = q;

            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Course course)
        {

            var (status, courseId) = await _courseService.UpdateCourse(course);

            if (status)
            {
                return Json(new { success = status, message = "Course updated successfully" });
            }
            else
            {
                return Json(new { success = status, message = "Failed to update course" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string q)
        {
            try
            {
                long courseId = Convert.ToInt64(_protector.Unprotect(q));

                var result = await _courseService.DeleteCourse(courseId);

                if (result)
                {
                    return Json(new { success = true, message = "Course deleted successfully"} );
                }

                return Json(new { success = false, message = "Unable to delete course" });
            }
            catch (Exception)
            {
                return Json(new{ success = false, message = "Invalid request" });
            }
        }


        public async Task<IActionResult> CourseOffCanvas()
        {
            return View();
        }
    }
}
