using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.Models.Entities;
using SIMS.Services.Interfaces;

namespace SIMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClassController : Controller
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClass(Class cls)
        {
            try
            {
                await _classService.CreateClassAsync(cls);
                TempData["SuccessMessage"] = "New class added successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error adding: " + ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateClass(Class cls)
        {
            try
            {
                await _classService.UpdateClassAsync(cls);
                TempData["SuccessMessage"] = "Class updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Update error: " + ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClass(int id)
        {
            try
            {
                await _classService.DeleteClassAsync(id);
                TempData["SuccessMessage"] = "Class deleted.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }
    }
}