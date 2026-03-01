using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.StudentPortalService.Interfaces;
using SENSEI.DOMAIN;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SENSEI.WEB.Areas.StudentPortal.Controllers
{
    [Area("StudentPortal")]
    public class StudentBatchPaymentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentBatchPaymentController(IStudentService studentService, IWebHostEnvironment webHostEnvironment)
        {
            _studentService = studentService;
            _webHostEnvironment = webHostEnvironment;
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

        [HttpGet]
        public async Task<IActionResult> UpdatePayment()
        {
            long userId = Convert.ToInt64(User.FindFirst("UserId")?.Value ?? "0");
            var student = await _studentService.GetStudentProfile(userId);

            StudentBatchPayment studentBatchPayment = new StudentBatchPayment();
            studentBatchPayment.StudentBatchId = student.StudentBatches.FirstOrDefault()?.StudentBatchId ?? 0;

            return View(studentBatchPayment);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePayment(StudentBatchPayment studentBatchPayment)
        {
            long userId = Convert.ToInt64(User.FindFirst("UserId")?.Value ?? "0");

            if (studentBatchPayment == null) return Json(new { status = false });

            if (studentBatchPayment.SlipImage == null || studentBatchPayment.SlipImage.Length == 0)
                return Json(new { success = false, message = "Slip image is required." });

            if (!studentBatchPayment.SlipImage.ContentType.StartsWith("image/"))
                return Json(new { success = false, message = "Invalid file type." });

            if (studentBatchPayment.SlipImage.Length > 5 * 1024 * 1024)
                return Json(new { success = false, message = "File too large (max 5MB)." });

            var studentBatch = await _studentService.GetStudentBatchById(studentBatchPayment.StudentBatchId);
            if (studentBatch.FirstOrDefault().Batch == null)
                return Json(new { success = false, message = "Batch not found." });

            var student = await _studentService.GetStudentProfile(userId);
            if (student == null)
                return Json(new { success = false, message = "Student not found." });

            string batchName = SafePathSegment(studentBatch.FirstOrDefault().Batch.BatchName); 
            string studentIndex = SafePathSegment(student.IndexNumber); 

            var root = _webHostEnvironment.WebRootPath;

            batchName = SafePathSegment(batchName);
            studentIndex = SafePathSegment(studentIndex);

            var batchFolder = Path.Combine(root, "uploads", "payment-slips", batchName);
            if (!Directory.Exists(batchFolder))
                Directory.CreateDirectory(batchFolder);

            var studentFolder = Path.Combine(batchFolder, studentIndex);
            if (!Directory.Exists(studentFolder))
                Directory.CreateDirectory(studentFolder);

            var monthStamp = studentBatchPayment.PaymentMonth.ToString("MMMyyyy", CultureInfo.InvariantCulture).ToUpper(); // FEB2026

            var baseName = monthStamp;
            var savedFileName = $"{monthStamp}.jpg";
            var fullPath = Path.Combine(studentFolder, savedFileName);

            int i = 1;
            while (System.IO.File.Exists(fullPath))
            {
                savedFileName = $"{baseName}_{i}.jpg";
                fullPath = Path.Combine(studentFolder, savedFileName);
                i++;
            }

            using (var fs = new FileStream(fullPath, FileMode.Create))
            {
                await studentBatchPayment.SlipImage.CopyToAsync(fs);
            }

            var slipUrl = $"/uploads/payment-slips/{batchName}/{studentIndex}/{savedFileName}";

            var payment = new StudentBatchPayment
            {
                StudentBatchPaymentId = studentBatchPayment.StudentBatchPaymentId,
                StudentBatchId = studentBatchPayment.StudentBatchId,
                Amount = studentBatchPayment.Amount,
                PaymentMonth = studentBatchPayment.PaymentMonth,
                PaymentDate = DateTime.UtcNow,
                SlipUrl = slipUrl,
                IsApproved = false,
                ApprovedById = null,
                IsDeleted = false
            };

            var (status, primaryKey) = await _studentService.UpdateStudentBatchPayment(payment);

            return Json(new { success = status, message = "Slip Submitted Successfully!" });
        }

        private static string SafePathSegment(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return "unknown";

            value = value.Trim();

            // replace invalid path chars + keep it URL/path friendly
            value = Regex.Replace(value, @"[^\w\- ]+", ""); // remove special chars
            value = Regex.Replace(value, @"\s+", "-");      // spaces -> hyphen
            return value;
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

        [HttpGet]
        public async Task<IActionResult> StudentBatchPaymentOffCanvas()
        {
            return View();
        }
    }
}
