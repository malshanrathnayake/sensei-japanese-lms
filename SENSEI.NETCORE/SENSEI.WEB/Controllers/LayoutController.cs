using Microsoft.AspNetCore.DataProtection;
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
        private readonly IDataProtector _protector;

        public LayoutController(IUserNotificationService userNotificationService, IDataProtectionProvider provider)
        {
            _userNotificationService = userNotificationService;
            _protector = provider.CreateProtector("CourseProtector");
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
            var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            var notifications = await _userNotificationService.GetUserNotificationForUser(userId);

            //var notifications = new List<UserNotification>
            //{
            //    new UserNotification
            //    {
            //        UserNotificationId = 1,
            //        NotificationType = "System",
            //        Message = "Your profile was updated successfully.",
            //        IsRead = false,
            //        CreatedAt = DateTime.Now.AddMinutes(-5),
            //        Icon = "user",
            //        IsDeleted = false
            //    },
            //    new UserNotification
            //    {
            //        UserNotificationId = 2,
            //        NotificationType = "Course",
            //        Message = "New lesson has been added to your course.",
            //        IsRead = false,
            //        CreatedAt = DateTime.Now.AddHours(-1),
            //        Icon = "book-open",
            //        IsDeleted = false
            //    },
            //    new UserNotification
            //    {
            //        UserNotificationId = 3,
            //        NotificationType = "Payment",
            //        Message = "Your payment was received successfully.",
            //        IsRead = true,
            //        CreatedAt = DateTime.Now.AddDays(-1),
            //        Icon = "credit-card",
            //        IsDeleted = false
            //    }
            //};

            notifications.ToList().ForEach(e =>
            {
                e.EncryptedKey = _protector.Protect(e.UserNotificationId.ToString());
            });

            return View(notifications);
        }

        public async Task<IActionResult> StudentPortalNotification()
        {
            var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            var notifications = await _userNotificationService.GetUserNotificationForUser(userId);

            notifications.ToList().ForEach(e =>
            {
                e.EncryptedKey = _protector.Protect(e.UserNotificationId.ToString());
            });

            return View(notifications);
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

        [HttpPost]
        public async Task<JsonResult> UpdateUserNotificationReadability(string q)
        {
            if(q is null)
            {
                return Json(new { success = false, message = "Not Updated" });
            }

            long userNotificationId = Convert.ToInt64(_protector.Unprotect(q));
            var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));

            var notifications = await _userNotificationService.UpdateReadability(userNotificationId, userId);

            return Json(new { success = notifications, message = "Updated as read" });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateUserNotificationReadabilityMultiple(List<string> q)
        {
            if (q is null)
            {
                return Json(new { success = false, message = "Not Updated" });
            }

            var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));

            foreach (var query in q)
            {
                try
                {
                    long userNotificationId = Convert.ToInt64(_protector.Unprotect(query));
                    var notifications = await _userNotificationService.UpdateReadability(userNotificationId, userId);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex .Message.ToString()});
                }
                
            }
            

            return Json(new { success = true, message = "Updated all as read" });
        }
        public async Task<JsonResult> GetAdminActionCounts()
        {
            var studentRegistrationService = HttpContext.RequestServices.GetService<IStudentRegistrationService>();
            var studentService = HttpContext.RequestServices.GetService<SENSEI.BLL.StudentPortalService.Interfaces.IStudentService>();
            var batchStudentLessonService = HttpContext.RequestServices.GetService<IBatchStudentLessonService>();

            var (registrations, _) = await studentRegistrationService.SearchStudentRegistraion(0, 0, 1000);
            var pendingRegistrations = registrations.Count(x => !x.IsApproved && !x.IsRejected);

            var (payments, _) = await studentService.SearchStudentBatchPayments(0, 0, 0, 1000); 
            var pendingPayments = payments.Count(x => !x.IsApproved);

            var (pendingRequests, _, _) = await batchStudentLessonService.GetLessonRequestStats();

            return Json(new { 
                pendingRegistrations = pendingRegistrations,
                pendingPayments = pendingPayments,
                pendingLessonRequests = pendingRequests
            });
        }
    }
}
