using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS.Services.Interfaces;
using System.Security.Claims;

namespace SIMS.Controllers
{
    [Authorize(Roles = "Student")]
    [Route("Student/Assignment")]
    public class StudentAssignmentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentAssignmentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("List/{courseId}")]
        public async Task<IActionResult> CourseAssignments(int courseId)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail)) return RedirectToAction("Login", "Auth");

            var data = await _studentService.GetCourseAssignmentsAsync(courseId, userEmail);
            if (data.Course == null) return NotFound();

            ViewBag.Course = data.Course;
            ViewBag.MyGrades = data.MyGrades;

            return View("~/Views/Student/CourseAssignments.cshtml", data.Assignments);
        }


        [HttpGet("Detail/{assignmentId}")]
        public async Task<IActionResult> AssignmentDetail(int assignmentId)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail)) return RedirectToAction("Login", "Auth");

            var data = await _studentService.GetAssignmentDetailAsync(assignmentId, userEmail);
            if (data.Assignment == null) return NotFound();

            ViewBag.Student = data.Student;
            ViewBag.Submission = data.Submission;

            return View("~/Views/Student/AssignmentDetail.cshtml", data.Assignment);
        }

        [HttpPost("Submit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAssignment(int assignmentId, IFormFile fileUpload)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            var result = await _studentService.SubmitAssignmentAsync(assignmentId, userEmail!, fileUpload);

            if (result.Success) TempData["Success"] = result.Message;
            else TempData["Error"] = result.Message;

            return RedirectToAction("AssignmentDetail", "StudentAssignment", new { assignmentId = assignmentId });
        }
    }
}