using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        public SenseiJapaneseSchoolController(ISmsService smsService)
        {
            _smsService = smsService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

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


        public async Task<IActionResult> Register()
        {
            return View();
        }

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
    }
}
