using Microsoft.AspNetCore.Mvc;

namespace SENSEI.WEB.Areas.StudentPortal.Controllers
{
    [Area("StudentPortal")]
    public class MyLearningController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
