using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using SIMS.Services.Interfaces; // Đã đổi theo namespace mới của bạn
using System.Security.Claims;

namespace SIMS_System.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // If user has been login in, -> redirect to dashboard by role
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var role = User.FindFirstValue(ClaimTypes.Role);
                return RedirectToDashboard(role);
            }
            return View();
        }


        // 1. Login manual (Email, Password)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) {
                ModelState.AddModelError(string.Empty, "Please enter email and password");
                return View();
            }

            try {

                var user = await _authService.AuthenticateAsync(email, password);
                await SignInUser(user!.Email, user.FullName, user.Role);
                return RedirectToDashboard(user.Role);
            }
            catch (Exception ex) {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        // 2. Login by Google
        [HttpGet]
        [Route("Auth/LoginGoogle")]
        public IActionResult LoginGoogle()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                TempData["ErrorMessage"] = "Authentication Google failed";
                return RedirectToAction("Login", "Auth");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (email == null) {
                TempData["ErrorMessage"] = "Can't get email from your Google account";
                return RedirectToAction("Login", "Auth");
            }

            try {
                var user = await _authService.AuthenticateGoogleUserAsync(email, name ?? "Unknown");
                await SignInUser(user.Email, user.FullName, user.Role);

                return RedirectToDashboard(user.Role);
            }
            catch (Exception ex) {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Login", "Auth");
            }
        }


        // 3. Log out
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }


        // Helper method set up Cookie
        private async Task SignInUser(string email, string fullName, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, fullName),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        // Helper method redirect by Role
        private IActionResult RedirectToDashboard(string? role)
        {
            return role switch
            {
                "Admin" => RedirectToAction("AdminDashboard", "Admin"),
                "Faculty" => RedirectToAction("FacultyDashboard", "Faculty"),
                "Student" => RedirectToAction("StudentDashboard", "Student"),
                "Applicant" => RedirectToAction("Profile", "Applicant"),
                _ => RedirectToAction("Logout", "Auth")
            };
        }
    }
}