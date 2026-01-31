using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.DOMAIN;

namespace SENSEI.WEB.Areas.AdminPortal.Controllers
{
    [Area("AdminPortal")]
    public class LessonController : Controller
    {
        private readonly ILessonService _lessonService;
        private readonly ICourseService _courseService;
        private readonly IDataProtector _protector;

        public LessonController(ILessonService lessonService, ICourseService courseService, IDataProtectionProvider provider)
        {
            _lessonService = lessonService;
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

            IQueryable<Lesson> lessons = new List<Lesson>().AsQueryable();

            var (lessonsList, count) = await _lessonService.SearchLessons(courseId, start, length, searchValue, sortColumn, sortDirection);
            lessons = lessonsList.AsQueryable();

            lessons.ToList().ForEach(e =>
            {
                e.EncryptedKey = _protector.Protect(e.LessonId.ToString());
            });


            return Json(new { draw, recordsTotal = count, recordsFiltered = count, data = lessons });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Lesson lesson)
        {
            var (status, lessonId) = await _lessonService.UpdateLesson(lesson);

            if (status)
            {
                return Json(new { success = status, message = "Lesson updated successfully" });
            }
            else
            {
                return Json(new { success = status, message = "Failed to update lesson" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string q)
        {
            long lessonId = Convert.ToInt64(_protector.Unprotect(q));

            var lesson = await _lessonService.GetLesson(lessonId);
            lesson.EncryptedKey = q;

            return View(lesson);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Lesson lesson)
        {
            var (status, lessonId) = await _lessonService.UpdateLesson(lesson);

            if (status)
            {
                return Json(new { success = status, message = "Lesson updated successfully" });
            }
            else
            {
                return Json(new { success = status, message = "Failed to update lesson" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string q)
        {
            try
            {
                long lessonId = Convert.ToInt64(_protector.Unprotect(q));

                var result = await _lessonService.DeleteLesson(lessonId);

                if (result)
                {
                    return Json(new { success = true, message = "Lesson deleted successfully" });
                }

                return Json(new { success = false, message = "Unable to delete lesson" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Invalid request" });
            }
        }


        public async Task<IActionResult> LessonOffCanvas()
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
