using Microsoft.AspNetCore.Mvc;
using SIMS.Models.DTOs;
using SIMS.Models.Entities;
using SIMS.Repositories.Interfaces;
using SIMS.Services.Interfaces;
using System.Security.Claims;

namespace SIMS.Controllers
{
    public class ApplicantController : Controller
    {
        private readonly IApplicantService _applicantService;
        private readonly IRepository<User> _userRepo;

        public ApplicantController(IApplicantService applicantService, IRepository<User> userRepo)
        {
            _applicantService = applicantService;
            _userRepo = userRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var users = await _userRepo.GetAllAsync();
            var currentUser = users.FirstOrDefault(u => u.Email == email);

            if (currentUser == null) return RedirectToAction("Login", "Auth");

            var application = await _applicantService.GetApplicationByUserIdAsync(currentUser.UserId);
            var departments = await _applicantService.GetAllDepartmentsAsync();

            ViewBag.Departments = departments;
            ViewBag.ApplicationStatus = application?.Status ?? "Incomplete";

            ViewBag.SelectedDepartmentId = application?.DepartmentId;

            return View(currentUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitProfile(UpdateProfileRequest request)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var users = await _userRepo.GetAllAsync();
            var currentUser = users.FirstOrDefault(u => u.Email == email);

            if (currentUser == null) return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all the required information.";
                return RedirectToAction(nameof(Profile));
            }

            try
            {
                await _applicantService.UpdateProfileAndSubmitAsync(currentUser.UserId, request);
                TempData["SuccessMessage"] = "Application successfully completed!";
                return RedirectToAction(nameof(Profile));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Profile));
            }
        }
    }
}
