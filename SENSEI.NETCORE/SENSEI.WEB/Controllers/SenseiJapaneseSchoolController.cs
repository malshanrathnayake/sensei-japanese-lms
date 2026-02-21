using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
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
        private readonly IUserService _userService;

        public SenseiJapaneseSchoolController
        (
            ISmsService smsService, 
            ICourseService courseService, 
            ILocationService locationService, 
            IStudentRegistrationService studentRegistrationService,
            IUserService userService
        )
        {
            _smsService = smsService;
            _courseService = courseService;
            _locationService = locationService;
            _studentRegistrationService = studentRegistrationService;
            _userService = userService;
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
            var otpCode = GlobalHelpers.GenerateOtp();
            var smsStatus = false;
            var phoneNo = "94" + phone.ToString();

            var user = await _userService.GetUserByPhone("+" + phoneNo);

            if (user is null)
            {
                TempData.AddNotification(new NotificationMessage
                {
                    Type = "Error",
                    Message = "Your account is not registered."
                });

                return RedirectToAction("Login");
            }

            var message = $"Your OTP code is {otpCode}. This code is valid for 5 minutes.";

            var userOtp = new User
            {
                UserId = user.UserId,
                LastOtpSequence = Convert.ToInt32(otpCode),
            };

            var optupdated = await _userService.UpdateOtpSequence(userOtp);

            if (optupdated)
            {
                smsStatus = await _smsService.SendSingleAsync(phoneNo, message);
            }

            if (!smsStatus)
            {
                TempData.AddNotification(new NotificationMessage
                {
                    Type = "Error",
                    Message = "OTP sent failed!"
                });

                return RedirectToAction("Login");
            }

            TempData.AddNotification(new NotificationMessage
            {
                Type = "success",
                Message = "OTP sent successfully!"
            });

            return RedirectToAction("OtpConfirm", new { userGlobalIdentity = user.UserGlobalidentity, phone = phone });
        }

        [HttpGet]
        public async Task<IActionResult> OtpConfirm(string userGlobalIdentity, int phone)
        {
            ViewBag.UserGlobalIdentity = userGlobalIdentity;
            ViewBag.Phone = phone;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OtpConfirm(string userGlobalIdentity, int phone, int otpCode)
        {
            ViewBag.UserGlobalIdentity = userGlobalIdentity;

            var user = await _userService.GetUserByUserGlobalIdentity(userGlobalIdentity);

            if (user is null || user.LastOtpSequence != otpCode)
            {
                TempData.AddNotification(new NotificationMessage
                {
                    Type = "Error",
                    Message = "Invalid OTP code."
                });

                return RedirectToAction("Login");
            }

            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("UserName", user.userName);
            HttpContext.Session.SetString("DisplayName", user.Staff != null ? user.Staff.StaffPopulatedName : user.Student.StudentPopulatedName);
            HttpContext.Session.SetString("UserType", user.UserTypeEnum.ToString());

            var appClaims = new List<Claim>
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.userName),
                new Claim(ClaimTypes.Name, user.Staff != null ? user.Staff.StaffPopulatedName : user.Student.StudentPopulatedName)
            };

            TempData.AddNotification(new NotificationMessage
            {
                Type = "Success",
                Message = "Login successful!"
            });

            TempData.AddNotification(new NotificationMessage
            {
                Type = "info",
                Message = $"Welcome {user?.Staff?.StaffPopulatedName ?? user?.Student?.StudentPopulatedName ?? "User"}"
            });

            if (user.UserTypeEnum == UserTypeEnum.Admin)
            {
                return RedirectToAction("Index", "Home", new { Area = "Adminportal" });

            }
            else if (user.UserTypeEnum == UserTypeEnum.Manager)
            {
                return RedirectToAction("Index", "Home", new { Area = "Adminportal" });
            }
            else if (user.UserTypeEnum == UserTypeEnum.Student)
            {
                return RedirectToAction("Index", "Home", new { Area = "Studentportal" });
            }
            else
            {
                TempData.AddNotification(new NotificationMessage
                {
                    Type = "error",
                    Message = "Your account did not match any type"
                });
                return RedirectToAction("Login");
            }
            
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
            //if (!ModelState.IsValid)
            //{
            //    return View(studentRegistration);
            //}

            var isEmailUnique = await _userService.GetUserByEmail(studentRegistration.Email);
            
            if (isEmailUnique != null)
            {
                TempData.AddNotification(new NotificationMessage
                {
                    Type = "Error",
                    Message = "This email is already registered.!"
                });
                return View(studentRegistration);
            }

            var (status, primaryKey) = await _studentRegistrationService.UpdateStudentRegistraion(studentRegistration);

            if (status)
            {
                var phone = studentRegistration.PhoneNo?.Replace("+", "");

                var message = $"Your registration with sensei japanese center was successfull. You will get notified when an admin approve your registration request.";

                var messageStatus = await _smsService.SendSingleAsync(phone, message);

                TempData.AddNotification(new NotificationMessage
                {
                    Type = "success",
                    Message = "Registration success!"
                });

                return RedirectToAction("Login");
            }

            TempData.AddNotification(new NotificationMessage
            {
                Type = "error",
                Message = "registration Failed!"
            });

            return View();
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

            var user = await _userService.GetUserByEmail(email);

            if(user is null)
            {
                TempData.AddNotification(new NotificationMessage
                {
                    Type = "Error",
                    Message = "Your account is not registered."
                });

                return RedirectToAction("Login");
            }

            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("UserName", user.userName);
            HttpContext.Session.SetString("DisplayName", string.IsNullOrWhiteSpace(name) ? "User" : name);
            HttpContext.Session.SetString("UserType", user.UserTypeEnum.ToString());

            var appClaims = new List<Claim>
            {
                new Claim("UserId", user.UserId.ToString()),
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


            if (user.UserTypeEnum == UserTypeEnum.Admin)
            {
                return RedirectToAction("Index", "Home", new { Area = "Adminportal" });

            }
            else if (user.UserTypeEnum == UserTypeEnum.Manager)
            {
                return RedirectToAction("Index", "Home", new { Area = "Adminportal" });
            }
            else if (user.UserTypeEnum == UserTypeEnum.Student)
            {
                return RedirectToAction("Index", "Home", new { Area = "Studentportal" });
            }
            else
            {
                TempData.AddNotification(new NotificationMessage
                {
                    Type = "error",
                    Message = "Your account did not match any type"
                });

                return RedirectToAction("Login");
            }
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

            var result = countries.OrderBy(e => e.CountryName).Select(e => new { id = e.CountryId, text = e.CountryName }).ToList();

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
