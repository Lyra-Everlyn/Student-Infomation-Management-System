using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.Services.Interfaces;

namespace SIMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> AdminDashboard()
        {
            var viewModel = await _adminService.GetDashboardDataAsync();
            return View(viewModel);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("Admin/Course/Details/{courseId}")]
        public async Task<IActionResult> Details(int courseId)
        {
            var viewModel = await _adminService.ViewCourseDetailsAsync(courseId);
            if (viewModel == null)
            {
                TempData["Error"] = "Course not found!";
                return RedirectToAction("Dashboard", "Admin");
            }
            return View("~/Views/Admin/CourseDetails.cshtml", viewModel);
        }

    }
}