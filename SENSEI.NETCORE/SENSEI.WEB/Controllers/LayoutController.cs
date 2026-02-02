using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.DOMAIN;
using SENSEI.WEB.Models;
using System.Text.Json;

namespace SENSEI.WEB.Controllers
{
    public class LayoutController : Controller
    {
        private IUserNotificationService _userNotificationService;

        public LayoutController(IUserNotificationService userNotificationService)
        {
            _userNotificationService = userNotificationService;
        }
        public async Task<IActionResult> AdminSidebar()
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "AdminNavigationidebar.json");
            var json = await System.IO.File.ReadAllTextAsync(jsonPath);

            var sidebarItems = JsonSerializer.Deserialize<List<SidebarItem>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(sidebarItems);
        }

        public async Task<IActionResult> StudentSidebar()
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "StudentNavigationidebar.json");
            var json = await System.IO.File.ReadAllTextAsync(jsonPath);

            var sidebarItems = JsonSerializer.Deserialize<List<SidebarItem>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(sidebarItems);
        }

        public async Task<IActionResult> AdminPortalNotification()
        {
            //var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            //var notifications = await _userNotificationService.GetUserNotificationForUser(userId);

            var notifications = new List<UserNotification>
{
                new UserNotification
                {
                    UserNotificationId = 1,
                    NotificationType = "System",
                    Message = "Your profile was updated successfully.",
                    IsRead = false,
                    CreatedAt = DateTime.Now.AddMinutes(-5),
                    Icon = "user",
                    IsDeleted = false
                },
                new UserNotification
                {
                    UserNotificationId = 2,
                    NotificationType = "Course",
                    Message = "New lesson has been added to your course.",
                    IsRead = false,
                    CreatedAt = DateTime.Now.AddHours(-1),
                    Icon = "book-open",
                    IsDeleted = false
                },
                new UserNotification
                {
                    UserNotificationId = 3,
                    NotificationType = "Payment",
                    Message = "Your payment was received successfully.",
                    IsRead = true,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    Icon = "credit-card",
                    IsDeleted = false
                }
            };


            return View(notifications);
        }

        public async Task<IActionResult> StudentPortalNotification()
        {
            return View();
        }

        public async Task<IActionResult> AdminPortalMessages()
        {
            return View();
        }

        public async Task<IActionResult> StudentPortalMessages()
        {
            return View();
        }

        public async Task<IActionResult> AdminPortalProfile()
        {
            return View();
        }

        public async Task<IActionResult> StudentPortalProfile()
        {
            return View();
        }

        public async Task<IActionResult> AdminPortalHeader()
        {
            return View();
        }

        public async Task<IActionResult> AdminPortalFooter()
        {
            return View();
        }

        public async Task<IActionResult> StudentPortalHeader()
        {
            return View();
        }

        public async Task<IActionResult> StudentPortalFooter()
        {
            return View();
        }
    }
}
