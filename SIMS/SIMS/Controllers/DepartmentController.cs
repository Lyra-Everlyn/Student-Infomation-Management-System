using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.Models.Entities;
using SIMS.Services.Interfaces;

namespace SIMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDepartment(Department department)
        {
            try
            {
                await _departmentService.CreateDepartmentAsync(department);
                TempData["SuccessMessage"] = "Department successfully added!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error adding: " + ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDepartment(Department department)
        {
            try
            {
                await _departmentService.UpdateDepartmentAsync(department);
                TempData["SuccessMessage"] = "Department updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Update error: " + ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                await _departmentService.DeleteDepartmentAsync(id);
                TempData["SuccessMessage"] = "Department deleted.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }
    }
}