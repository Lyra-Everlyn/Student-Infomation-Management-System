using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.Models.Entities;
using SIMS.Services.Implementations;
using SIMS.Services.Interfaces;
using System.Security.Claims;

namespace SIMS.Controllers
{
    [Authorize(Roles = "Faculty")]
    [Route("Faculty/Assignment")]
    public class AssignmentController : Controller
    {
        private readonly IFacultyService _facultyService;
        private readonly IAssignmentService _assignmentService;

        public AssignmentController(
            IFacultyService facultyService, 
            IAssignmentService assignmentService,
            IGradeService gradeService)
        {
            _facultyService = facultyService;
            _assignmentService = assignmentService;
        }

        // From faculty dashboard, faculty can manage assignments for a specific course
        [HttpGet("Manage/{courseId}")]
        public async Task<IActionResult> Manage(int courseId)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail)) return RedirectToAction("Login", "Auth");

            var viewModel = await _facultyService.GetCourseDetailsAsync(courseId, userEmail);

            if (viewModel == null) return RedirectToAction("Dashboard", "Faculty");

            return View("~/Views/Faculty/ManageAssignment.cshtml", viewModel);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAssignment(Assignment assignment)
        {
            await _assignmentService.CreateAssignmentAsync(assignment);
            TempData["Success"] = "Assignment successfully assigned!";
            return RedirectToAction("Manage", "Assignment", new { courseId = assignment.CourseId });
        }

        [HttpPost("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAssignment(Assignment assignment)
        {
            await _assignmentService.UpdateAssignmentAsync(assignment);
            TempData["Success"] = "Assignment updated successfully!";
            return RedirectToAction("Manage", "Assignment", new { courseId = assignment.CourseId });
        }

        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAssignment(int AssignmentId, int CourseId)
        {
            await _assignmentService.DeleteAssignmentAsync(AssignmentId);
            TempData["Success"] = "Assignment deleted successfully!";
            return RedirectToAction("Manage", "Assignment", new { courseId = CourseId });
        }
    }
}
