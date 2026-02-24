using Microsoft.AspNetCore.Mvc;

namespace SENSEI.WEB.Areas.StudentPortal.Controllers
{
    [Area("StudentPortal")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));

            return View();
        }
    }
}
