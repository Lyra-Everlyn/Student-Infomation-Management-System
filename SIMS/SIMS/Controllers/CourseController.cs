using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.Models.Entities;
using SIMS.Services.Interfaces;

namespace SIMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseManagerService;

        public CourseController(ICourseService courseManagerService)
        {
            _courseManagerService = courseManagerService;
        }

        [HttpGet]
        [Route("admin/api/department-data/{departmentId}")]
        public async Task<IActionResult> GetDepartmentData(int departmentId)
        {
            var data = await _courseManagerService.GetDepartmentFilterDataAsync(departmentId);
            return Json(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(Course course, int ClassId)
        {
            try
            {
                var result = await _courseManagerService.CreateCourseAsync(course, ClassId);
                TempData["SuccessMessage"] = result.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error creating course: " + ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCourse(Course course)
        {
            try
            {
                await _courseManagerService.UpdateCourseAsync(course);
                TempData["SuccessMessage"] = "Course updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating course: " + ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                var result = await _courseManagerService.DeleteCourseAsync(id);

                if (result.Success)
                    TempData["SuccessMessage"] = result.Message;
                else
                    TempData["ErrorMessage"] = result.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting course: " + ex.Message;
            }
            return RedirectToAction("AdminDashboard", "Admin");
        }
    }
}