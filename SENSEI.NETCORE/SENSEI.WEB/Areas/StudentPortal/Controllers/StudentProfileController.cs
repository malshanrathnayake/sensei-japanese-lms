using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.StudentPortalService.Interfaces;
using SENSEI.DOMAIN;
using System;
using System.Threading.Tasks;

namespace SENSEI.WEB.Areas.StudentPortal.Controllers
{
    [Area("StudentPortal")]
    public class StudentProfileController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentProfileController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        {
            long userId = Convert.ToInt64(User.FindFirst("UserId")?.Value ?? "0");
            var student = await _studentService.GetStudentProfile(userId);
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(Student student)
        {
            // Update logic can be implemented here in the future
            return View("Index", student);
        }
    }
}
