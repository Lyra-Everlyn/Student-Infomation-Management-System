using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.Services.Interfaces;
using System.Security.Claims;

namespace SIMS.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class FacultyController : Controller
    {
        private readonly IFacultyService _facultyService;

        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        [HttpGet("Faculty/Dashboard")]
        public async Task<IActionResult> FacultyDashboard()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail)) return RedirectToAction("Login", "Auth");

            var viewModel = await _facultyService.GetDashboardDataAsync(userEmail);
            if (viewModel == null) return RedirectToAction("Login", "Auth");

            return View(viewModel);
        }
    }
}
