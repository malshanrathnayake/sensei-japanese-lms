using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SENSEI.WEB.Areas.AdminPortal.Controllers
{
    [Area("AdminPortal")]
    [Authorize(Roles = "Admin,Manager")]
    public class AdminProfileController : Controller
    {
        private readonly IUserService _userService;

        public AdminProfileController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            long userId = Convert.ToInt64(User.FindFirst("UserId")?.Value ?? "0");
            var user = await _userService.GetUserByUserId(userId);
            
            // If it's a staff member, we'll want to show staff details too.
            // The User domain model usually contains a Staff property if it's an admin/manager.
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(User userModel)
        {
            // Implementation for updating profile can be added here
            // For now, let's keep it simple as the user primarily asked to "show" the profile
            TempData["Success"] = "Profile functionality is under development.";
            return RedirectToAction("Index");
        }
    }
}
