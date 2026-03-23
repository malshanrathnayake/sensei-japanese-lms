using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.StudentPortalService.Interfaces;
using SENSEI.DOMAIN;
using System;
using System.Threading.Tasks;

namespace SENSEI.WEB.Areas.StudentPortal.Controllers
{
    [Area("StudentPortal")]
    [Authorize(Roles = "Student")]
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
            try
            {
                var success = await _studentService.UpdateStudentProfile(student);
                if (success)
                {
                    // Refresh DisplayName in session so the header/sidebar name updates immediately
                    HttpContext.Session.SetString("DisplayName", student.StudentPopulatedName);
                    TempData["Success"] = "Profile updated successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to update profile. Server rejected the change.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
