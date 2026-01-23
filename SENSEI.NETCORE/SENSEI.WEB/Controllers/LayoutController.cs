using Microsoft.AspNetCore.Mvc;
using SENSEI.WEB.Models;
using System.Text.Json;

namespace SENSEI.WEB.Controllers
{
    public class LayoutController : Controller
    {
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
            return View();
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
