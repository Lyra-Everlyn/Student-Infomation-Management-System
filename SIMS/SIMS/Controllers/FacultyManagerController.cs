using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.Models.Entities;
using SIMS.Services.Interfaces;

namespace SIMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FacultyManagerController : Controller
    {
        private readonly IFacultyManagerService _facultyManagerService;

        public FacultyManagerController(IFacultyManagerService facultyManagerService)
        {
            _facultyManagerService = facultyManagerService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFaculty(Faculty faculty)
        {
            try
            {
                await _facultyManagerService.CreateFacultyAsync(faculty);
                TempData["SuccessMessage"] = "Faculty assigned successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error assigning faculty: " + ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFaculty(Faculty faculty)
        {
            try
            {
                await _facultyManagerService.UpdateFacultyAsync(faculty);
                TempData["SuccessMessage"] = "Faculty profile updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating faculty profile: " + ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }
    }
}