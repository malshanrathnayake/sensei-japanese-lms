using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SENSEI.BLL.AdminPortalService.Interface;
using SENSEI.BLL.SystemService.Interfaces;
using SENSEI.DOMAIN;
using SENSEI.WEB.Helpers;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Text;

namespace SENSEI.WEB.Controllers
{
    public class SenseiJapaneseSchoolController : Controller
    {
        private readonly ISmsService _smsService;
        private readonly ICourseService _courseService;
        private readonly ILocationService _locationService;
        private readonly IStudentRegistrationService _studentRegistrationService;

        public SenseiJapaneseSchoolController(ISmsService smsService, ICourseService courseService, ILocationService locationService, IStudentRegistrationService studentRegistrationService)
        {
            _smsService = smsService;
            _courseService = courseService;
            _locationService = locationService;
            _studentRegistrationService = studentRegistrationService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Gallery()
        {
            return View();
        }

        #region Login

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> OTPLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OTPLogin(string phone)
        {
            //94711408116 sample number
            var otpCode = GlobalHelpers.GenerateOtp();

            phone = "94" + phone;

            var message = $"Your OTP code is {otpCode}. This code is valid for 5 minutes.";

            //var status = await _smsService.SendSingleAsync(phone, message);
            var status = true;

            if (!status)
            {
                return RedirectToAction("Login");
            }

            return RedirectToAction("OtpConfirm");
        }

        #endregion

        #region Registration

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(StudentRegistration studentRegistration)
        {
            if (!ModelState.IsValid)
            {
                return View(studentRegistration);
            }

            // Backend unique email validation (basic check for demonstration)
            // In a real app, this would call a service to check the DB.
            var isEmailUnique = true; // Replace with: await _studentRegistrationService.IsEmailUnique(studentRegistration.Email);
            
            if (!isEmailUnique)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(studentRegistration);
            }

            var (status, primaryKey) = await _studentRegistrationService.UpdateStudentRegistraion(studentRegistration);

            if (status)
            {
                TempData.AddNotification(new NotificationMessage
                {
                    Type = "success",
                    Message = "Registration submitted successfully!"
                });
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "An error occurred during registration. Please try again.");
                return View(studentRegistration);
            }
        }

        [HttpGet]
        public async Task<JsonResult> CheckEmailUnique(string email)
        {
            // Placeholder for remote validation if needed
            // var isUnique = await _studentRegistrationService.IsEmailUnique(email);
            return Json(true);
        }
        #endregion

        #region Google Authentication

        public async Task<IActionResult> SigninGoogle()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return RedirectToAction("Login");

            var claims = result.Principal.Identities
                .FirstOrDefault()?.Claims;

            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            Random rnd = new Random();

            var randuserId = rnd.Next(1, 1000);

            HttpContext.Session.SetString("UserId", randuserId.ToString());

            var appClaims = new List<Claim>
            {
                new Claim("UserId", randuserId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, name)
            };

            var identity = new ClaimsIdentity(appClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            TempData.AddNotification(new NotificationMessage
            {
                Type = "success",
                Message = "Signed in successfully!"
            });

            TempData.AddNotification(new NotificationMessage
            {
                Type = "info",
                Message = $"Welcome {name}"
            });


            return RedirectToAction("Index", "Home", new { Area = "Adminportal" });
        }

        public IActionResult LogoutGoogle()
        {
            return SignOut(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        #endregion

        #region Dropdown

        [HttpGet]
        public async Task<JsonResult> GetCourseListJsonResult()
        {
            var courses = await _courseService.GetCourses();

            var result = courses.Where(e => !e.IsDeleted).OrderBy(e => e.CourseName).Select(e => new { id = e.CourseId, text = e.CourseName }).ToList();

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetCountryListJsonResult()
        {
            var countries = await _locationService.GetContries();

            var result = countries.OrderBy(e => e.CountryName).Select(e => new { id = e.CountryCode, text = e.CountryName }).ToList();

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetStateListJsonResult(int countryId)
        {
            var states = await _locationService.GetStates(countryId);

            var result = states.OrderBy(e => e.StateName).Select(e => new { id = e.StateId, text = e.StateName }).ToList();

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetCityListJsonResult(int stateId)
        {
            var cities = await _locationService.GetCities(stateId);

            var result = cities.OrderBy(e => e.CityName).Select(e => new { id = e.CityId, text = e.CityName }).ToList();

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetBranchListJsonResult()
        {
            var branches = await _locationService.GetBranches();

            var result = branches.OrderBy(e => e.BranchName).Select(e => new { id = e.BranchId, text = e.BranchName }).ToList();

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetLearningModeListJsonResult()
        {
            var modes = await _locationService.GetStudentLearningModes();

            var result = modes.OrderBy(e => e.LearningModeName).Select(e => new { id = e.StudentLearningModeId, text = e.LearningModeName }).ToList();

            return Json(result);
        }

        #endregion
    }
}
