using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIMS.Data;
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
        public async Task<IActionResult> CreateCourse(Course course, int classId, List<int> selectedStudentIds)
        {
            try
            {
                var result = await _courseManagerService.CreateCourseAsync(course, classId, selectedStudentIds);
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


        [HttpGet]
        [Route("/api/students/by-class/{classId}")]
        public async Task<IActionResult> GetStudentsByClass(int classId)
        {
            try
            {
                var students = await _courseManagerService.GetStudentsByClassIdAsync(classId);
                return Json(students);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error fetching students: " + ex.Message;
                return StatusCode(500, new { message = "Internal server error: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudentToCourse(int CourseId, string StudentCode)
        {
            try
            {
                var result = await _courseManagerService.AddStudentToCourseAsync(CourseId, StudentCode);
                if (result.Success)
                    TempData["SuccessMessage"] = result.Message;
                else
                    TempData["ErrorMessage"] = result.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error adding student: " + ex.Message;
            }
            return Redirect($"/Admin/Course/Details/{CourseId}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveStudentFromCourse(int CourseId, int StudentId)
        {
            try
            {
                var result = await _courseManagerService.RemoveStudentFromCourseAsync(CourseId, StudentId);
                if (result.Success)
                    TempData["SuccessMessage"] = result.Message;
                else
                    TempData["ErrorMessage"] = result.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error removing student: " + ex.Message;
            }
            return Redirect($"/Admin/Course/Details/{CourseId}");
        }

    }
}