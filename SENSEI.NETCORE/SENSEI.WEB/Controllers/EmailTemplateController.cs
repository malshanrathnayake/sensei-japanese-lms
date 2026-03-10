using Microsoft.AspNetCore.Mvc;

namespace SENSEI.WEB.Controllers
{
    public class EmailTemplateController : Controller
    {
        public async Task<IActionResult> CommonTemplate()
        {
            return View();
        }

        public async Task<IActionResult> StudentRegistrationTemplate()
        {
            return View();
        }
    }
}
