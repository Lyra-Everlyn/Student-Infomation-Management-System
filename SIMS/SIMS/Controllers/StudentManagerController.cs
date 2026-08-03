using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.Models.Entities;
using SIMS.Services.Interfaces;

namespace SIMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentManagerController : Controller
    {
        private readonly IStudentManagerService _studentManagerService;

        public StudentManagerController(IStudentManagerService studentManagerService)
        {
            _studentManagerService = studentManagerService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStudent(Student student)
        {
            try
            {
                await _studentManagerService.UpdateStudentAsync(student);
                TempData["SuccessMessage"] = "Update student profile successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating student profile: " + ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }
    }
}