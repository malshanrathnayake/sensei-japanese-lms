using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.StudentPortalService.Interfaces;
using SENSEI.DOMAIN;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SENSEI.WEB.Areas.StudentPortal.Controllers
{
    [Area("StudentPortal")]
    public class StudentBatchPaymentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentBatchPaymentController(IStudentService studentService)
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
        public async Task<IActionResult> SearchPayments()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));

            int skip = start != null ? Convert.ToInt32(start) : 0;
            int pageSize = length != null ? Convert.ToInt32(length) : 10;

            var (payments, totalRecords) = await _studentService.SearchStudentBatchPayments(studentId, 0, skip, pageSize);

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = payments });
        }

        public async Task<IActionResult> GetBatches()
        {
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));
            var batches = await _studentService.GetStudentBatchesList(studentId);
            return Json(batches);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePayment(StudentBatchPayment payment)
        {
            var (status, primaryKey) = await _studentService.UpdateStudentBatchPayment(payment);
            return Json(new { status = status, id = primaryKey });
        }

        public async Task<IActionResult> GetPayment(long id)
        {
            var payment = await _studentService.GetStudentBatchPayment(id);
            return Json(payment);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePayment(long id)
        {
            var status = await _studentService.DeleteStudentBatchPayment(id);
            return Json(new { success = status });
        }

        public async Task<IActionResult> GetPaymentSummary()
        {
            var studentId = Convert.ToInt64(HttpContext.Session.GetString("StudentId"));
            var summary = await _studentService.GetStudentPaymentSummary(studentId);
            return Json(summary);
        }
    }
}
