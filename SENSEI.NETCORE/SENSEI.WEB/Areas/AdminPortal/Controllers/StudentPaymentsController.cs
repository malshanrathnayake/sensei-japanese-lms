using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;
using static System.Net.Mime.MediaTypeNames;

namespace SENSEI.WEB.Areas.AdminPortal.Controllers
{
    [Area("AdminPortal")]
    [Authorize(Roles = "Admin,Manager")]
    public class StudentPaymentsController : Controller
    {
        private readonly IStudentPaymentService _studentPaymentService;
        private IBatchService _batchService;
        private readonly ICourseService _courseService;
        private readonly IDataProtector _protector;
        private readonly ISmsService _smsService;

        public StudentPaymentsController
        (
            IStudentPaymentService studentPaymentService,
            IBatchService batchService,
            ICourseService courseService,
            IDataProtectionProvider provider,
            ISmsService smsService
        )
        {
            _studentPaymentService = studentPaymentService;
            _batchService = batchService;
            _courseService = courseService;
            _protector = provider.CreateProtector("CourseProtector");
            _smsService = smsService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Approve(string q)
        {
            long studentBatchPaymentId = Convert.ToInt64(_protector.Unprotect(q));

            var studentBatchPayment = await _studentPaymentService.GetStudentBatchPayment(studentBatchPaymentId);

            return View(studentBatchPayment);
        }

        [HttpPost]
        public async Task<IActionResult> ListOfStudentPayments(long courseId = 0, long batchId = 0, string indexNumber = "")
        {
            int draw = int.Parse(Request.Form["draw"]);
            int start = int.Parse(Request.Form["start"]);
            int length = int.Parse(Request.Form["length"]);

            string searchValue = Request.Form["search[value]"];
            int sortColumnIndex = int.Parse(Request.Form["order[0][column]"]);
            string sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
            string sortDirection = Request.Form["order[0][dir]"]; // asc | desc

            IQueryable<StudentBatchPayment> studentBatchPayments = new List<StudentBatchPayment>().AsQueryable();

            var (studentBatchPaymentList, count) = await _studentPaymentService.SearchStudentBatchPayment(courseId, batchId, indexNumber, start, length, searchValue, sortColumn, sortDirection);
            studentBatchPayments = studentBatchPaymentList.AsQueryable();

            studentBatchPayments.ToList().ForEach(e =>
            {
                e.EncryptedKey = _protector.Protect(e.StudentBatchPaymentId.ToString());
            });


            return Json(new { draw, recordsTotal = count, recordsFiltered = count, data = studentBatchPayments });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(long StudentBatchPaymentId)
        {
            var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));

            var result = await _studentPaymentService.ApproveStudentBatchPayment(StudentBatchPaymentId, userId);

            if (result)
            {
                var studentBatchPayment = await _studentPaymentService.GetStudentBatchPayment(StudentBatchPaymentId);
                var phone = studentBatchPayment.StudentBatch.Student.PhoneNo?.Replace("+", "");

                var message =
                        $"Your payment for {studentBatchPayment.StudentBatch.Batch.Course.CourseName} - {studentBatchPayment.StudentBatch.Batch.BatchName} was approved. " +
                        $"Payment date {studentBatchPayment.PaymentDate?.ToString("yyyy-MM-dd")}." +
                        $"Login using your Google account ({studentBatchPayment.StudentBatch.Student.Email}) " +
                        $"or mobile number ({studentBatchPayment.StudentBatch.Student.PhoneNo}).";

                var messageStatus = await _smsService.SendSingleAsync(phone, message);
                return Json(new { success = result, message = "Student payment approved successfully" });
            }
            else
            {
                return Json(new { success = result, message = "Failed to approve student payment" });
            }

        }

        [HttpGet]
        public async Task<IActionResult> Reject(string q)
        {
            long studentBatchPaymentId = Convert.ToInt64(_protector.Unprotect(q));

            var studentBatchPayment = await _studentPaymentService.GetStudentBatchPayment(studentBatchPaymentId);

            return View(studentBatchPayment);
        }

        [HttpPost]
        public async Task<IActionResult> Reject(long StudentBatchPaymentId)
        {
            var userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));

            var result = await _studentPaymentService.RejectStudentBatchPayment(StudentBatchPaymentId, userId);

            if (result)
            {
                var studentBatchPayment = await _studentPaymentService.GetStudentBatchPayment(StudentBatchPaymentId);
                var phone = studentBatchPayment.StudentBatch.Student.PhoneNo?.Replace("+", "");

                var message =
                        $"Your payment for {studentBatchPayment.StudentBatch.Batch.Course.CourseName} - {studentBatchPayment.StudentBatch.Batch.BatchName} was rejected. " +
                        $"Payment date {studentBatchPayment.PaymentDate?.ToString("yyyy-MM-dd")}." +
                        $"Please do not hesitate to contact us for further assistance.";

                var messageStatus = await _smsService.SendSingleAsync(phone, message);

                return Json(new { success = result, message = "Student payment rejected successfully" });
            }
            else
            {
                return Json(new { success = result, message = "Failed to reject student payment" });
            }

        }

        public async Task<IActionResult> StudentPaymentOffCanvas()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetCourseListJsonResult()
        {
            var courses = await _courseService.GetCourses();

            var result = courses.Where(e => !e.IsDeleted).OrderBy(e => e.CourseName).Select(e => new { id = e.CourseId, text = e.CourseName }).ToList();

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetBatchListJsonResult(long courseId = 0)
        {
            var batches = await _batchService.GetBatches();

            var result = courseId != 0 ? batches.Where(e => e.CourseId == courseId).OrderBy(e => e.BatchName).Select(e => new { id = e.BatchId, text = e.BatchName }).ToList()
                : batches.OrderBy(e => e.BatchName).Select(e => new { id = e.BatchId, text = e.BatchName }).ToList();

            return Json(result);
        }
    }
}
